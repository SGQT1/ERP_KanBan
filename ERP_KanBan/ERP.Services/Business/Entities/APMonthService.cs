using System;
using System.Linq;
using System.Linq.Expressions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class APMonthService : BusinessService
    {
        private Services.Entities.APMonthService APMonth { get; }
        // private Services.Entities.CompanyService Company { get; }

        public APMonthService(
            Services.Entities.APMonthService apMonthService,
            // Services.Entities.CompanyService companyService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            APMonth = apMonthService;
            // Company = companyService;
        }
        public IQueryable<Models.Views.APMonth> Get()
        {
            return APMonth.Get().Select(i => new Models.Views.APMonth
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
                CloseUserName = i.CloseUserName,
                CloseTime = i.CloseTime,
                APGetPre = i.APGetPre,
                PaymentLocaleId = i.PaymentLocaleId,
                // PaymentLocale = Company.Get().Where(c => c.Id == i.LocaleId).Max(c => c.CompanyNo)
                APType = 1,
            });
        }
        public Models.Views.APMonth Create(Models.Views.APMonth item)
        {
            var _item = APMonth.Create(Build(item));

            // Update APNo
            _item.APNo = _item.YYYYMM+"-"+_item.Id;
            APMonth.Update(_item);

            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.APMonth Update(Models.Views.APMonth item)
        {
            var _item = APMonth.Update(Build(item));

            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.APMonth item)
        {
            APMonth.Remove(Build(item));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.APMonth, bool>> predicate)
        {
            APMonth.RemoveRange(predicate);
        }
        //for update, transfer view model to entity
        private Models.Entities.APMonth Build(Models.Views.APMonth item)
        {
            return new Models.Entities.APMonth()
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
                CloseUserName = item.CloseUserName,
                CloseTime = item.CloseTime,
                APGetPre = item.APGetPre,
                PaymentLocaleId = item.PaymentLocaleId
            };
        }
    }
}