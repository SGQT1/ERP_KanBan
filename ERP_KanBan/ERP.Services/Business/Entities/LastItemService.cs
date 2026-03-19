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
    public class LastItemService : BusinessService
    {
        private Services.Entities.LastItemService LastItem { get; }
        public LastItemService(
            Services.Entities.LastItemService lastItemService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            LastItem = lastItemService;
        }
        public IQueryable<Models.Views.LastItem> Get()
        {
            return LastItem.Get().Select(i => new Models.Views.LastItem
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                LastId = i.LastId,
                ShoeSize = i.ShoeSize,
                ShoeSizeSuffix = i.ShoeSizeSuffix,
                ShoeSizeSortKey = i.ShoeSizeSortKey,
                Qty = i.Qty,
                MadeDate = i.MadeDate,
                Cost = i.Cost,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.LastItem> items)
        {
            LastItem.CreateRange(BuildRange(items));
        }
        public void UpdateRange(IEnumerable<Models.Views.LastItem> items)
        {
            LastItem.UpdateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.LastItem, bool>> predicate)
        {
            LastItem.RemoveRange(predicate);
        }
        private IEnumerable<Models.Entities.LastItem> BuildRange(IEnumerable<Models.Views.LastItem> items)
        {
            return items.Select(item => new Models.Entities.LastItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                LastId = item.LastId,
                ShoeSize = item.ShoeSize,
                ShoeSizeSuffix = item.ShoeSizeSuffix,
                ShoeSizeSortKey = item.ShoeSizeSuffix == "J" ? item.ShoeSize : item.ShoeSize * 1000,
                Qty = item.Qty,
                MadeDate = item.MadeDate,
                Cost = item.Cost,
            });
        }


    }
}