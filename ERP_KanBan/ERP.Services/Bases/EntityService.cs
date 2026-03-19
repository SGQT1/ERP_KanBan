using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using Microsoft.EntityFrameworkCore.Query;

namespace ERP.Services.Bases
{
    public abstract class EntityService<T> where T : class
    {
        protected virtual BaseRepository<T> Repository { get; }

        public EntityService(BaseRepository<T> repository)
        {
            Repository = repository;
        }

        public virtual IQueryable<T> Get()
        {
            return Repository.Get();
        }

        public T Create(T item)
        {
            return Repository.Create(item);
        }
        public T CreateKeepId(T item)
        {
            return Repository.CreateKeepId(item);
        }
        public void CreateRange(IEnumerable<T> items)
        {
            Repository.CreateRange(items);
        }
        public void CreateRangeKeepId(IEnumerable<T> items)
        {
            Repository.CreateRangeKeepId(items);
        }
        public void CreateRangeKeepAll(IEnumerable<T> items)
        {
            Repository.CreateRangeKeepAll(items);
        }
        public void CreateRangeKeepTime(IEnumerable<T> items)
        {
            Repository.CreateRangeKeepTime(items);
        }


        public T Update(T item)
        {
            return Repository.Update(item);
        }
        public void UpdateRange(IEnumerable<T> items)
        {
            Repository.UpdateRange(items);
        }
        // public void UpdateRange(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> upatePredicate)
        // {
        //     Repository.UpdateRange(predicate, upatePredicate);
        // }
        public void UpdateRange(Expression<Func<T, bool>> predicate, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> upatePredicate)
        {
            Repository.UpdateRange(predicate, upatePredicate);
        }
        
        
        public void ExecuteSqlCommand(string predicate)
        {
            Repository.ExecuteSqlCommand(predicate);
        }
        public void Remove(T item)
        {
            Repository.Remove(item);
        }

        public void RemoveRange(Expression<Func<T, bool>> predicate)
        {
            Repository.RemoveRange(predicate);
        }
    }

    // public class DynamicLinqProvider : DefaultDynamicLinqCustomTypeProvider
    // {
    //     public override HashSet<Type> GetCustomTypes()
    //     {
    //         HashSet<Type> types = new HashSet<Type>();
    //         types.Add(typeof(EF));
    //         types.Add(typeof(DbFunctionsExtensions));
    //         return types;
    //     }
    // }
}