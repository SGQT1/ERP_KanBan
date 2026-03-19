using System;
using System.Linq;
using System.Linq.Expressions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class MonthMaterialStockItemSchService : BusinessService
    {
        private ERP.Services.Entities.StockIOMonthSchService StockIOMonthSch { get; }

        public MonthMaterialStockItemSchService(
            ERP.Services.Entities.StockIOMonthSchService stockIOMonthSchService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.StockIOMonthSch = stockIOMonthSchService;
        }
        public IQueryable<Models.Views.MonthMaterialStockItemSch> Get()
        {
            return StockIOMonthSch.Get().Select(i => new Models.Views.MonthMaterialStockItemSch
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                IOMonth = i.IOMonth,
                CalTime = i.CalTime,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
            });
        }
        public Models.Views.MonthMaterialStockItemSch Create(Models.Views.MonthMaterialStockItemSch item)
        {
            var _item = StockIOMonthSch.Create(Build(item));

            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MonthMaterialStockItemSch Update(Models.Views.MonthMaterialStockItemSch item)
        {
            var _item = StockIOMonthSch.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MonthMaterialStockItemSch item)
        {
            StockIOMonthSch.Remove(Build(item));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.StockIOMonthSch, bool>> predicate)
        {
            StockIOMonthSch.RemoveRange(predicate);
        }
        private Models.Entities.StockIOMonthSch Build(Models.Views.MonthMaterialStockItemSch item)
        {
            return new Models.Entities.StockIOMonthSch
            { 
                Id = item.Id,
                LocaleId = item.LocaleId,
                IOMonth = item.IOMonth,
                CalTime = DateTime.Now,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            };
        }
    }
}