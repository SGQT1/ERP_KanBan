using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class POItemSizeService : BusinessService
    {
        private ERP.Services.Entities.POItemSizeService POItemSize { get; set; }

        public POItemSizeService(
            ERP.Services.Entities.POItemSizeService poItemSize,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            POItemSize = poItemSize;
        }
        public IQueryable<Models.Views.POItemSize> Get()
        {
            return POItemSize.Get().Select(i => new Models.Views.POItemSize
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                POItemId = i.POItemId,
                DisplaySize = i.DisplaySize,
                Qty = i.Qty,
                SeqNo = i.SeqNo,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                PreQty = i.PreQty,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.POItemSize> items)
        {
            POItemSize.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.POItemSize, bool>> predicate)
        {
            POItemSize.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.POItemSize> BuildRange(IEnumerable<Models.Views.POItemSize> items)
        {
            return items.Select(item => new ERP.Models.Entities.POItemSize
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                POItemId = item.POItemId,
                DisplaySize = item.DisplaySize,
                Qty = item.Qty,
                SeqNo = item.SeqNo,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                PreQty = item.PreQty,
            });
        }


    }
}