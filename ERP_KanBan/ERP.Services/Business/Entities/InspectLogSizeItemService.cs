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
    public class InspectLogSizeItemService : BusinessService
    {
        private Services.Entities.ReceivedLogSizeItemService ReceivedLogSizeItem { get; }

        public InspectLogSizeItemService(
            Services.Entities.ReceivedLogSizeItemService receivedLogSizeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            this.ReceivedLogSizeItem = receivedLogSizeItemService;
        }
        public IQueryable<Models.Views.InspectLogSizeItem> Get()
        {
            return ReceivedLogSizeItem.Get().Select(i => new Models.Views.InspectLogSizeItem
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                RefLocaleId = i.RefLocaleId,
                ReceivedLogId = i.ReceivedLogId,
                DisplaySize = i.DisplaySize,
                ReceivedQty = i.ReceivedQty,
                IQCGetQty = i.IQCGetQty,
                StockQty = i.StockQty,
                TransferQty = i.TransferQty,
                SeqNo = i.SeqNo,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                ShoeInnerSize = i.ShoeInnerSize,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.InspectLogSizeItem> items)
        {
            ReceivedLogSizeItem.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.ReceivedLogSizeItem, bool>> predicate)
        {
            ReceivedLogSizeItem.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.ReceivedLogSizeItem> BuildRange(IEnumerable<Models.Views.InspectLogSizeItem> items)
        {
            return items.Select(item => new ERP.Models.Entities.ReceivedLogSizeItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                RefLocaleId = item.RefLocaleId,
                ReceivedLogId = item.ReceivedLogId,
                DisplaySize = item.DisplaySize,
                ReceivedQty = item.ReceivedQty,
                IQCGetQty = item.IQCGetQty,
                StockQty = item.StockQty,
                TransferQty = item.TransferQty,
                SeqNo = item.SeqNo,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                ShoeInnerSize = item.ShoeInnerSize,
            });
        }
    }
}