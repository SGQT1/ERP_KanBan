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
    public class APMonthOtherItemService : BusinessService
    {
        private Services.Entities.APMonthOtherItemService APMonthOtherItem { get; }
        private Services.Entities.APMonthOtherService APMonthOther { get; }
        public APMonthOtherItemService(
            Services.Entities.APMonthOtherItemService apMonthOtherItemService,
            Services.Entities.APMonthOtherService apMonthOtherService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            APMonthOtherItem = apMonthOtherItemService;
            APMonthOther = apMonthOtherService;
        }
        public IQueryable<Models.Views.APMonthOtherItem> Get()
        {
            var result = (
                from api in APMonthOtherItem.Get()
                join ap in APMonthOther.Get() on new { APMonthId = api.APMonthOtherId, LocaleId = api.LocaleId } equals new { APMonthId = ap.Id, LocaleId = ap.LocaleId }
                select new Models.Views.APMonthOtherItem
                {
                    Id = api.Id,
                    LocaleId = api.LocaleId,
                    APMonthOtherId = api.APMonthOtherId,
                    IsGet = api.IsGet,
                    ReceivedDate = api.ReceiveDate,
                    ReceivedRefNo = api.ReceiveRefNo,
                    PayQty = api.PayQty,
                    DollarCodeName = api.DollarCodeName,
                    BankingRate = api.BankingRate,
                    APAmount = api.APAmount,
                    TaxRate = api.TaxRate,
                    APTax = api.APTax,
                    UnitPrice = api.UnitPrice,
                    PreAPGet = api.PreAPGet,
                    APTTL = api.APTTL,
                    APGet = api.APGet,

                    UniInvoiceNo = api.UniInvoiceNo,
                    Remark = api.Remark,
                    APYM = api.APYM,
                    Discount = api.Discount,
                    PUnit = api.PUnit,
                    TypeNo = api.TypeNo,
                    VendorMaterialNo = api.VendorMaterialNo,
                    Spec = api.Spec,
                    WONo = api.WONo,
                    WarehouseNo = api.WarehouseNo,
                    SeqNo = api.SeqNo,
                    IsDraft = api.IsDraft,
                    VoucherNo = api.VoucherNo,
                    PurLocaleId = api.PurLocaleId,
                    PurUserName = api.PurUserName,
                    PaymentLocaleId = ap.PaymentLocaleId,
                    POItemId = api.POItemId,
                    ReceivedLocaleId = api.ReceivedLocaleId,
                    ReceivedLogId = api.ReceivedLogId,
                }
            );
            return result;
        }
        // public IQueryable<Models.Views.APMonthOtherItem> Get()
        // {
        //     return APMonthOtherItem.Get().Select(i => new Models.Views.APMonthOtherItem
        //     {
        //         Id = i.Id,
        //         LocaleId = i.LocaleId,
        //         APMonthOtherId = i.APMonthOtherId,
        //         IsGet = i.IsGet,
        //         ReceiveDate = i.ReceiveDate,
        //         ReceiveRefNo = i.ReceiveRefNo,
        //         PayQty = i.PayQty,
        //         DollarCodeName = i.DollarCodeName,
        //         BankingRate = i.BankingRate,
        //         APAmount = i.APAmount,
        //         TaxRate = i.TaxRate,
        //         APTax = i.APTax,
        //         UnitPrice = i.UnitPrice,
        //         PreAPGet = i.PreAPGet,
        //         APTTL = i.APTTL,
        //         APGet = i.APGet,
        //         UniInvoiceNo = i.UniInvoiceNo,
        //         Remark = i.Remark,
        //         APYM = i.APYM,
        //         Discount = i.Discount,
        //         PUnit = i.PUnit,
        //         TypeNo = i.TypeNo,
        //         VendorMaterialNo = i.VendorMaterialNo,
        //         Spec = i.Spec,
        //         WONo = i.WONo,
        //         WarehouseNo = i.WarehouseNo,
        //         SeqNo = i.SeqNo,
        //         IsDraft = i.IsDraft,
        //         VoucherNo = i.VoucherNo,
        //         PurLocaleId = i.PurLocaleId,
        //         PurUserName = i.PurUserName,

        //         // PaymentLocaleId = i.PaymentLocaleId,
        //         POItemId = i.POItemId,
        //         ReceivedLocaleId = i.ReceivedLocaleId,
        //         ReceivedLogId = i.ReceivedLogId,
        //     });
        // }
        public void CreateRange(IEnumerable<Models.Views.APMonthOtherItem> items)
        {
            APMonthOtherItem.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.APMonthOtherItem, bool>> predicate)
        {
            APMonthOtherItem.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.APMonthOtherItem> BuildRange(IEnumerable<Models.Views.APMonthOtherItem> items)
        {
            return items.Select(item => new ERP.Models.Entities.APMonthOtherItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                APMonthOtherId = item.APMonthOtherId,
                IsGet = item.IsGet,
                ReceiveDate = item.ReceivedDate,
                ReceiveRefNo = item.ReceivedRefNo,
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