using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business.Entities
{

    public class MPSReceivedLogService : BusinessService
    {
        private ERP.Services.Entities.MpsReceivedLogService MPSReceivedLog { get; set; }

        public MPSReceivedLogService(
            ERP.Services.Entities.MpsReceivedLogService mpsReceivedLogService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSReceivedLog = mpsReceivedLogService;
        }

        public IQueryable<Models.Views.MPSReceivedLog> Get()
        {
            return MPSReceivedLog.Get().Select(i => new Models.Views.MPSReceivedLog
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                MpsProcedurePOId = i.MpsProcedurePOId,
                RefLocaleId = i.RefLocaleId,
                ReceivedDate = i.ReceivedDate,
                ReceivedQty = i.ReceivedQty,
                ChargeQty = i.ChargeQty,
                FreeQty = i.FreeQty,
                QCBackQty = i.QCBackQty,
                doPay = i.doPay,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                Remark = i.Remark,
                PayMonth = i.PayMonth,
                AddFreeQty = i.AddFreeQty,
                DiscountRate = i.DiscountRate,
                WarehouseNo = i.WarehouseNo,
            });
        }
        public Models.Views.MPSReceivedLog Create(Models.Views.MPSReceivedLog item)
        {
            var _item = MPSReceivedLog.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MPSReceivedLog Update(Models.Views.MPSReceivedLog item)
        {
            var _item = MPSReceivedLog.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MPSReceivedLog item)
        {
            MPSReceivedLog.Remove(Build(item));
        }
        private Models.Entities.MpsReceivedLog Build(Models.Views.MPSReceivedLog item)
        {
            var payMonth = item.ReceivedDate.ToString("yyyyMM");
            var payDate = item.DayOfMonth == 0 ? new DateTime(item.ReceivedDate.AddMonths(1).Year, item.ReceivedDate.AddMonths(1).Month, 1).AddDays(-1) :
                                                        new DateTime(item.ReceivedDate.Year, item.ReceivedDate.Month, (int)item.DayOfMonth);
            payMonth = item.ReceivedDate.Date <= payDate.Date ? payMonth : new DateTime(item.ReceivedDate.AddMonths(1).Year, item.ReceivedDate.AddMonths(1).Month, 1).ToString("yyyyMM");
            return new Models.Entities.MpsReceivedLog
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MpsProcedurePOId = item.MpsProcedurePOId,
                RefLocaleId = item.RefLocaleId,
                ReceivedDate = item.ReceivedDate,
                ReceivedQty = item.ReceivedQty,
                ChargeQty = item.ChargeQty,
                FreeQty = item.FreeQty,
                QCBackQty = item.QCBackQty,
                doPay = item.doPay,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                Remark = item.Remark,
                PayMonth = payMonth, //item.PayMonth,
                AddFreeQty = item.AddFreeQty,
                DiscountRate = item.DiscountRate,
                WarehouseNo = item.WarehouseNo,
            };
        }

        public void UpdateDoPay(int localeId, List<decimal> doPayIds, List<decimal> noPayIds)
        {
            // update MPSReceivedLog doPay : doPay = 1
            MPSReceivedLog.UpdateRange(
                i => doPayIds.Contains(i.Id) && i.LocaleId == localeId,
                // u => new Models.Entities.MpsReceivedLog { doPay = 1 }
                u => u.SetProperty(p => p.doPay, v => 1)
            );

            // update MPSReceivedLog noPay : noPay = 1
            MPSReceivedLog.UpdateRange(
                i => noPayIds.Contains(i.Id) && i.LocaleId == localeId,
                // u => new Models.Entities.MpsReceivedLog { doPay = 0 }
                u => u.SetProperty(p => p.doPay, v => 0)
            );
        }
    }
}
