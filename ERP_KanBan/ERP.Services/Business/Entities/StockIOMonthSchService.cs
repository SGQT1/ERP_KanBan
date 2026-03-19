using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class StockIOMonthSchService : BusinessService
    {
        private Services.Entities.StockIOMonthSchService StockIOMonthSch { get; }

        public StockIOMonthSchService(
            Services.Entities.StockIOMonthSchService stockIOMonthSchService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.StockIOMonthSch = stockIOMonthSchService;
        }
        public IQueryable<Models.Views.MonthMaterialStockItemSch> Get()
        {
            var result = (
                from s in StockIOMonthSch.Get()
                select new Models.Views.MonthMaterialStockItemSch
                {
                    Id = s.Id,
                    LocaleId = s.LocaleId,
                    IOMonth = s.IOMonth,
                    CalTime = s.CalTime,
                    ModifyUserName = s.ModifyUserName,
                    LastUpdateTime = s.LastUpdateTime,
                });
            return result;
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
        //for update, transfer view model to entity
        private Models.Entities.StockIOMonthSch Build(Models.Views.MonthMaterialStockItemSch item)
        {
            return new Models.Entities.StockIOMonthSch()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                IOMonth = item.IOMonth,
                CalTime = item.CalTime,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            };
        }

    }
}