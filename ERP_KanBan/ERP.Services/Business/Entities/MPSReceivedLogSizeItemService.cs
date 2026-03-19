using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class MPSReceivedLogSizeItemService : BusinessService
    {
        private Services.Entities.MpsReceivedLogSizeItemService MPSReceivedLogSizeItem { get; }

        public MPSReceivedLogSizeItemService(
            Services.Entities.MpsReceivedLogSizeItemService mpsReceivedLogSizeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSReceivedLogSizeItem = mpsReceivedLogSizeItemService;
        }
        public IQueryable<Models.Views.MPSReceivedLogSizeItem> Get()
        {
            return MPSReceivedLogSizeItem.Get().Select(i => new Models.Views.MPSReceivedLogSizeItem
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                MpsReceivedLogId = i.MpsReceivedLogId,
                DisplaySize = i.DisplaySize,
                QCQty = i.QCQty,
                FreeQty = i.FreeQty,
                SubQty = i.QCQty + i.FreeQty,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.MPSReceivedLogSizeItem> items)
        {
            MPSReceivedLogSizeItem.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsReceivedLogSizeItem, bool>> predicate)
        {
            MPSReceivedLogSizeItem.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MpsReceivedLogSizeItem> BuildRange(IEnumerable<Models.Views.MPSReceivedLogSizeItem> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsReceivedLogSizeItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MpsReceivedLogId = item.MpsReceivedLogId,
                DisplaySize = item.DisplaySize,
                QCQty = item.QCQty,
                FreeQty = item.FreeQty,
            });
        }
    }
}