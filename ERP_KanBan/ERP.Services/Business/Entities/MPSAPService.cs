using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business.Entities
{

    public class MPSAPService : BusinessService
    {
        private ERP.Services.Entities.MpsAPService MPSAP { get; set; }

        public MPSAPService(
            ERP.Services.Entities.MpsAPService mpsAPService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSAP = mpsAPService;
        }

        public IQueryable<Models.Views.MPSAP> Get()
        {
            return MPSAP.Get().Select(i => new Models.Views.MPSAP
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                MpsReceivedLogId = i.MpsReceivedLogId,
                MpsProcedurePOId = i.MpsProcedurePOId,
                PayMonth = i.PayMonth,
                Vendor = i.Vendor,
                PaymentLocaleId = i.PaymentLocaleId,
                ChargeQty = i.ChargeQty,
                PurUnitName = i.PurUnitName,
                UnitPrice = i.UnitPrice,
                DollarNameTw = i.DollarNameTw,
                AdjustAmount = i.AdjustAmount,
                SubAmount = i.SubAmount,
                Remark = i.Remark,
                OrderNo = i.OrderNo,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
            });
        }

        public void CreateRange(IEnumerable<Models.Views.MPSAP> items)
        {
            MPSAP.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsAP, bool>> predicate)
        {
            MPSAP.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MpsAP> BuildRange(IEnumerable<Models.Views.MPSAP> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsAP
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MpsReceivedLogId = item.MpsReceivedLogId,
                MpsProcedurePOId = item.MpsProcedurePOId,
                PayMonth = item.PayMonth,
                Vendor = item.Vendor,
                PaymentLocaleId = item.PaymentLocaleId,
                ChargeQty = item.ChargeQty,
                PurUnitName = item.PurUnitName,
                UnitPrice = item.UnitPrice,
                DollarNameTw = item.DollarNameTw,
                AdjustAmount = item.AdjustAmount,
                SubAmount = item.SubAmount,
                Remark = item.Remark,
                OrderNo = item.OrderNo,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }

        
        public Models.Views.MPSAP Create(Models.Views.MPSAP item)
        {
            var _item = MPSAP.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MPSAP Update(Models.Views.MPSAP item)
        {
            var _item = MPSAP.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MPSAP item)
        {
            MPSAP.Remove(Build(item));
        }
        private Models.Entities.MpsAP Build(Models.Views.MPSAP item)
        {
            return new Models.Entities.MpsAP()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MpsReceivedLogId = item.MpsReceivedLogId,
                MpsProcedurePOId = item.MpsProcedurePOId,
                PayMonth = item.PayMonth,
                Vendor = item.Vendor,
                PaymentLocaleId = item.PaymentLocaleId,
                ChargeQty = item.ChargeQty,
                PurUnitName = item.PurUnitName,
                UnitPrice = item.UnitPrice,
                DollarNameTw = item.DollarNameTw,
                AdjustAmount = item.AdjustAmount,
                SubAmount = item.SubAmount,
                Remark = item.Remark,
                OrderNo = item.OrderNo,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            };
        }
    }
}
