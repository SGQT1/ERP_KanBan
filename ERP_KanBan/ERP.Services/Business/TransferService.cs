using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Models.Views;
using ERP.Services.Business.Entities;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    public class TransferService : BusinessService
    {
        private ERP.Services.Business.Entities.TransferService Transfer { get; set; }
        private ERP.Services.Business.Entities.TransferItemService TransferItem { get; set; }
        private ERP.Services.Business.Entities.ReceivedLogService ReceivedLog { get; set; }
        private ERP.Services.Business.Entities.ReceivedLogSizeItemService ReceivedLogSizeItem { get; set; }
        private ERP.Services.Business.Entities.POItemService POItem { get; set; }
        private ERP.Services.Business.Entities.POItemSizeService POItemSize { get; set; }
        public TransferService(
            ERP.Services.Business.Entities.TransferService transferService,
            ERP.Services.Business.Entities.TransferItemService transferItemService,
            ERP.Services.Business.Entities.ReceivedLogService receivedLogService,
            ERP.Services.Business.Entities.ReceivedLogSizeItemService receivedLogSizeItemService,
            ERP.Services.Business.Entities.POItemService poItemService,
            ERP.Services.Business.Entities.POItemSizeService poItemSizeService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Transfer = transferService;
            TransferItem = transferItemService;
            ReceivedLog = receivedLogService;
            ReceivedLogSizeItem = receivedLogSizeItemService;
            POItem = poItemService;
            POItemSize = poItemSizeService;
        }

        public ERP.Models.Views.TransferGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.TransferGroup { };
            var transfer = Transfer.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (transfer != null)
            {
                group.Transfer = transfer;
                group.TransferItem = TransferItem.Get().Where(i => i.TransferId == id && i.LocaleId == localeId).ToList();
            }
            return group;
        }

        public ERP.Models.Views.TransferGroup Save(TransferGroup item)
        {
            var transfer = item.Transfer;
            var transferItem = item.TransferItem.ToList();

            List<decimal> receivedLogIds = new List<decimal>();
            decimal localeId = 0;

            if (transfer != null)
            {

                try
                {
                    UnitOfWork.BeginTransaction();
                    //Transfer
                    {
                        var _transfer = Transfer.Get().Where(i => i.LocaleId == transfer.LocaleId && i.Id == transfer.Id).FirstOrDefault();

                        if (_transfer != null)
                        {
                            transfer.Id = _transfer.Id;
                            transfer.LocaleId = _transfer.LocaleId;
                            transfer = Transfer.Update(transfer);
                        }
                        else
                        {
                            transfer = Transfer.Create(transfer);
                        }
                    }

                    //Transfer Item
                    {
                        if (transfer.Id != 0)
                        {
                            var uReceivedLogItems = TransferItem.Get().Where(i => i.TransferId == transfer.Id && i.LocaleId == transfer.LocaleId).Select(i => new { i.ReceivedLogId, i.ReceivedLogLocaleId }).Distinct().ToList();

                            //receivedLogIds = uReceivedLogItems.Any() ? uReceivedLogItems.Select(i => i.ReceivedLogId).Distinct().ToList() : transferItem.Select(i => i.ReceivedLogId).Distinct().ToList();
                            // localeId = uReceivedLogItems.Any() ? uReceivedLogItems.Max(i => i.ReceivedLogLocaleId) : transferItem.Max(i => i.ReceivedLogLocaleId);

                            var ids1 = uReceivedLogItems.Where(i => i.ReceivedLogId > 0).Select(i => i.ReceivedLogId).Distinct().ToList();
                            var ids2 = transferItem.Where(i => i.ReceivedLogId > 0).Select(i => i.ReceivedLogId).Distinct().ToList();

                            receivedLogIds = ids1.Union(ids2).Distinct().ToList();
                            // localeId = transferItem.Max(i => i.ReceivedLogLocaleId);
                            localeId = transfer.LocaleId;
                            
                            transferItem.ForEach(i => i.TransferId = transfer.Id);
                            TransferItem.RemoveRange(i => i.TransferId == transfer.Id && i.LocaleId == transfer.LocaleId);
                            TransferItem.CreateRange(transferItem);

                            ReceivedLog.UpdateTransferQty(receivedLogIds, localeId);
                        }
                    }
                    UnitOfWork.Commit();
                }
                catch (Exception e)
                {
                    UnitOfWork.Rollback();
                    throw e;
                }
            }
            return Get((int)transfer.Id, (int)transfer.LocaleId);
        }

        public void Remove(TransferGroup item)
        {
            var transfer = item.Transfer;
            var transferItem = item.TransferItem.ToList();

            List<decimal> receivedLogIds = new List<decimal>();
            decimal localeId = 0;
            
            UnitOfWork.BeginTransaction();
            try
            {
                // var receivedLogIds = item.TransferItem.Select(i => i.ReceivedLogId).Distinct().ToList();
                // var localeId = item.TransferItem.Max(i => i.ReceivedLogLocaleId);

                var uReceivedLogItems = TransferItem.Get().Where(i => i.TransferId == transfer.Id && i.LocaleId == transfer.LocaleId).Select(i => new { i.ReceivedLogId, i.ReceivedLogLocaleId }).Distinct().ToList();

                receivedLogIds = uReceivedLogItems.Any() ? uReceivedLogItems.Select(i => i.ReceivedLogId).Distinct().ToList() : transferItem.Select(i => i.ReceivedLogId).Distinct().ToList();
                localeId = uReceivedLogItems.Any() ? uReceivedLogItems.Max(i => (decimal)i.ReceivedLogLocaleId) : transferItem.Max(i => (decimal)i.ReceivedLogLocaleId);

                TransferItem.RemoveRange(i => i.TransferId == transfer.Id && i.LocaleId == transfer.LocaleId);
                Transfer.Remove(transfer);
                ReceivedLog.RemoveTransferQty(receivedLogIds, localeId);

                UnitOfWork.Commit();

                
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
    }
}
