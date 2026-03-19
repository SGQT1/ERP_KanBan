using Diamond.DataSource.Models;
using Diamond.DataSource.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.DataSource.Webs
{
    public class QueryRequestBinder : IModelBinder
    {
        private JsonSerializerSettings SerializerSettings
        {
            get
            {
                var settings = new JsonSerializerSettings();
                settings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                return settings;
            }
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var queryRequest = new QueryRequest();
            if (!bindingContext.HttpContext.Request.QueryString.HasValue)
            {
                return Task.CompletedTask;
            }
            var queryString = bindingContext.HttpContext.Request.QueryString.Value.Remove(0, 1);
            queryString = HttpUtility.UrlDecode(queryString);

            var map = JsonConvert.DeserializeObject<Dictionary<string, object>>(queryString);
            if (map.Any(e => e.Key == "hasCount"))
            {
                var count = Convert.ToString(map["hasCount"]).ToLower();
                queryRequest.Count = JsonConvert.DeserializeObject<bool>(count);
            }
            if (map.Any(e => e.Key == "take"))
            {
                queryRequest.Take = JsonConvert.DeserializeObject<int>(Convert.ToString(map["take"]));
            }
            if (map.Any(e => e.Key == "skip"))
            {
                queryRequest.Skip = JsonConvert.DeserializeObject<int>(Convert.ToString(map["skip"]));
            }
            if (map.Any(e => e.Key == "sort"))
            {
                var sortMapList = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(Convert.ToString(map["sort"]));
                queryRequest.Sort = new List<SortDescriptor>();
                foreach (var sortMap in sortMapList)
                {
                    (queryRequest.Sort as List<SortDescriptor>).Add(new SortDescriptor
                    {
                        Field = sortMap["field"],
                        Direction = sortMap["dir"] == "asc" ? SortDirection.Ascending : SortDirection.Descending
                    });
                }
            }
            if (map.Any(e => e.Key == "filter"))
            {
                var filterMap = JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(map["filter"]),
                    SerializerSettings);
                queryRequest.Filter = GetFilterDescriptor(filterMap);
            }
            if (map.Any(e => e.Key == "select"))
            {
                var selectMapList = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(Convert.ToString(map["select"]));
                queryRequest.Select = new List<FieldDescriptor>();
                foreach (var selectMap in selectMapList)
                {
                    (queryRequest.Select as List<FieldDescriptor>).Add(new FieldDescriptor
                    {
                        Field = selectMap["field"]
                    });
                }
            }
            if (map.Any(e => e.Key == "groupBy"))
            {
                var groupByMapList = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(Convert.ToString(map["groupBy"]));
                queryRequest.GroupBy = new List<FieldDescriptor>();
                foreach (var groupByMap in groupByMapList)
                {
                    (queryRequest.GroupBy as List<FieldDescriptor>).Add(new FieldDescriptor
                    {
                        Field = groupByMap["field"]
                    });
                }
            }
            if (map.Any(e => e.Key == "extention"))
            {
                var extentionMapList = JsonConvert.DeserializeObject<string[]>(Convert.ToString(map["extention"]));
                queryRequest.Extention = extentionMapList;
            }

            bindingContext.Result = ModelBindingResult.Success(queryRequest);

            this.QueryRequestLog(bindingContext, queryRequest);

            return Task.CompletedTask;
        }

        private IFilterDescriptor GetFilterDescriptor(Dictionary<string, object> map)
        {
            if (map.Any(e => e.Key == "logic"))
            {
                return BuildCompositeFilterDescriptor(map);
            }
            return BuildFilterDescriptor(map);
        }

        private FilterDescriptor BuildFilterDescriptor(Dictionary<string, object> map)
        {
            var filter = new FilterDescriptor();
            filter.Field = map["field"] as string;
            switch (map["operator"] as string)
            {
                case "eq":
                    filter.Operator = FilterOperator.IsEqualTo;
                    break;
                case "neq":
                    filter.Operator = FilterOperator.IsNotEqualTo;
                    break;
                case "lt":
                    filter.Operator = FilterOperator.IsLessThan;
                    break;
                case "lte":
                    filter.Operator = FilterOperator.IsLessThanOrEqualTo;
                    break;
                case "gt":
                    filter.Operator = FilterOperator.IsGreaterThan;
                    break;
                case "gte":
                    filter.Operator = FilterOperator.IsGreaterThanOrEqualTo;
                    break;
                case "startswith":
                    filter.Operator = FilterOperator.StartsWith;
                    break;
                case "endswith":
                    filter.Operator = FilterOperator.EndsWith;
                    break;
                case "contains":
                    filter.Operator = FilterOperator.Contains;
                    break;
            }
            filter.Value = map["value"];
            return filter;
        }

        private CompositeFilterDescriptor BuildCompositeFilterDescriptor(Dictionary<string, object> map)
        {
            var filter = new CompositeFilterDescriptor();
            filter.Logic = (map["logic"] as string) == "and" ? FilterCompositionLogicalOperator.And : FilterCompositionLogicalOperator.Or;
            filter.Filters = new List<IFilterDescriptor>();
            var filtersJson = JsonConvert.SerializeObject(map["filters"]);
            var filters = JsonConvert.DeserializeObject<Dictionary<string, object>[]>(filtersJson, SerializerSettings);
            foreach (var item in filters)
            {
                (filter.Filters as List<IFilterDescriptor>).Add(GetFilterDescriptor(item));
            }
            return filter;
        }

        // private void QueryRequestLog(ModelBindingContext bindingContext, QueryRequest queryRequest)
        // {
        //     var log = "QueryLog >>>[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]_[" 
        //                              + bindingContext.HttpContext.Connection.RemoteIpAddress.ToString() + "]_["
        //                              + bindingContext.HttpContext.Request.Path + "]_["
        //                              + queryRequest.ToWhere()+ "]";

        //     System.Diagnostics.Debug.WriteLine(log);
        //     Console.WriteLine(log);
        // }
        private void QueryRequestLog(ModelBindingContext bindingContext, QueryRequest queryRequest)
        {
            var log = $"QueryLog >>>[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]_[{bindingContext.HttpContext.Connection.RemoteIpAddress}]_[{bindingContext.HttpContext.Request.Path}]_[{queryRequest.ToWhere()}]";
            Console.WriteLine(log); // 進 stdout 檔
            bindingContext.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("QueryLog").LogInformation(log); // 進 Console logger（同樣會進檔）
        }
    }
}
