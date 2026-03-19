using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Diamond.DataSource.Models;
using System.Linq.Dynamic.Core.CustomTypeProviders;

namespace Diamond.DataSource.Extensions
{
    public static class QueryResultExtension
    {
        public static QueryResult ToQueryResult(this IQueryable data, QueryRequest queryRequest, bool isFilter = true)
        {
            var result = new QueryResult();
            var isGroupBy = false;

            if (queryRequest == null)
            {
                result.Total = data.Count();
                result.Data = data;
                return result;
            }

            if (isFilter)
            {
                if (queryRequest.Filter != null)
                {
                    var str = FilterDescriptorQuery(queryRequest.Filter);
                    // System.Diagnostics.Debug.WriteLine(data.ToString() + "_" + str);
                    // Console.WriteLine(data.ToString() + "_" + str);
                    
                    if(str != "()") {
                        data = data.Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, FilterDescriptorQuery(queryRequest.Filter));
                    }
                }
            }

            if (queryRequest.Sort != null)
            {
                var ordering = String.Join(",", queryRequest.Sort.Select(s => s.Field + " " + (s.Direction == SortDirection.Ascending ? "ascending" : "descending")));
                data = data.OrderBy(ordering);
            }

            if (queryRequest.Count != null && queryRequest.Count.Value)
            {
                result.Total = data.Count();
            }

            if (queryRequest.GroupBy != null && queryRequest.GroupBy.Any())
            {
                isGroupBy = true;
                var groupBySelector = string.Empty;
                var selectSelector = string.Empty;
                foreach (var groupBy in queryRequest.GroupBy)
                {
                    if (!string.IsNullOrWhiteSpace(groupBySelector))
                    {
                        groupBySelector += ",";
                    }
                    groupBySelector += groupBy.Field;
                    selectSelector += string.Format("it.Key as {0}", groupBy.Field);
                }
                data = data.GroupBy(groupBySelector, "it").Select(string.Format("new({0})", selectSelector));
            }

            if (!isGroupBy && queryRequest.Select != null && queryRequest.Select.Any())
            {
                var selector = string.Empty;
                foreach (var select in queryRequest.Select)
                {
                    if (!string.IsNullOrWhiteSpace(selector))
                    {
                        selector += ",";
                    }
                    selector += select.Field;
                }
                data = data.Select(string.Format("new({0})", selector));
            }

            if (queryRequest.Skip.HasValue && queryRequest.Skip != 0)
            {
                data = data.Skip(queryRequest.Skip.Value);
            }

            if (queryRequest.Take.HasValue && queryRequest.Take != 0)
            {
                data = data.Take(queryRequest.Take.Value);
            }
            
            var list = data.ToDynamicList();

            if (queryRequest.Count == null || !queryRequest.Count.Value)
            {
                result.Total = list.Count();
            }
            result.Data = list;
            return result;
        }

        public static IQueryable<T> ToWhere<T>(this IQueryable<T> data, QueryRequest queryRequest)
        {
            if (queryRequest.Filter != null)
            {
                data = data.Where(FilterDescriptorQuery(queryRequest.Filter));
            }
            return data;
        }
        public static string ToSelect(this QueryRequest queryRequest)
        {
            if (queryRequest.Select != null && queryRequest.Select.Any())
            {
                var selector = string.Empty;
                foreach (var select in queryRequest.Select)
                {
                    if (!string.IsNullOrWhiteSpace(selector))
                    {
                        selector += ",";
                    }
                    selector += select.Field;
                }
                return string.Format("new({0})", selector);
            }
            else
            {
                return null;
            }
        }
        public static string ToWhere(this QueryRequest queryRequest)
        {
            if (queryRequest == null)
            {
                return null;
            }
            return FilterDescriptorQuery(queryRequest.Filter);
        }
        public static string ToSort(this QueryRequest queryRequest)
        {
            if (queryRequest.Sort == null)
            {
                return null;
            }
            return String.Join(",", queryRequest.Sort.Select(s => s.Field + " " + (s.Direction == SortDirection.Ascending ? "ascending" : "descending")));
        }

        private static string FilterDescriptorQuery(IFilterDescriptor descriptor)
        {
            var type = descriptor.GetType();

            if (type == typeof(FilterDescriptor))
            {
                return FilterDescriptorQuery(descriptor as FilterDescriptor);
            }
            else if (type == typeof(CompositeFilterDescriptor))
            {
                return FilterDescriptorQuery(descriptor as CompositeFilterDescriptor);
            }

            return "";
        }

        private static string FilterDescriptorQuery(FilterDescriptor descriptor)
        {
            var field = string.IsNullOrEmpty(descriptor.Field) ? "it" : descriptor.Field;


            var value = string.Empty;

            if( descriptor.Value != null ) {
                var valueType = descriptor.Value.GetType();
                if (valueType == typeof(string))
                {
                    var statement = descriptor.Value.ToString();
                    statement = statement.Replace("\"", "\\\"");
                    
                    // value = string.Format("\"{0}\"", descriptor.Value);
                    value = string.Format("\"{0}\"", statement);
                }
                else if (valueType == typeof(DateTime))
                {
                    var dateTime = (DateTime) descriptor.Value;
                    dateTime = dateTime.ToLocalTime();
                    value = string.Format("DateTime({0}, {1}, {2}, {3}, {4}, {5})",
                        dateTime.Year,
                        dateTime.Month,
                        dateTime.Day,
                        dateTime.Hour,
                        dateTime.Minute,
                        dateTime.Second
                    );
                }
                else
                {
                    value = descriptor.Value.ToString();
                }
            } else {
                value = "null";
            }


            switch (descriptor.Operator)
            {
                case FilterOperator.IsEqualTo:
                case FilterOperator.IsNotEqualTo:
                case FilterOperator.IsGreaterThan:
                case FilterOperator.IsGreaterThanOrEqualTo:
                case FilterOperator.IsLessThan:
                case FilterOperator.IsLessThanOrEqualTo:
                    return string.Format("{0} {1} {2}",
                        field, FilterOperatorQuery(descriptor.Operator), value);
                // case FilterOperator.StartsWith:
                // case FilterOperator.EndsWith:
                //     return string.Format("{0}.ToLower().{1}({2})",
                //         field, FilterOperatorQuery(descriptor.Operator), value.ToLower());
                // case FilterOperator.Contains:
                //     return FilterContainStatement(field, value);
                case FilterOperator.StartsWith:
                case FilterOperator.EndsWith:
                case FilterOperator.Contains:
                    return FilterFuzzyStatement(field, descriptor.Operator, value);
            }

            return null;
        }
        private static string FilterFuzzyStatement(string field, FilterOperator filterOperator,string statement)
        {
            // remove \" start & end char
            statement = statement.Substring(1, statement.Length -2 );
            // statement = statement.Replace("\"", "\\\"");

            var predicate = "";
            var conditions = statement.Split('%', StringSplitOptions.RemoveEmptyEntries);
            if (conditions.Length == 1)
            {
                switch (filterOperator)
                {
                    case FilterOperator.StartsWith:
                        predicate = string.Format("{0}.StartsWith(\"{1}\")", field, statement.ToLower());
                        break;
                    case FilterOperator.EndsWith:
                        predicate = string.Format("{0}.EndsWith(\"{1}\")", field, statement.ToLower());
                        break;
                    case FilterOperator.Contains:
                        predicate = string.Format("EF.Functions.Like({0}, \"%{1}%\")", field, statement.ToLower());
                        break;
                }                
            }
            else
            {
                // for (var i = 0; i < conditions.Length; i++)
                // {
                //     conditions[i] = string.Format("{0}.ToLower().Contains(\"{1}\")", field, conditions[i].ToLower());
                // }
                // predicate = String.Join("&&", conditions);

                // predicate = statement;
                predicate = string.Format("EF.Functions.Like({0}, \"{1}%\")", field, statement);
            }

            return predicate;
        }
        // private static string FilterContainStatement(string field, string statement)
        // {
        //     // remove \" start & end char
        //     statement = statement.Substring(1, statement.Length -2 );
        //     statement = statement.Replace("\"", "\\\"");

        //     var predicate = "";
        //     var conditions = statement.Split('%', StringSplitOptions.RemoveEmptyEntries);
        //     if (conditions.Length == 1)
        //     {
        //         // predicate = string.Format("{0}.ToLower().Contains(\"{1}\")", field, statement.ToLower());
        //         predicate = string.Format("EF.Functions.Like({0}, \"%{1}%\")", field, statement);
                
        //     }
        //     else
        //     {
        //         for (var i = 0; i < conditions.Length; i++)
        //         {
        //             conditions[i] = string.Format("{0}.ToLower().Contains(\"{1}\")", field, conditions[i].ToLower());
        //             // conditions[i] = string.Format("EF.Functions.Like({0}, \"%{1}%\")", field, statement);
        //         }
        //         predicate = String.Join("&&", conditions);
        //     }

        //     return predicate;
        // }
        private static string FilterOperatorQuery(FilterOperator filterOperator)
        {
            switch (filterOperator)
            {
                case FilterOperator.IsEqualTo:
                    return "==";
                case FilterOperator.IsNotEqualTo:
                    return "!=";
                case FilterOperator.IsGreaterThan:
                    return ">";
                case FilterOperator.IsGreaterThanOrEqualTo:
                    return ">=";
                case FilterOperator.IsLessThan:
                    return "<";
                case FilterOperator.IsLessThanOrEqualTo:
                    return "<=";
                case FilterOperator.StartsWith:
                    return "StartsWith";
                case FilterOperator.EndsWith:
                    return "EndsWith";
                case FilterOperator.Contains:
                    return "Contains";
            }

            return null;
        }

        private static string FilterDescriptorQuery(CompositeFilterDescriptor descriptor)
        {
            var query = string.Empty;
            var logicQuery = descriptor.Logic == FilterCompositionLogicalOperator.And ? "&&" : "||";
            foreach (var item in descriptor.Filters)
            {
                query += (string.IsNullOrWhiteSpace(query) ? string.Empty : logicQuery) + FilterDescriptorQuery(item);
            }
            return "(" + query + ")";
        }
    }

    public class DynamicLinqProvider : DefaultDynamicLinqCustomTypeProvider
    {
        public override HashSet<Type> GetCustomTypes()
        {
            HashSet<Type> types = new HashSet<Type>();
            types.Add(typeof(EF));
            types.Add(typeof(DbFunctionsExtensions));
            return types;
        }
    }

}