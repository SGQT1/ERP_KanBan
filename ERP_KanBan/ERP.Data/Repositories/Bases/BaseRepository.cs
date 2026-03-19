using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
// using Z.EntityFramework.Plus;

namespace ERP.Data.Repositories.Bases
{
    public abstract class BaseRepository<T> where T : class
    {
        protected DbContext Db { get; }

        public BaseRepository(DbContext db)
        {
            Db = db;
        }

        public virtual IQueryable<T> Get()
        {
            return Db.Set<T>();
        }

        public virtual T Create(T item)
        {
            if (item.GetType().GetProperty("LastUpdateTime") != null)
            {
                item.GetType().GetProperty("LastUpdateTime").SetValue(item, DateTime.Now);
            }

            var entity = Db.Set<T>().Add(NextValue(item));
            Db.SaveChanges();
            return entity.Entity;
        }
        public virtual T CreateKeepId(T item)
        {
            var type = typeof(T);

            // 設定 LastUpdateTime
            var lastUpdateProp = type.GetProperty("LastUpdateTime");
            if (lastUpdateProp != null && lastUpdateProp.CanWrite)
            {
                lastUpdateProp.SetValue(item, DateTime.Now);
            }

            // 判斷 Id 是否需要取號
            var idProp = type.GetProperty("Id");
            bool needGenerateId = idProp != null && (idProp.GetValue(item) == null || Convert.ToDecimal(idProp.GetValue(item)) == 0
            );

            if (needGenerateId)
            {
                item = NextValue(item); // 自動編號
            }

            var entity = Db.Set<T>().Add(item);
            Db.SaveChanges();
            return entity.Entity;
        }

        //不保留Id, 不保留LastUpdateTime
        public virtual void CreateRange(IEnumerable<T> items)
        {
            var _items = items.ToList();
            if (_items.Count() > 0)
            {
                decimal from = 0, to = 0;

                NextValueRange(_items.FirstOrDefault(), _items.Count(), out from, out to);

                if (from != 0 && to != 0)
                {
                    _items.ForEach(item =>
                    {
                        from += 1;
                        item.GetType().GetProperty("Id").SetValue(item, from);

                        if (item.GetType().GetProperty("LastUpdateTime") != null)
                        {
                            item.GetType().GetProperty("LastUpdateTime").SetValue(item, DateTime.Now);
                        }
                    });
                }
                else
                {
                    _items.ForEach(item =>
                    {
                        item.GetType().GetProperty("Id").SetValue(item, null);
                        if (item.GetType().GetProperty("LastUpdateTime") != null)
                        {
                            item.GetType().GetProperty("LastUpdateTime").SetValue(item, DateTime.Now);
                        }
                    });
                }
                Db.Set<T>().AddRange(_items);
            }
            Db.SaveChanges();
        }
        //保留Id, 不保留LastUpdateTime
        public virtual void CreateRangeKeepId(IEnumerable<T> items)
        {
            var _items = items.ToList();
            if (_items.Count == 0) return;

            // 將要自動編號的資料篩選出來 (Id 為 null 或 0)
            var needGenerateId = _items
                .Where(item =>
                    item.GetType().GetProperty("Id")?.GetValue(item) is null ||
                    Convert.ToDecimal(item.GetType().GetProperty("Id").GetValue(item)) == 0)
                .ToList();

            // 執行取號邏輯（只針對需要取號的資料）
            if (needGenerateId.Count > 0)
            {
                decimal from = 0, to = 0;
                NextValueRange(needGenerateId.First(), needGenerateId.Count, out from, out to);
                if (from != 0 && to != 0)
                {
                    foreach (var item in needGenerateId)
                    {
                        from += 1;
                        item.GetType().GetProperty("Id")?.SetValue(item, from);
                    }
                }
            }

            // 所有項目都更新 LastUpdateTime
            foreach (var item in _items)
            {
                item.GetType().GetProperty("LastUpdateTime")?.SetValue(item, DateTime.Now);
            }

            // 新增資料
            Db.Set<T>().AddRange(_items);
            Db.SaveChanges();
        }
        //保留Id, 保留LastUpdateTime
        public virtual void CreateRangeKeepAll(IEnumerable<T> items)
        {
            var _items = items.ToList();
            if (_items.Count == 0) return;

            // 將要自動編號的資料篩選出來 (Id 為 null 或 0)
            var needGenerateId = _items
                .Where(item =>
                    item.GetType().GetProperty("Id")?.GetValue(item) is null ||
                    Convert.ToDecimal(item.GetType().GetProperty("Id").GetValue(item)) == 0)
                .ToList();

            // 執行取號邏輯（只針對需要取號的資料）
            if (needGenerateId.Count > 0)
            {
                decimal from = 0, to = 0;
                NextValueRange(needGenerateId.First(), needGenerateId.Count, out from, out to);
                if (from != 0 && to != 0)
                {
                    foreach (var item in needGenerateId)
                    {
                        from += 1;
                        item.GetType().GetProperty("Id")?.SetValue(item, from);
                    }
                }
            }

            // 只更新 LastUpdateTime
            _items.ForEach(item =>
            {
                if (item.GetType().GetProperty("LastUpdateTime") != null && Convert.ToDateTime(item.GetType().GetProperty("LastUpdateTime").GetValue(item)) == null)
                {
                    item.GetType().GetProperty("LastUpdateTime").SetValue(item, DateTime.Now);
                }
            });

            // 新增資料
            Db.Set<T>().AddRange(_items);
            Db.SaveChanges();
        }
        //不保留Id, 保留LastUpdateTime
        public virtual void CreateRangeKeepTime(IEnumerable<T> items)
        {
            var _items = items.ToList();
            if (_items.Count() > 0)
            {
                decimal from = 0, to = 0;

                NextValueRange(_items.FirstOrDefault(), _items.Count(), out from, out to);

                if (from != 0 && to != 0)
                {
                    _items.ForEach(item =>
                    {
                        from += 1;
                        item.GetType().GetProperty("Id").SetValue(item, from);

                        if (item.GetType().GetProperty("LastUpdateTime") != null && Convert.ToDateTime(item.GetType().GetProperty("LastUpdateTime").GetValue(item)) == null)
                        {
                            item.GetType().GetProperty("LastUpdateTime").SetValue(item, DateTime.Now);
                        }
                    });
                }
                else
                {
                    _items.ForEach(item =>
                    {
                        item.GetType().GetProperty("Id").SetValue(item, null);
                        if (item.GetType().GetProperty("LastUpdateTime") != null && Convert.ToDateTime(item.GetType().GetProperty("LastUpdateTime").GetValue(item)) == null)
                        {
                            item.GetType().GetProperty("LastUpdateTime").SetValue(item, DateTime.Now);
                        }
                    });
                }
                Db.Set<T>().AddRange(_items);
            }
            Db.SaveChanges();
        }

        public virtual T Update(T item)
        {
            if (item.GetType().GetProperty("LastUpdateTime") != null)
            {
                item.GetType().GetProperty("LastUpdateTime").SetValue(item, DateTime.Now);
            }

            // find the exist Entity and update
            var entityEntry = Db.Entry(item);
            if (entityEntry.State == EntityState.Detached)
            {
                var keyValues = Db.Model.FindEntityType(typeof(T))
                                        .FindPrimaryKey().Properties
                                        .Select(p => entityEntry.Property(p.Name).CurrentValue)
                                        .ToArray();

                var existingEntity = Db.Set<T>().Find(keyValues);
                if (existingEntity != null)
                {
                    Db.Entry(existingEntity).CurrentValues.SetValues(item);
                    Db.Entry(existingEntity).State = EntityState.Modified;
                }
                else
                {
                    entityEntry = Db.Set<T>().Update(item);
                }
            }
            else
            {
                entityEntry = Db.Set<T>().Update(item);
            }

            Db.SaveChanges();
            return entityEntry.Entity;
        }
        public virtual void UpdateRange(IEnumerable<T> items)
        {
            var _items = items.ToList();
            if (_items.Count() > 0)
            {
                _items.ForEach(item =>
                {
                    if (item.GetType().GetProperty("LastUpdateTime") != null)
                    {
                        item.GetType().GetProperty("LastUpdateTime").SetValue(item, DateTime.Now);
                    }
                });

                Db.Set<T>().UpdateRange(_items);
            }

            Db.SaveChanges();
        }
        public virtual int UpdateRange(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> upatePredicate)
        {
            // Db.Set<T>().Where(predicate).Update(upatePredicate);
            // Db.SaveChanges();
            var setters = ToSetters(upatePredicate);
            return Db.Set<T>().Where(predicate).ExecuteUpdate(setters);
        }
        public virtual int UpdateRange(Expression<Func<T, bool>> predicate, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setProps)
        {
            return Db.Set<T>().Where(predicate).ExecuteUpdate(setProps);
        }
        public virtual void Remove(T item)
        {
            var entity = Db.Set<T>().Remove(item);
            Db.SaveChanges();
        }
        public virtual int RemoveRange(Expression<Func<T, bool>> predicate)
        {
            // var entity = Db.Set<T>().Where(predicate).Delete();
            // Db.SaveChanges();
            return Db.Set<T>().Where(predicate).ExecuteDelete();

        }
        public virtual void ExecuteSqlCommand(string predicate)
        {
            Db.Database.ExecuteSqlRaw(predicate);
        }
        private T NextValue(T item)
        {

            if (item.GetType().GetProperty("Id") != null)
            {
                decimal Id;
                if (decimal.TryParse(item.GetType().GetProperty("Id").GetValue(item, null).ToString(), out Id) == true)
                {
                    var tableName = item.GetType().Name;
                    // 特殊判斷，全面上線後要修改
                    if (tableName == "ArticlePart")
                    {
                        tableName = "ProjectArticlePart";
                    }

                    var entity = Db.Set<SerialNo>().AsNoTracking().FirstOrDefault(i => i.FormName == tableName);

                    if (entity != null)
                    {
                        var nextValue = entity.CurrentValue + 1;
                        entity.CurrentValue = nextValue;

                        // Db.Entry(entity).Property(e => e.CurrentValue).IsModified = true;

                        // tranking issue, clean tranking state.
                        var local = Db.Set<SerialNo>().Local.FirstOrDefault(i => i.FormName == tableName);
                        if (local != null)
                        {
                            Db.Entry(local).State = EntityState.Detached;
                        }

                        Db.Set<SerialNo>().Update(entity);
                        Db.SaveChanges();

                        item.GetType().GetProperty("Id").SetValue(item, nextValue);
                    }
                    else
                    {
                        item.GetType().GetProperty("Id").SetValue(item, null);
                    }
                    return item;
                }
            }

            return item;

        }
        private void NextValueRange(T item, int size, out decimal from, out decimal to)
        {
            from = to = 0;
            var tableName = item.GetType().Name;

            // 特殊判斷，全面上線後要修改
            if (tableName == "ArticlePart")
            {
                tableName = "ProjectArticlePart";
            }

            // 檢查是否已追蹤此實體
            var trackedEntity = Db.ChangeTracker.Entries<SerialNo>()
                .FirstOrDefault(e => e.Entity.FormName == tableName)?.Entity;

            SerialNo entity;

            if (trackedEntity != null)
            {
                entity = trackedEntity;
            }
            else
            {
                entity = Db.Set<SerialNo>().AsNoTracking() // 使用 AsNoTracking 以避免重複追蹤
                          .FirstOrDefault(i => i.FormName == tableName);
            }

            if (entity != null)
            {
                from = entity.CurrentValue;
                to = from + size;
                entity.CurrentValue = to;

                // 將實體附加到 DbContext 並標記為已修改
                Db.Attach(entity);
                Db.Entry(entity).Property(e => e.CurrentValue).IsModified = true;

                Db.SaveChanges();
            }
        }

        // 將 x => new T { A=..., B=... } 轉成：s => s.SetProperty(e => e.A, ...).SetProperty(e => e.B, ...)
        private static Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> ToSetters(Expression<Func<T, T>> updateProjection)
        {
            if (updateProjection.Body is not MemberInitExpression mi)
                throw new NotSupportedException("updateProjection 必須是 x => new T { ... } 的 MemberInit 寫法。");

            var sParam = Expression.Parameter(typeof(SetPropertyCalls<T>), "s"); // s =>
            Expression chain = sParam;

            var eParam = Expression.Parameter(typeof(T), "e");                    // e =>

            foreach (var binding in mi.Bindings)
            {
                if (binding is not MemberAssignment assign) continue;

                var prop = (PropertyInfo)assign.Member;
                var propType = prop.PropertyType;

                // e => e.Prop
                var propAccess = Expression.Lambda(
                    Expression.MakeMemberAccess(eParam, prop),
                    eParam);

                // 把原本 updateProjection 的參數 x 換成 e
                var rhsBody = new ReplaceParameterVisitor(updateProjection.Parameters[0], eParam)
                    .Visit(assign.Expression)!;

                // 取得 SetProperty 的 MethodInfo（常數版本 or lambda 版本）
                if (rhsBody is ConstantExpression c)
                {
                    // 常數版本：SetProperty<TEntity,TProp>(SetPropertyCalls<TEntity>, Expr<Func<TEntity,TProp>>, TProp)
                    var miSetConst = ResolveSetPropertyMethod(useLambda: false)
                        .MakeGenericMethod(typeof(T), propType);

                    // 常數型別不合時轉型（例如 int → decimal 或 nullable）
                    // var constFixed = c.Type == propType ? c : Expression.Convert(c, propType);
                    Expression constFixed = c.Type == propType ? (Expression)c : Expression.Convert(c, propType);
                    chain = Expression.Call(
                        miSetConst,
                        chain,
                        Expression.Quote(propAccess),
                        constFixed);
                }
                else
                {
                    // Lambda 版本：SetProperty<TEntity,TProp>(..., Expr<Func<TEntity,TProp>>, Expr<Func<TEntity,TProp>>)
                    var miSetLambda = ResolveSetPropertyMethod(useLambda: true)
                        .MakeGenericMethod(typeof(T), propType);

                    // 確保右邊值型別與欄位一致
                    var rhsFixed = rhsBody.Type == propType ? rhsBody : Expression.Convert(rhsBody, propType);
                    var valueLambda = Expression.Lambda(rhsFixed, eParam);

                    chain = Expression.Call(
                        miSetLambda,
                        chain,
                        Expression.Quote(propAccess),
                        Expression.Quote(valueLambda));
                }
            }

            return Expression.Lambda<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>>(chain, sParam);
        }

        // 從宣告 SetPropertyCalls<> 的組件掃描公開泛型擴充方法 "SetProperty"
        private static MethodInfo ResolveSetPropertyMethod(bool useLambda)
        {
            // 直接取 SetPropertyCalls<> 所在組件（EFCore.Relational）
            var asm = typeof(SetPropertyCalls<>).Assembly;

            var methods = asm
                .GetTypes()
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
                .Where(m => m.Name == "SetProperty" && m.IsGenericMethodDefinition)
                .Where(m =>
                {
                    var ps = m.GetParameters();
                    if (ps.Length != 3) return false;

                    // 參數0：SetPropertyCalls<TEntity>
                    var p0 = ps[0].ParameterType;
                    if (!p0.IsGenericType) return false;
                    if (p0.GetGenericTypeDefinition() != typeof(SetPropertyCalls<>)) return false;

                    // 參數1 必須是 Expression<...>
                    var p1 = ps[1].ParameterType;
                    if (!p1.IsGenericType || p1.GetGenericTypeDefinition() != typeof(Expression<>)) return false;

                    // 參數2：useLambda 決定是常數 TProp 或 Expression<Func<...>>
                    var p2 = ps[2].ParameterType;
                    var p2IsLambda = p2.IsGenericType && p2.GetGenericTypeDefinition() == typeof(Expression<>);

                    return useLambda ? p2IsLambda : !p2IsLambda;
                })
                .ToArray();

            if (methods.Length == 0)
                throw new MissingMethodException("找不到 EF Core 的 SetProperty 擴充方法。請確認已參考 Microsoft.EntityFrameworkCore.Relational。");

            // 有多個候選時，挑第一個符合條件的即可
            return methods[0];
        }

        // 把 x 參數替換成 e
        private sealed class ReplaceParameterVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _from;
            private readonly Expression _to;
            public ReplaceParameterVisitor(ParameterExpression from, Expression to)
            { _from = from; _to = to; }

            protected override Expression VisitParameter(ParameterExpression node)
                => node == _from ? _to : base.VisitParameter(node);
        }
    }
}