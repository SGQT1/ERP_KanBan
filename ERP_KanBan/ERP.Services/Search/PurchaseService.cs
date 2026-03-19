using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Search
{
    public class PurchaseService : SearchService
    {
        private ERP.Services.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Entities.POService PO { get; set; }
        private ERP.Services.Entities.POItemService POItem { get; set; }
        private ERP.Services.Entities.ReceivedLogService ReceivedLog { get; set; }
        private ERP.Services.Entities.ReceivedLogAddService ReceivedLogAdd { get; set; }
        private ERP.Services.Entities.VendorService Vendor { get; set; }
        private ERP.Services.Entities.CodeItemService CodeItem { get; set; }
        private ERP.Services.Entities.CompanyService Company { get; set; }
        private ERP.Services.Business.Entities.TypeService Type { get; set; }

        public PurchaseService(
            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Entities.POService poService,
            ERP.Services.Entities.POItemService poItemService,
            ERP.Services.Entities.ReceivedLogService receivedLogService,
            ERP.Services.Entities.ReceivedLogAddService receivedLogAddService,
            ERP.Services.Entities.VendorService vendorService,
            ERP.Services.Entities.CodeItemService codeItemService,
            ERP.Services.Entities.CompanyService companyService,
            ERP.Services.Business.Entities.TypeService typeService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Orders = ordersService;
            PO = poService;
            POItem = poItemService;
            ReceivedLog = receivedLogService;
            ReceivedLogAdd = receivedLogAddService;
            Vendor = vendorService;
            CodeItem = codeItemService;
            Company = companyService;
            Type = typeService;
        }

        // public IQueryable<Models.Views.PayableForVendor> GetMonthAPSummary(string predicate)
        // {
        //     var company = Company.Get().ToList();
        //     var item = (
        //         from rl in ReceivedLog.Get()
        //         join rla in ReceivedLogAdd.Get() on new { ReceivedLogId = rl.Id, LocaleId = rl.LocaleId } equals new { ReceivedLogId = rla.ReceivedLogId, LocaleId = rla.LocaleId } into rlaGRP
        //         from rla in rlaGRP.DefaultIfEmpty()
        //         join pi in POItem.Get() on new { POItemId = rl.POItemId, LocaleId = rl.RefLocaleId } equals new { POItemId = pi.Id, LocaleId = pi.LocaleId }
        //         join p in PO.Get() on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = p.Id, LocaleId = p.LocaleId }
        //         join o in Orders.Get() on new { OrderNo = pi.OrderNo } equals new { OrderNo = o.OrderNo } into oGRP
        //         from o in oGRP.DefaultIfEmpty()
        //         join v in Vendor.Get() on new { VendorId = rl.ShippingListVendorId, LocaleId = rl.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId }
        //         select new
        //         {
        //             Vendor = v.NameTw,
        //             VendorId = rl.ShippingListVendorId,
        //             CompanyId = pi.CompanyId != null ? pi.CompanyId : o.CompanyId,
        //             SubAmount = rl.SubTotalPrice,
        //             PaymentLocaleId = pi.PaymentLocaleId,
        //             PurLocaleId = pi.PurLocaleId,
        //             POLocaleId = pi.LocaleId,
        //             PayCodeId = pi.PayCodeId,
        //             PayDollarCodeId = pi.DollarCodeId,
        //             PayDollar = rla.PurDollarNameTw,
        //             ReceivedDate = rl.ReceivedDate,
        //             IQCResult = v.PaymentPoint == 0 ? rl.IQCResult : 4,
        //             TransferInId = rl.TransferInId,
        //             IsAccounting = rl.IsAccounting,
        //             LocaleId = pi.LocaleId,
        //             CloseMonth = rla.CloseMonth,
        //         }
        //     )
        //     .Where(i => i.TransferInId == 0 && i.IsAccounting == 0 && i.IQCResult >= 2)
        //     .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
        //     .GroupBy(i => new { i.Vendor, i.CompanyId, i.PaymentLocaleId, i.PayDollarCodeId, i.PayDollar, i.PayCodeId,i.PurLocaleId, i.LocaleId })
        //     .Select(i => new Models.Views.PayableForVendor
        //     {
        //         Vendor = i.Key.Vendor,
        //         CompanyId = i.Key.CompanyId,
        //         PaymentLocaleId = i.Key.PaymentLocaleId,
        //         PurLocaleId = i.Key.PurLocaleId,
        //         POLocaleId = i.Key.LocaleId,

        //         SubAmount = i.Sum(g => g.SubAmount),
        //         PayDollarCodeId = i.Key.PayDollarCodeId,
        //         PayDollar = i.Key.PayDollar,
        //         PayCodeId = i.Key.PayCodeId,
        //         CloseMonthFrom = i.Min(g => g.CloseMonth),
        //         CloseMonthTo = i.Max(g => g.CloseMonth)
        //     })
        //     .ToList();

        //     item.ForEach(i =>
        //     {
        //         // i.Company = company.Where(c => c.Id == i.CompanyId).Max(c => c.CompanyNo);
        //         // i.PaymentLocale = company.Where(c => c.Id == i.PaymentLocaleId).Max(c => c.CompanyNo);
        //         // i.PurLocale = company.Where(c => c.Id == i.PurLocaleId).Max(c => c.CompanyNo);
        //         i.PayCodeId = i.PayCodeId > 4 ? 0 :i.PayCodeId; // old system has bug in PayCodeId
        //         i.PayType = Type.GetPayType().Where(t => t.Id == i.PayCodeId).Max(t => t.NameTw);
        //     });

        //     return item.AsQueryable();
        // }
        // public IQueryable<Models.Views.POItemForVendor> GetMonthAP(string predicate)
        // {
        //     var company = Company.Get().ToList();
        //     var item = (
        //         from rl in ReceivedLog.Get()
        //         join rla in ReceivedLogAdd.Get() on new { ReceivedLogId = rl.Id, LocaleId = rl.LocaleId } equals new { ReceivedLogId = rla.ReceivedLogId, LocaleId = rla.LocaleId } into rlaGRP
        //         from rla in rlaGRP.DefaultIfEmpty()
        //         join pi in POItem.Get() on new { POItemId = rl.POItemId, LocaleId = rl.RefLocaleId } equals new { POItemId = pi.Id, LocaleId = pi.LocaleId }
        //         join p in PO.Get() on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = p.Id, LocaleId = p.LocaleId }
        //         join o in Orders.Get() on new { OrderNo = pi.OrderNo } equals new { OrderNo = o.OrderNo } into oGRP
        //         from o in oGRP.DefaultIfEmpty()
        //         join v in Vendor.Get() on new { VendorId = rl.ShippingListVendorId, LocaleId = rl.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId }
        //         select new {
        //             Vendor = v.NameTw,
        //             PaymentPoint = v.PaymentPoint,
        //             DayOfMonth = v.DayOfMonth,
        //             RefPONo = rla.RefPONo,
        //             MaterialId = rla.MaterialId,
        //             MaterialNameTw = rla.MaterialNameTw,
        //             PODate = p.PODate,
        //             ReceivedDate = rl.ReceivedDate,
        //             UnitPrice = rl.UnitPrice,
        //             SubTotalPrice = rl.SubTotalPrice,
        //             PurDollarNameTw = rla.PurDollarNameTw,
        //             PurUnitNameTw = rla.PurUnitNameTw,
        //             PurQty = pi.Qty,
        //             IQCGetQty = rl.IQCGetQty,
        //             ReceivedQty = rl.ReceivedQty,
        //             VendorETD = p.VendorETD,
        //             OrderNo = o.OrderNo,
        //             StyleNo = o.StyleNo,
        //             LCSD = o.LCSD,
        //             CSD = o.CSD,
        //             CompanyId = pi.CompanyId != null ? pi.CompanyId : o.CompanyId,
        //             PaymentLocaleId = pi.PaymentLocaleId,
        //             PurLocaleId = pi.PurLocaleId,
        //             LocaleId = pi.LocaleId,
        //             PONo = p.BatchNo+"-"+p.SeqId,
        //             Confirmer = rl.ModifyUserName,
        //             CloseMonth = rla.CloseMonth,
        //             IQCResult = v.PaymentPoint == 0 ? rl.IQCResult : 4,
        //             TransferInId = rl.TransferInId,
        //             IsAccounting = rl.IsAccounting,
        //             POItemId = pi.Id
        //         }
        //     )
        //     .Where(i => i.TransferInId == 0 && i.IsAccounting == 0 && i.IQCResult >= 2)
        //     .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
        //     .Select(i => new Models.Views.POItemForVendor
        //     {
        //         Vendor = i.Vendor,
        //         PaymentPoint = i.PaymentPoint,
        //         DayOfMonth = i.DayOfMonth,
        //         RefPONo = i.RefPONo,
        //         MaterialId = i.MaterialId,
        //         MaterialNameTw = i.MaterialNameTw,
        //         PODate = i.PODate,
        //         ReceivedDate = i.ReceivedDate,
        //         UnitPrice = i.UnitPrice,
        //         SubTotalPrice = i.SubTotalPrice,
        //         PurDollarNameTw = i.PurDollarNameTw,
        //         PurUnitNameTw = i.PurUnitNameTw,
        //         PurQty = i.PurQty,
        //         IQCGetQty = i.IQCGetQty,
        //         ReceivedQty = i.ReceivedQty,
        //         VendorETD = i.VendorETD,
        //         OrderNo = i.OrderNo,
        //         StyleNo = i.StyleNo,
        //         LCSD = i.LCSD,
        //         CSD = i.CSD,
        //         CompanyId = i.CompanyId,
        //         PaymentLocaleId = i.PaymentLocaleId,
        //         PurLocaleId = i.PurLocaleId,
        //         LocaleId = i.LocaleId,
        //         PONo = i.PONo,
        //         Confirmer = i.Confirmer,
        //         CloseMonth = i.CloseMonth,
        //         POItemId = i.POItemId
        //     })
        //     .ToList();

        //     // item.ForEach(i =>
        //     // {
        //     //     i.CompanyNo = company.Where(c => c.Id == i.CompanyId).Max(c => c.CompanyNo);
        //     //     i.PurLocale = company.Where(c => c.Id == i.PurLocaleId).Max(c => c.CompanyNo);
        //     //     i.PaymentLocale = company.Where(c => c.Id == i.PaymentLocaleId).Max(c => c.CompanyNo);
        //     // });

        //     return item.AsQueryable();
        // }
   
    }
}