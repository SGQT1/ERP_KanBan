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
    public class APMonthOtherItemDiscountService : BusinessService
    {
        private Services.Entities.APMonthOtherItemDiscountService APMonthOtherItemDiscount { get; }

        public APMonthOtherItemDiscountService(
            Services.Entities.APMonthOtherItemDiscountService apMonthOtherItemDiscountService, 
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.APMonthOtherItemDiscount = apMonthOtherItemDiscountService;
        }
        public IQueryable<Models.Views.APMonthOtherItemDiscount> Get()
        {
            return APMonthOtherItemDiscount.Get().Select(i => new Models.Views.APMonthOtherItemDiscount
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                APMonthOtherId = i.APMonthOtherId,
                IsGet = i.IsGet,
                ReceiveDate = i.ReceiveDate,
                ReceiveRefNo = i.ReceiveRefNo,
                PayQty = i.PayQty,
                DollarCodeName = i.DollarCodeName,
                BankingRate = i.BankingRate,
                APAmount = i.APAmount,
                TaxRate = i.TaxRate,
                APTax = i.APTax,
                UnitPrice = i.UnitPrice,
                PreAPGet = i.PreAPGet,
                APTTL = i.APTTL,
                APGet = i.APGet,
                UniInvoiceNo = i.UniInvoiceNo,
                Remark = i.Remark,
                APYM = i.APYM,
                Discount = i.Discount,
                PUnit = i.PUnit,
                TypeNo = i.TypeNo,
                VendorMaterialNo = i.VendorMaterialNo,
                Spec = i.Spec,
                WONo = i.WONo,
                WarehouseNo = i.WarehouseNo,
                SeqNo = i.SeqNo,
                IsDraft = i.IsDraft,
                VoucherNo = i.VoucherNo,
                PurLocaleId = i.PurLocaleId,
                PurUserName = i.PurUserName,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.APMonthOtherItemDiscount> items)
        {
            APMonthOtherItemDiscount.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.APMonthOtherItemDiscount, bool>> predicate)
        {
            APMonthOtherItemDiscount.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.APMonthOtherItemDiscount> BuildRange(IEnumerable<Models.Views.APMonthOtherItemDiscount> items)
        {
            return items.Select(item => new ERP.Models.Entities.APMonthOtherItemDiscount
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                APMonthOtherId = item.APMonthOtherId,
                IsGet = item.IsGet,
                ReceiveDate = item.ReceiveDate,
                ReceiveRefNo = item.ReceiveRefNo,
                PayQty = item.PayQty,
                DollarCodeName = item.DollarCodeName,
                BankingRate = item.BankingRate,
                APAmount = item.APAmount,
                TaxRate = item.TaxRate,
                APTax = item.APTax,
                UnitPrice = item.UnitPrice,
                PreAPGet = item.PreAPGet,
                APTTL = item.APTTL,
                APGet = item.APGet,
                UniInvoiceNo = item.UniInvoiceNo,
                Remark = item.Remark,
                APYM = item.APYM,
                Discount = item.Discount,
                PUnit = item.PUnit,
                TypeNo = item.TypeNo,
                VendorMaterialNo = item.VendorMaterialNo,
                Spec = item.Spec,
                WONo = item.WONo,
                WarehouseNo = item.WarehouseNo,
                SeqNo = item.SeqNo,
                IsDraft = item.IsDraft,
                VoucherNo = item.VoucherNo,
                PurLocaleId = item.PurLocaleId,
                PurUserName = item.PurUserName,
            });
        }

    }
}