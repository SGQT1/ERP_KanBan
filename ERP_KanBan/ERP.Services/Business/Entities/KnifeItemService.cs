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
    public class KnifeItemService : BusinessService
    {
        private Services.Entities.KnifeItemService KnifeItem { get; }
        public KnifeItemService(
            Services.Entities.KnifeItemService knifeItemService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            KnifeItem = knifeItemService;
        }
        public IQueryable<Models.Views.KnifeItem> Get()
        {
            return KnifeItem.Get().Select(i => new Models.Views.KnifeItem
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                KnifeId = i.KnifeId,
                ShoeSize = i.ShoeSize,
                ShoeSizeSuffix = i.ShoeSizeSuffix,
                ShoeSizeSortKey = i.ShoeSizeSortKey,
                Qty = i.Qty,
                MadeDate = i.MadeDate,
                Cost = i.Cost,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.KnifeItem> items)
        {
            KnifeItem.CreateRange(BuildRange(items));
        }
        public void UpdateRange(IEnumerable<Models.Views.KnifeItem> items)
        {
            KnifeItem.UpdateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.KnifeItem, bool>> predicate)
        {
            KnifeItem.RemoveRange(predicate);
        }
        private IEnumerable<Models.Entities.KnifeItem> BuildRange(IEnumerable<Models.Views.KnifeItem> items)
        {
            return items.Select(item => new Models.Entities.KnifeItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                KnifeId = item.KnifeId,
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