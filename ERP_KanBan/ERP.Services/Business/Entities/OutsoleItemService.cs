using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class OutsoleItemService : BusinessService
    {
        private Services.Entities.OutsoleItemService OutsoleItem { get; }
        public OutsoleItemService(
            Services.Entities.OutsoleItemService outsoleItemService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            OutsoleItem = outsoleItemService;
        }
        public IQueryable<Models.Views.OutsoleItem> Get()
        {
            return OutsoleItem.Get().Select(i => new Models.Views.OutsoleItem
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                OutsoleId = i.OutsoleId,
                ShoeSize = i.ShoeSize,
                ShoeSizeSuffix = i.ShoeSizeSuffix,
                ShoeSizeSortKey = i.ShoeSizeSortKey,
                Qty = i.Qty,
                MadeDate = i.MadeDate,
                Cost = i.Cost,
                Map2MDSize = i.Map2MDSize,
                Map2EVASize = i.Map2EVASize,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.OutsoleItem> items)
        {
            OutsoleItem.CreateRange(BuildRange(items));
        }
        public void UpdateRange(IEnumerable<Models.Views.OutsoleItem> items)
        {
            OutsoleItem.UpdateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.OutsoleItem, bool>> predicate)
        {
            OutsoleItem.RemoveRange(predicate);
        }
        private IEnumerable<Models.Entities.OutsoleItem> BuildRange(IEnumerable<Models.Views.OutsoleItem> items)
        {
            return items.Select(item => new Models.Entities.OutsoleItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                OutsoleId = item.OutsoleId,
                ShoeSize = item.ShoeSize,
                ShoeSizeSuffix = item.ShoeSizeSuffix,
                ShoeSizeSortKey = item.ShoeSizeSuffix == "J" ? item.ShoeSize : item.ShoeSize * 1000,
                Qty = item.Qty,
                MadeDate = item.MadeDate,
                Cost = item.Cost,
                Map2MDSize = item.Map2MDSize,
                Map2EVASize = item.Map2EVASize,
            });
        }


    }
}