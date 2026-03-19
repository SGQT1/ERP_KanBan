using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class APMonthOtherService : BusinessService
    {
        private Services.Entities.APMonthOtherService APMonthOther { get; }
        private Services.Entities.CompanyService Company { get; }

        public APMonthOtherService(
            Services.Entities.APMonthOtherService apMonthOtherService,
            Services.Entities.CompanyService companyService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.APMonthOther = apMonthOtherService;
        }
        public IQueryable<Models.Views.APMonthOther> Get()
        {
            return APMonthOther.Get().Select(i => new Models.Views.APMonthOther
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                APNo = i.APNo,
                YYYYMM = i.YYYYMM,
                VendorNameTw = i.VendorNameTw,
                AP = i.AP,
                Tax = i.Tax,
                IsClose = i.IsClose,
                PreAPTTL = i.PreAPTTL,
                APTTL = i.APTTL,
                APGet = i.APGet,
                DollarCodeName = i.DollarCodeName,
                BankingRate = i.BankingRate,
                Remark = i.Remark,
                ISONo = i.ISONo,
                ReceiveAddress = i.ReceiveAddress,
                TelNo = i.TelNo,
                PaymentCodeName = i.PaymentCodeName,
                Discount = i.Discount,
                TaxRate = i.TaxRate,
                IsTaxCombined = i.IsTaxCombined,
                APGetPre = i.APGetPre,
                PaymentLocaleId = i.PaymentLocaleId,
                // PaymentLocale = Company.Get().Where(c => c.Id == i.LocaleId).Max(c => c.CompanyNo)
            });
        }
        public Models.Views.APMonthOther Create(Models.Views.APMonthOther item)
        {
            var _item = APMonthOther.Create(Build(item));

            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.APMonthOther Update(Models.Views.APMonthOther item)
        {
            var _item = APMonthOther.Update(Build(item));

            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.APMonthOther item)
        {
            APMonthOther.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.APMonthOther Build(Models.Views.APMonthOther item)
        {
            return new Models.Entities.APMonthOther()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                APNo = item.APNo,
                YYYYMM = item.YYYYMM,
                VendorNameTw = item.VendorNameTw,
                AP = item.AP,
                Tax = item.Tax,
                IsClose = item.IsClose,
                PreAPTTL = item.PreAPTTL,
                APTTL = item.APTTL,
                APGet = item.APGet,
                DollarCodeName = item.DollarCodeName,
                BankingRate = item.BankingRate,
                Remark = item.Remark,
                ISONo = item.ISONo,
                ReceiveAddress = item.ReceiveAddress,
                TelNo = item.TelNo,
                PaymentCodeName = item.PaymentCodeName,
                Discount = item.Discount,
                TaxRate = item.TaxRate,
                IsTaxCombined = item.IsTaxCombined,
                APGetPre = item.APGetPre,
            };
        }
    }
}