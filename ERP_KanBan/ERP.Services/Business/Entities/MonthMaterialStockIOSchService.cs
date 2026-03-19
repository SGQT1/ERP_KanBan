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
    public class MonthMaterialStockIOSchService : BusinessService
    {
        private ERP.Services.Entities.MonthMaterialStockIOSchService MonthMaterialStockIOSch { get; }

        public MonthMaterialStockIOSchService(
            ERP.Services.Entities.MonthMaterialStockIOSchService MonthMaterialStockIOSch,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.MonthMaterialStockIOSch = MonthMaterialStockIOSch;
        }
        public IQueryable<Models.Views.MonthMaterialStockIOSch> Get()
        {
            return MonthMaterialStockIOSch.Get().Select(i => new Models.Views.MonthMaterialStockIOSch
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                IOMonth = i.IOMonth,
                CalTime = i.CalTime,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                Records = i.Records,
            });
        }
        public Models.Views.MonthMaterialStockIOSch Create(Models.Views.MonthMaterialStockIOSch item)
        {
            var _item = MonthMaterialStockIOSch.Create(Build(item));

            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MonthMaterialStockIOSch Update(Models.Views.MonthMaterialStockIOSch item)
        {
            var _item = MonthMaterialStockIOSch.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MonthMaterialStockIOSch item)
        {
            MonthMaterialStockIOSch.Remove(Build(item));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MonthMaterialStockIOSch, bool>> predicate)
        {
            MonthMaterialStockIOSch.RemoveRange(predicate);
        }
        private Models.Entities.MonthMaterialStockIOSch Build(Models.Views.MonthMaterialStockIOSch item)
        {
            return new Models.Entities.MonthMaterialStockIOSch
            { 
                Id = item.Id,
                LocaleId = item.LocaleId,
                IOMonth = item.IOMonth,
                CalTime = DateTime.Now,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                Records = item.Records,
            };
        }
    }
}