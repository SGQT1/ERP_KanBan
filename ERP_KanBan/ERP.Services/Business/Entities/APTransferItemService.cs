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
    public class APTransferItemService : BusinessService
    {
        private Services.Entities.APTransferService APTransfer { get; }
        private Services.Entities.APTransferItemService APTransferItem { get; }

        public APTransferItemService(
            Services.Entities.APTransferService apTransferService,
            Services.Entities.APTransferItemService apTransferItemService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.APTransfer = apTransferService;
            this.APTransferItem = apTransferItemService;
        }
        public IQueryable<Models.Views.APTransferItem> Get()
        {
            var result = (
                from atfi in APTransferItem.Get()
                join atf in APTransfer.Get() on new { APId = atfi.APTransferId, LocaleId = atfi.LocaleId } equals new { APId = atf.Id, LocaleId = atf.LocaleId }
                select new Models.Views.APTransferItem
                {
                    Id = atfi.Id,
                    LocaleId = atfi.LocaleId,
                    APTransferId = atfi.APTransferId,
                    IsTransfer = atfi.IsTransfer,
                    VendorNameTw = atfi.VendorNameTw,
                    APQty = atfi.APQty,
                    UnitPrice = atfi.UnitPrice,
                    SaleAmount = atfi.SaleAmount,
                    Tax = atfi.Tax,
                    TTL = atfi.TTL,
                    IsIntergrate = atfi.IsIntergrate,
                    IsFromInvoice = atfi.IsFromInvoice,
                    PurDollarNameTw = atfi.PurDollarNameTw,
                    ModifyUserName = atf.ModifyUserName,
                    LastUpdateTime = atf.LastUpdateTime,
                    YYYYMM = atf.YYYYMM,
                });
            return result;
        }
        public void CreateRange(IEnumerable<Models.Views.APTransferItem> items)
        {
            APTransferItem.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.APTransferItem, bool>> predicate)
        {
            APTransferItem.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.APTransferItem> BuildRange(IEnumerable<Models.Views.APTransferItem> items)
        {

            return items.Select(item => new ERP.Models.Entities.APTransferItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                APTransferId = item.APTransferId,
                IsTransfer = item.IsTransfer,
                VendorNameTw = item.VendorNameTw,
                APQty = item.APQty,
                UnitPrice = item.UnitPrice,
                SaleAmount = item.SaleAmount,
                Tax = item.Tax,
                TTL = item.TTL,
                IsIntergrate = item.IsIntergrate,
                IsFromInvoice = item.IsFromInvoice,
                PurDollarNameTw = item.PurDollarNameTw,
            });
        }

    }
}