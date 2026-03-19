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
    public class APMonthItemService : BusinessService
    {
        private Services.Entities.APMonthItemService APMonthItem { get; }
        private Services.Entities.APMonthService APMonth { get; }

        public APMonthItemService(
            Services.Entities.APMonthItemService apMonthItemService,
            Services.Entities.APMonthService apMonthService,
            Services.Entities.CompanyService companyService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            APMonthItem = apMonthItemService;
            APMonth = apMonthService;
        }
        public IQueryable<Models.Views.APMonthItem> Get()
        {
            var result = (
                from api in APMonthItem.Get()
                join ap in APMonth.Get() on new { APMonthId = api.APMonthId, LocaleId = api.LocaleId } equals new { APMonthId = ap.Id, LocaleId = ap.LocaleId }
                select new Models.Views.APMonthItem
                {
                    Id = api.Id,
                    LocaleId = api.LocaleId,
                    APMonthId = api.APMonthId,
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
        public void CreateRange(IEnumerable<Models.Views.APMonthItem> items)
        {
            APMonthItem.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.APMonthItem, bool>> predicate)
        {
            APMonthItem.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.APMonthItem> BuildRange(IEnumerable<Models.Views.APMonthItem> items)
        {
            return items.Select(item => new ERP.Models.Entities.APMonthItem
            {
                Id = item.Id ?? 0,
                LocaleId = item.LocaleId ?? 0,
                APMonthId = item.APMonthId ?? 0,
                IsGet = item.IsGet ?? 1,
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
                // WarehouseNo = item.WarehouseNo,
                WarehouseNo = item.PurLocaleId.ToString(),
                SeqNo = item.SeqNo,
                IsDraft = item.IsDraft,
                VoucherNo = item.VoucherNo,
                PurLocaleId = item.PurLocaleId,
                PurUserName = item.PurUserName,
                // PaymentLocaleId = item.PaymentLocaleId,
                POItemId = item.POItemId,
                ReceivedLocaleId = item.ReceivedLocaleId,
                ReceivedLogId = item.ReceivedLogId,
            });
        }

    }
}