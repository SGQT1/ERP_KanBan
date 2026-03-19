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
    public class TransferSizeItemService : BusinessService
    {
        private Services.Entities.TransferSizeItemService TransferSizeItem { get; }

        public TransferSizeItemService(
            Services.Entities.TransferSizeItemService transferSizeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            this.TransferSizeItem = transferSizeItemService;
        }
        public IQueryable<Models.Views.TransferSizeItem> Get()
        {
            return TransferSizeItem.Get().Select(i => new Models.Views.TransferSizeItem
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                TransferItemId = i.TransferItemId,
                DisplaySize = i.DisplaySize,
                TransferQty = i.TransferQty,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.TransferSizeItem> items)
        {
            TransferSizeItem.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.TransferSizeItem, bool>> predicate)
        {
            TransferSizeItem.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.TransferSizeItem> BuildRange(IEnumerable<Models.Views.TransferSizeItem> items)
        {
            return items.Select(item => new ERP.Models.Entities.TransferSizeItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                TransferItemId = item.TransferItemId,
                DisplaySize = item.DisplaySize,
                TransferQty = item.TransferQty,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }
    }
}