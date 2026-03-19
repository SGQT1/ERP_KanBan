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
    public class APMonthItemDiscountService : BusinessService
    {
        private Services.Entities.APMonthItemDiscountService APMonthItemDiscount { get; }

        public APMonthItemDiscountService(
            Services.Entities.APMonthItemDiscountService apMonthItemDiscountService, 
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.APMonthItemDiscount = apMonthItemDiscountService;
        }
        public IQueryable<Models.Views.APMonthItemDiscount> Get()
        {
            return APMonthItemDiscount.Get().Select(i => new Models.Views.APMonthItemDiscount
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                APMonthId = i.APMonthId,
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
                // PaymentLocaleId = i.PaymentLocaleId,
                POItemId = i.POItemId,
                ReceivedLocaleId = i.ReceivedLocaleId
            });
        }
        public void CreateRange(IEnumerable<Models.Views.APMonthItemDiscount> items)
        {
            APMonthItemDiscount.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.APMonthItemDiscount, bool>> predicate)
        {
            APMonthItemDiscount.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.APMonthItemDiscount> BuildRange(IEnumerable<Models.Views.APMonthItemDiscount> items)
        {
            return items.Select(item => new ERP.Models.Entities.APMonthItemDiscount
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                APMonthId = item.APMonthId,
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
                PurLocaleId = item.PurLocaleId == null ? Convert.ToInt32(item.WarehouseNo) : item.PurLocaleId,
                PurUserName = item.PurUserName,
                // PaymentLocaleId = item.PaymentLocaleId,
                POItemId = item.POItemId,
                ReceivedLocaleId = item.ReceivedLocaleId
            });
        }

    }
}