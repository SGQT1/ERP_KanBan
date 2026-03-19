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
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;

namespace ERP.Services.Business
{
    public class APMonthService : BusinessService
    {
        private ERP.Services.Business.Entities.APMonthService APMonth { get; set; }
        private ERP.Services.Business.Entities.APMonthItemService APMonthItem { get; set; }
        private ERP.Services.Business.Entities.APMonthItemDiscountService APMonthItemDiscount { get; set; }


        private ERP.Services.Business.Entities.APMonthOtherService APMonthOther { get; set; }
        private ERP.Services.Business.Entities.APMonthOtherItemService APMonthOtherItem { get; set; }
        private ERP.Services.Business.Entities.APMonthOtherItemDiscountService APMonthOtherItemDiscount { get; set; }

        private ERP.Services.Business.Entities.ReceivedLogService ReceivedLog { get; set; }
        private ERP.Services.Business.Entities.ReceivedLogAddService ReceivedLogAdd { get; set; }
        private ERP.Services.Entities.WarehouseService Warehouse { get; set; }
        private ERP.Services.Entities.VendorService Vendor { get; set; }
        private ERP.Services.Entities.POItemService POItem { get; set; }
        // private ERP.Services.Entities.CompanyService Company { get; set; }
        private ERP.Services.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Entities.POService PO { get; set; }
        public APMonthService(
            ERP.Services.Business.Entities.APMonthService apMonthService,
            ERP.Services.Business.Entities.APMonthItemService apMonthItemService,
            ERP.Services.Business.Entities.APMonthItemDiscountService apMonthItemDiscountService,

            ERP.Services.Business.Entities.ReceivedLogService receivedLogService,
            ERP.Services.Business.Entities.ReceivedLogAddService receivedLogAddService,
            ERP.Services.Entities.WarehouseService warehouseService,
            ERP.Services.Entities.VendorService vendorService,
            ERP.Services.Entities.POItemService poItemService,
            // ERP.Services.Entities.CompanyService companyService,
            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Entities.POService poService,

            ERP.Services.Business.Entities.APMonthOtherService apMonthOtherService,
            ERP.Services.Business.Entities.APMonthOtherItemService aPMonthOtherItemService,
            ERP.Services.Business.Entities.APMonthOtherItemDiscountService aPMonthOtherItemDiscountService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            APMonth = apMonthService;
            APMonthItem = apMonthItemService;
            APMonthItemDiscount = apMonthItemDiscountService;

            Orders = ordersService;
            PO = poService;
            POItem = poItemService;
            ReceivedLog = receivedLogService;
            ReceivedLogAdd = receivedLogAddService;
            Vendor = vendorService;
            // Company = companyService;
            Warehouse = warehouseService;

            APMonthOther = apMonthOtherService;
            APMonthOtherItem = aPMonthOtherItemService;
            APMonthOtherItemDiscount = aPMonthOtherItemDiscountService;
        }

        public ERP.Models.Views.APMonthGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.APMonthGroup { };
            var apmonth = APMonth.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (apmonth != null)
            {
                group.APMonth = apmonth;
                group.APMonthItem = APMonthItem.Get().Where(i => i.APMonthId == id && i.LocaleId == localeId && i.IsGet == 1).OrderBy(i => i.SeqNo).ToList();
                group.APMonthItemDiscount = APMonthItemDiscount.Get().Where(i => i.APMonthId == id && i.LocaleId == localeId).OrderBy(i => i.SeqNo).ToList();
            }
            return group;
        }

        public ERP.Models.Views.APMonthGroup GetForOthers(int id, int localeId)
        {
            var group = new ERP.Models.Views.APMonthGroup { };
            var apmonth = APMonthOther.Get().Where(i => i.Id == id && i.LocaleId == localeId)
                .Select(i => new APMonth
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
                    CloseUserName = i.ModifyUserName,
                    CloseTime = i.LastUpdateTime,
                    APGetPre = i.APGetPre,
                    PaymentLocaleId = i.PaymentLocaleId,
                    APType = 2,

                })
                .FirstOrDefault();
            if (apmonth != null)
            {
                group.APMonth = apmonth;
                group.APMonthItem = APMonthOtherItem.Get().Where(i => i.APMonthOtherId == id && i.LocaleId == localeId && i.IsGet == 1)
                    .Select(i => new APMonthItem
                    {
                        Id = i.Id,
                        LocaleId = i.LocaleId,
                        APMonthId = i.APMonthOtherId,
                        IsGet = i.IsGet,
                        ReceivedDate = i.ReceivedDate,
                        ReceivedRefNo = i.ReceivedRefNo,
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

                        PaymentLocaleId = i.PaymentLocaleId,
                        POItemId = i.POItemId,
                        ReceivedLocaleId = i.ReceivedLocaleId,
                        ReceivedLogId = i.ReceivedLogId,

                    })
                    .OrderBy(i => i.SeqNo).ToList();
                group.APMonthItemDiscount = APMonthOtherItemDiscount.Get().Where(i => i.APMonthOtherId == id && i.LocaleId == localeId)
                    .Select(i => new APMonthItemDiscount
                    {
                        Id = i.Id,
                        LocaleId = i.LocaleId,
                        APMonthId = i.APMonthOtherId,
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
                    })
                    .OrderBy(i => i.SeqNo).ToList();
            }
            return group;
        }

        public IQueryable<Models.Views.APMonth> GetWithItem(string predicate)
        {
            var items = (
                from ap in APMonth.Get()
                join api in APMonthItem.Get() on new { APId = (decimal?)ap.Id, LocaleId = (decimal?)ap.LocaleId } equals new { APId = api.APMonthId, LocaleId = api.LocaleId } into apiGRP
                from api in apiGRP.DefaultIfEmpty()
                select new
                {
                    Id = ap.Id,
                    LocaleId = ap.LocaleId,
                    ModifyUserName = ap.ModifyUserName,
                    LastUpdateTime = ap.LastUpdateTime,
                    APNo = ap.APNo,
                    YYYYMM = ap.YYYYMM,
                    VendorNameTw = ap.VendorNameTw,
                    AP = ap.AP,
                    Tax = ap.Tax,
                    IsClose = ap.IsClose,
                    PreAPTTL = ap.PreAPTTL,
                    APTTL = ap.APTTL,
                    APGet = ap.APGet,
                    DollarCodeName = ap.DollarCodeName,
                    BankingRate = ap.BankingRate,
                    Remark = ap.Remark,
                    ISONo = ap.ISONo,
                    ReceiveAddress = ap.ReceiveAddress,
                    TelNo = ap.TelNo,
                    PaymentCodeName = ap.PaymentCodeName,
                    Discount = ap.Discount,
                    TaxRate = ap.TaxRate,
                    IsTaxCombined = ap.IsTaxCombined,
                    CloseUserName = ap.CloseUserName,
                    CloseTime = ap.CloseTime,
                    APGetPre = ap.APGetPre,
                    PaymentLocaleId = ap.PaymentLocaleId,
                    // PaymentLocale = ap.PaymentLocale,
                    ReceivedDate = api.ReceivedDate,
                    PurUserName = api.PurUserName,
                    TypeNo = api.TypeNo,
                    OrderNo = api.WONo,
                    PONo = api.VendorMaterialNo,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new ERP.Models.Views.APMonth
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
                APType = 1,
            })
            .Distinct()
            .ToList();

            var otherItems = (
                from ap in APMonthOther.Get()
                join api in APMonthOtherItem.Get() on new { APId = ap.Id, LocaleId = ap.LocaleId } equals new { APId = api.APMonthOtherId, LocaleId = api.LocaleId } into apiGRP
                from api in apiGRP.DefaultIfEmpty()
                select new
                {
                    Id = ap.Id,
                    LocaleId = ap.LocaleId,
                    ModifyUserName = ap.ModifyUserName,
                    LastUpdateTime = ap.LastUpdateTime,
                    APNo = ap.APNo,
                    YYYYMM = ap.YYYYMM,
                    VendorNameTw = ap.VendorNameTw,
                    AP = ap.AP,
                    Tax = ap.Tax,
                    IsClose = ap.IsClose,
                    PreAPTTL = ap.PreAPTTL,
                    APTTL = ap.APTTL,
                    APGet = ap.APGet,
                    DollarCodeName = ap.DollarCodeName,
                    BankingRate = ap.BankingRate,
                    Remark = ap.Remark ?? "",
                    ISONo = ap.ISONo,
                    ReceiveAddress = ap.ReceiveAddress,
                    TelNo = ap.TelNo,
                    PaymentCodeName = ap.PaymentCodeName,
                    Discount = ap.Discount,
                    TaxRate = ap.TaxRate,
                    IsTaxCombined = ap.IsTaxCombined,
                    CloseUserName = ap.ModifyUserName,
                    CloseTime = ap.LastUpdateTime,
                    APGetPre = ap.APGetPre,
                    PaymentLocaleId = ap.PaymentLocaleId,
                    // PaymentLocale = ap.PaymentLocale,
                    ReceivedDate = api.ReceivedDate,
                    PurUserName = api.PurUserName ?? "",
                    TypeNo = api.TypeNo ?? "",
                    OrderNo = api.WONo ?? "",
                    PONo = api.VendorMaterialNo ?? "",
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new ERP.Models.Views.APMonth
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
                APType = 2,
            })
            .Distinct()
            .ToList();

            var result = items.Union(otherItems).ToList();
            return result.AsQueryable();
        }
        public IQueryable<Models.Views.APMonth> GetWithItem1(string predicate)
        {
            var result = (
                from ap in APMonth.Get()
                join api in APMonthItem.Get() on new { APId = (decimal?)ap.Id, LocaleId = (decimal?)ap.LocaleId } equals new { APId = api.APMonthId, LocaleId = api.LocaleId } into apiGRP
                from api in apiGRP.DefaultIfEmpty()
                select new
                {
                    Id = ap.Id,
                    LocaleId = ap.LocaleId,
                    ModifyUserName = ap.ModifyUserName,
                    LastUpdateTime = ap.LastUpdateTime,
                    APNo = ap.APNo,
                    YYYYMM = ap.YYYYMM,
                    VendorNameTw = ap.VendorNameTw,
                    AP = ap.AP,
                    Tax = ap.Tax,
                    IsClose = ap.IsClose,
                    PreAPTTL = ap.PreAPTTL,
                    APTTL = ap.APTTL,
                    APGet = ap.APGet,
                    DollarCodeName = ap.DollarCodeName,
                    BankingRate = ap.BankingRate,
                    Remark = ap.Remark,
                    ISONo = ap.ISONo,
                    ReceiveAddress = ap.ReceiveAddress,
                    TelNo = ap.TelNo,
                    PaymentCodeName = ap.PaymentCodeName,
                    Discount = ap.Discount,
                    TaxRate = ap.TaxRate,
                    IsTaxCombined = ap.IsTaxCombined,
                    CloseUserName = ap.CloseUserName,
                    CloseTime = ap.CloseTime,
                    APGetPre = ap.APGetPre,
                    PaymentLocaleId = ap.PaymentLocaleId,
                    // PaymentLocale = ap.PaymentLocale,
                    ReceivedDate = api.ReceivedDate,
                    PurUserName = api.PurUserName,
                    TypeNo = api.TypeNo,
                    OrderNo = api.WONo,
                    PONo = api.VendorMaterialNo
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new ERP.Models.Views.APMonth
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
                PaymentLocaleId = i.PaymentLocaleId
            })
            .Distinct();
            return result;
        }
        // 應付帳款
        public IQueryable<Models.Views.APMonthItem> BuildAPMonthItemForRecevied(string predicate)
        {
            // var company = Company.Get().Select(i => new { i.Id, i.CompanyNo}).ToList();
            var startDate = DateTime.Now.AddDays(-365).ToString("yyyyMM");
            var result = (
                from rl in ReceivedLog.Get()
                join rla in ReceivedLogAdd.Get() on new { ReceivedLogId = rl.Id, LocaleId = rl.LocaleId } equals new { ReceivedLogId = rla.ReceivedLogId, LocaleId = rla.LocaleId } into rlaGRP
                from rla in rlaGRP.DefaultIfEmpty()
                join pi in POItem.Get() on new { POItem = rl.POItemId, LocaleId = rl.RefLocaleId } equals new { POItem = pi.Id, LocaleId = pi.LocaleId } into piGRP
                from pi in piGRP.DefaultIfEmpty()
                join v in Vendor.Get() on new { VendorId = rl.ShippingListVendorId, LocaleId = rl.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                join api in APMonthItem.Get() on new { ReceivedLogId = (decimal?)rl.Id, LocaleId = (decimal?)rl.LocaleId } equals new { ReceivedLogId = api.ReceivedLogId, LocaleId = api.ReceivedLocaleId } into apiGRP
                from api in apiGRP.DefaultIfEmpty()
                join apio in APMonthOtherItem.Get() on new { ReceivedLogId = (decimal?)rl.Id, LocaleId = (decimal?)rl.LocaleId } equals new { ReceivedLogId = apio.ReceivedLogId, LocaleId = apio.ReceivedLocaleId } into apioGRP
                from apio in apioGRP.DefaultIfEmpty()
                select new
                {
                    Id = 0,
                    LocaleId = api.LocaleId,

                    APMonthId = (decimal?)api.Id,
                    IsGet = api.IsGet,

                    APMonthOtherId = (decimal?)apio.Id,
                    APMonthOtherIsGet = apio.IsGet,

                    CloseMonth = Convert.ToInt32(rla.CloseMonth),
                    ReceivedDate = rl.ReceivedDate,
                    ReceivedRefNo = rl.Id.ToString(),
                    PayQty = rl.PayQty,
                    APAmount = rl.SubTotalPrice,
                    UnitPrice = rl.UnitPrice,
                    APTTL = rl.SubTotalPrice,
                    APGet = rl.SubTotalPrice,
                    IsAccounting = rl.IsAccounting,
                    WONo = rl.OrderNo,
                    WarehouseNo = Warehouse.Get().Where(i => i.Id == rl.WarehouseId && i.LocaleId == rl.LocaleId).Max(i => i.WarehouseNo),
                    PurLocaleId = rl.RefLocaleId,
                    POItemId = rl.POItemId,
                    ReceivedLocaleId = rl.LocaleId,
                    ReceivedLogId = rl.Id,

                    DollarCodeName = rla.PurDollarNameTw,
                    APYM = rla.CloseMonth,
                    PUnit = rla.PurUnitNameTw,
                    TypeNo = rla.MaterialNameTw,
                    VendorMaterialNo = rla.RefPONo,

                    PurUserName = pi.ModifyUserName,
                    PaymentLocaleId = pi.PaymentLocaleId,
                    Spec = pi.Qty.ToString(),
                    Vendor = v.NameTw,
                    IQCResult = v.PaymentPoint == 0 ? rl.IQCResult : 4,

                    BankingRate = 1,
                    TaxRate = 0,
                    APTax = 0,
                    PreAPGet = 0,
                    UniInvoiceNo = "",
                    Remark = "",
                    Discount = 0,
                    SeqNo = "",
                    IsDraft = 0,
                    VoucherNo = "",
                }
            )
            .Where(i => i.CloseMonth >= Convert.ToInt32(startDate) && i.IsAccounting == 0 && i.IQCResult >= 2 && ((i.APMonthId > 0 && i.IsGet == 0) || (i.APMonthId == null)) && ((i.APMonthOtherId > 0 && i.APMonthOtherIsGet == 0) || (i.APMonthOtherId == null)))
            // .Where(i => i.IsAccounting == 0 && i.IQCResult >= 2 && ((i.APMonthId > 0 && i.IsGet == 0) || (i.APMonthId == null)) && ((i.APMonthOtherId > 0 && i.APMonthOtherIsGet == 0) || (i.APMonthOtherId == null)))
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new Models.Views.APMonthItem
            {
                Id = null,
                LocaleId = i.LocaleId,
                APMonthId = null,
                IsGet = i.IsGet,
                ReceivedDate = i.ReceivedDate,
                ReceivedRefNo = i.ReceivedRefNo,
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
                PaymentLocaleId = i.PaymentLocaleId,
                POItemId = i.POItemId,
                ReceivedLocaleId = i.ReceivedLocaleId,
                ReceivedLogId = i.ReceivedLogId,
                Vendor = i.Vendor

            })
            .ToList();

            return result.AsQueryable();
        }

        public IQueryable<Models.Views.APMonthItem> BuildAPMonthItemForRecevied1(string predicate)
        {
            // var company = Company.Get().Select(i => new { i.Id, i.CompanyNo}).ToList();
            var startDate = DateTime.Now.AddDays(-365).ToString("yyyyMM");
            var result = (
                from rl in ReceivedLog.Get()
                join rla in ReceivedLogAdd.Get() on new { ReceivedLogId = rl.Id, LocaleId = rl.LocaleId } equals new { ReceivedLogId = rla.ReceivedLogId, LocaleId = rla.LocaleId } into rlaGRP
                from rla in rlaGRP.DefaultIfEmpty()
                join pi in POItem.Get() on new { POItem = rl.POItemId, LocaleId = rl.RefLocaleId } equals new { POItem = pi.Id, LocaleId = pi.LocaleId } into piGRP
                from pi in piGRP.DefaultIfEmpty()
                join v in Vendor.Get() on new { VendorId = rl.ShippingListVendorId, LocaleId = rl.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                join api in APMonthItem.Get() on new { ReceivedLogId = rl.Id, LocaleId = rl.LocaleId } equals new { ReceivedLogId = (decimal)api.ReceivedLogId, LocaleId = (decimal)api.ReceivedLocaleId } into apiGRP
                from api in apiGRP.DefaultIfEmpty()
                select new
                {
                    Id = api.Id,
                    LocaleId = api.LocaleId,
                    IsGet = api.IsGet,
                    APMonthId = api.Id,

                    CloseMonth = Convert.ToInt32(rla.CloseMonth),
                    ReceivedDate = rl.ReceivedDate,
                    ReceivedRefNo = rl.Id.ToString(),
                    PayQty = rl.PayQty,
                    APAmount = rl.SubTotalPrice,
                    UnitPrice = rl.UnitPrice,
                    APTTL = rl.SubTotalPrice,
                    APGet = rl.SubTotalPrice,
                    IsAccounting = rl.IsAccounting,
                    WONo = rl.OrderNo,
                    WarehouseNo = Warehouse.Get().Where(i => i.Id == rl.WarehouseId && i.LocaleId == rl.LocaleId).Max(i => i.WarehouseNo),
                    PurLocaleId = rl.RefLocaleId,
                    POItemId = rl.POItemId,
                    ReceivedLocaleId = rl.LocaleId,
                    ReceivedLogId = rl.Id,

                    DollarCodeName = rla.PurDollarNameTw,
                    APYM = rla.CloseMonth,
                    PUnit = rla.PurUnitNameTw,
                    TypeNo = rla.MaterialNameTw,
                    VendorMaterialNo = rla.RefPONo,

                    PurUserName = pi.ModifyUserName,
                    PaymentLocaleId = pi.PaymentLocaleId,
                    Spec = pi.Qty.ToString(),
                    Vendor = v.NameTw,
                    IQCResult = v.PaymentPoint == 0 ? rl.IQCResult : 4,

                    BankingRate = 1,
                    TaxRate = 0,
                    APTax = 0,
                    PreAPGet = 0,
                    UniInvoiceNo = "",
                    Remark = "",
                    Discount = 0,
                    SeqNo = "",
                    IsDraft = 0,
                    VoucherNo = "",
                }
            )
            .Where(i => i.CloseMonth >= Convert.ToInt32(startDate) && i.IsAccounting == 0 && i.IQCResult >= 2 && ((i.Id > 0 && i.IsGet == 0) || (i.Id == null)))
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new Models.Views.APMonthItem
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                APMonthId = i.Id,
                IsGet = i.IsGet,
                ReceivedDate = i.ReceivedDate,
                ReceivedRefNo = i.ReceivedRefNo,
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
                PaymentLocaleId = i.PaymentLocaleId,
                POItemId = i.POItemId,
                ReceivedLocaleId = i.ReceivedLocaleId,
                ReceivedLogId = i.ReceivedLogId,
                Vendor = i.Vendor

            })
            .ToList();
            return result.AsQueryable();
        }

        public ERP.Models.Views.APMonthGroup Save(APMonthGroup item)
        {
            var apMonth = item.APMonth;
            var apMonthItem = item.APMonthItem.ToList();
            var apMonthItemDiscount = item.APMonthItemDiscount.ToList();

            if (apMonth != null)
            {
                if (apMonth.APType == 1)
                {
                    try
                    {
                        UnitOfWork.BeginTransaction();

                        //APMonth
                        {
                            var _apMonth = APMonth.Get().Where(i => i.LocaleId == apMonth.LocaleId && i.Id == apMonth.Id).FirstOrDefault();

                            if (_apMonth != null)
                            {
                                apMonth.Id = _apMonth.Id;
                                apMonth.LocaleId = _apMonth.LocaleId;
                                apMonth = APMonth.Update(apMonth);
                            }
                            else
                            {
                                apMonth = APMonth.Create(apMonth);
                            }
                        }

                        //APMonth Item
                        {
                            if (apMonth.Id != 0)
                            {
                                apMonthItem.ForEach(i => i.APMonthId = apMonth.Id);
                                apMonthItemDiscount.ForEach(i => i.APMonthId = apMonth.Id);

                                APMonthItemDiscount.RemoveRange(i => i.APMonthId == apMonth.Id && i.LocaleId == apMonth.LocaleId);
                                APMonthItem.RemoveRange(i => i.APMonthId == apMonth.Id && i.LocaleId == apMonth.LocaleId);

                                APMonthItemDiscount.CreateRange(apMonthItemDiscount);
                                APMonthItem.CreateRange(apMonthItem);
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
                else
                {
                    // 刪除掉Other裡面的，移到APMonth
                    try
                    {
                        UnitOfWork.BeginTransaction();
                        //APMonth
                        {
                            var _apMonthOther = APMonthOther.Get().Where(i => i.LocaleId == apMonth.LocaleId && i.Id == apMonth.Id).FirstOrDefault();
                            if (_apMonthOther != null) {
                                APMonthOtherItemDiscount.RemoveRange(i => i.APMonthOtherId == _apMonthOther.Id && i.LocaleId == apMonth.LocaleId);
                                APMonthOtherItem.RemoveRange(i => i.APMonthOtherId == _apMonthOther.Id && i.LocaleId == apMonth.LocaleId);
                                APMonthOther.Remove(_apMonthOther); 
                            }
                            apMonth = APMonth.Create(apMonth);
                        }

                        //APMonth Item
                        {
                            if (apMonth.Id != 0)
                            {
                                apMonthItem.ForEach(i => i.APMonthId = apMonth.Id);
                                apMonthItemDiscount.ForEach(i => i.APMonthId = apMonth.Id);

                                APMonthItemDiscount.CreateRange(apMonthItemDiscount);
                                APMonthItem.CreateRange(apMonthItem);
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

            }
            return Get((int)apMonth.Id, (int)apMonth.LocaleId);

        }

        public ERP.Models.Views.APMonthGroup Save1(APMonthGroup item)
        {
            var apMonth = item.APMonth;
            var apMonthItem = item.APMonthItem.ToList();
            var apMonthItemDiscount = item.APMonthItemDiscount.ToList();

            if (apMonth != null)
            {
                try
                {
                    UnitOfWork.BeginTransaction();

                    //APMonth
                    {
                        var _apMonth = APMonth.Get().Where(i => i.LocaleId == apMonth.LocaleId && i.Id == apMonth.Id).FirstOrDefault();

                        if (_apMonth != null)
                        {
                            apMonth.Id = _apMonth.Id;
                            apMonth.LocaleId = _apMonth.LocaleId;
                            apMonth = APMonth.Update(apMonth);
                        }
                        else
                        {
                            apMonth = APMonth.Create(apMonth);
                        }
                    }

                    //APMonth Item
                    {
                        if (apMonth.Id != 0)
                        {
                            apMonthItem.ForEach(i => i.APMonthId = apMonth.Id);
                            apMonthItemDiscount.ForEach(i => i.APMonthId = apMonth.Id);

                            APMonthItemDiscount.RemoveRange(i => i.APMonthId == apMonth.Id && i.LocaleId == apMonth.LocaleId);
                            APMonthItem.RemoveRange(i => i.APMonthId == apMonth.Id && i.LocaleId == apMonth.LocaleId);

                            APMonthItemDiscount.CreateRange(apMonthItemDiscount);
                            APMonthItem.CreateRange(apMonthItem);
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
            return Get((int)apMonth.Id, (int)apMonth.LocaleId);

        }

        public void Remove(APMonthGroup item)
        {
            var apMonth = item.APMonth;
            UnitOfWork.BeginTransaction();
            try
            {
                APMonthItemDiscount.RemoveRange(i => i.APMonthId == apMonth.Id && i.LocaleId == apMonth.LocaleId);
                APMonthItem.RemoveRange(i => i.APMonthId == apMonth.Id && i.LocaleId == apMonth.LocaleId);
                APMonth.Remove(apMonth);

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
