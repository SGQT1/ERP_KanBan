using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Search
{
    public class MonthAPService : SearchService
    {
        private ERP.Services.Entities.APMonthService APMonth { get; set; }
        private ERP.Services.Entities.APMonthItemService APMonthItem { get; set; }
        private ERP.Services.Entities.APMonthItemDiscountService APMonthItemDiscount { get; set; }
        private ERP.Services.Entities.APMonthOtherService APMonthOther { get; set; }
        private ERP.Services.Entities.APMonthOtherItemService APMonthOtherItem { get; set; }
        private ERP.Services.Entities.APMonthOtherItemDiscountService APMonthOtherItemDiscount { get; set; }

        private ERP.Services.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Entities.POService PO { get; set; }
        private ERP.Services.Entities.POItemService POItem { get; set; }
        private ERP.Services.Entities.PurBatchItemService PurBatchItem { get; set; }
        private ERP.Services.Entities.ReceivedLogService ReceivedLog { get; set; }
        private ERP.Services.Entities.ReceivedLogAddService ReceivedLogAdd { get; set; }
        private ERP.Services.Entities.VendorService Vendor { get; set; }
        private ERP.Services.Entities.CodeItemService CodeItem { get; set; }
        private ERP.Services.Business.Entities.TypeService Type { get; set; }

        private ERP.Services.Entities.MRPItemService MRPItem { get; set; }
        private ERP.Services.Entities.MRPItemOrdersService MRPItemOrders { get; set; }

        public MonthAPService(
            ERP.Services.Entities.APMonthService apMonthService,
            ERP.Services.Entities.APMonthItemService apMonthItemService,
            ERP.Services.Entities.APMonthItemDiscountService apMonthItemDiscountService,
            ERP.Services.Entities.APMonthOtherService apMonthOtherService,
            ERP.Services.Entities.APMonthOtherItemService apMonthOtherItemService,
            ERP.Services.Entities.APMonthOtherItemDiscountService apMonthOtherItemDiscountService,

            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Entities.POService poService,
            ERP.Services.Entities.POItemService poItemService,
            ERP.Services.Entities.PurBatchItemService purBatchItemService,
            ERP.Services.Entities.ReceivedLogService receivedLogService,
            ERP.Services.Entities.ReceivedLogAddService receivedLogAddService,
            ERP.Services.Entities.VendorService vendorService,
            ERP.Services.Entities.CodeItemService codeItemService,
            ERP.Services.Business.Entities.TypeService typeService,

            ERP.Services.Entities.MRPItemService mrpItemService,
            ERP.Services.Entities.MRPItemOrdersService mrpItemOrdersService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            APMonth = apMonthService;
            APMonthItem = apMonthItemService;
            APMonthItemDiscount = apMonthItemDiscountService;
            APMonthOther = apMonthOtherService;
            APMonthOtherItem = apMonthOtherItemService;
            APMonthOtherItemDiscount = apMonthOtherItemDiscountService;

            Orders = ordersService;
            PO = poService;
            POItem = poItemService;
            PurBatchItem = purBatchItemService;
            ReceivedLog = receivedLogService;
            ReceivedLogAdd = receivedLogAddService;
            Vendor = vendorService;
            CodeItem = codeItemService;
            Type = typeService;

            MRPItem = mrpItemService;
            MRPItemOrders = mrpItemOrdersService;
        }

        public IQueryable<Models.Views.PayableForVendor> GetMonthAPForRecd(string predicate)
        {
            // var company = Company.Get().ToList();
            var items = (
                from rl in ReceivedLog.Get()
                join rla in ReceivedLogAdd.Get() on new { ReceivedLogId = rl.Id, LocaleId = rl.LocaleId } equals new { ReceivedLogId = rla.ReceivedLogId, LocaleId = rla.LocaleId } into rlaGRP
                from rla in rlaGRP.DefaultIfEmpty()
                join pi in POItem.Get() on new { POItemId = rl.POItemId, LocaleId = rl.RefLocaleId } equals new { POItemId = pi.Id, LocaleId = pi.LocaleId }
                join p in PO.Get() on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = p.Id, LocaleId = p.LocaleId }
                join o in Orders.Get() on new { OrderNo = pi.OrderNo } equals new { OrderNo = o.OrderNo } into oGRP
                from o in oGRP.DefaultIfEmpty()
                join v in Vendor.Get() on new { VendorId = rl.ShippingListVendorId, LocaleId = rl.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId }
                select new
                {
                    Vendor = v.NameTw,
                    VendorId = rl.ShippingListVendorId,

                    CompanyId = pi.CompanyId != null ? pi.CompanyId : pi.PurLocaleId,
                    SubAmount = rl.SubTotalPrice,
                    PaymentLocaleId = pi.PaymentLocaleId,
                    PurLocaleId = pi.PurLocaleId,
                    POLocaleId = pi.LocaleId,
                    // PayCodeId = pi.PayCodeId,
                    PayCodeId = 0,
                    PayDollarCodeId = rla.PurDollarCodeId,
                    PayDollar = rla.PurDollarNameTw,
                    ReceivedDate = rl.ReceivedDate,
                    IQCResult = v.PaymentPoint == 0 ? rl.IQCResult : 4, // 0:檢驗入庫, 1:收貨清點，為0的時候，看收貨的驗收狀態，為1的時候為允收
                    TransferInId = rl.TransferInId,
                    IsAccounting = rl.IsAccounting,
                    // LocaleId = pi.LocaleId,
                    CloseMonth = Convert.ToInt32(rla.CloseMonth),       // for condition, cause column type is string , query by linq is too slow, covert to interger 
                    CloseMonth1 = rla.CloseMonth,                        // for select column
                    Purchaser = pi.ModifyUserName,
                }
            )
            .Where(i => i.TransferInId == 0 && i.IsAccounting == 0 && i.IQCResult >= 2)
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .GroupBy(i => new { i.Vendor, i.CompanyId, i.PaymentLocaleId, i.PayDollarCodeId, i.PayDollar, i.PayCodeId, i.PurLocaleId })
            .Select(i => new Models.Views.PayableForVendor
            {
                Vendor = i.Key.Vendor,
                CompanyId = i.Key.CompanyId,
                PaymentLocaleId = i.Key.PaymentLocaleId,
                PurLocaleId = i.Key.PurLocaleId,
                POLocaleId = i.Key.PurLocaleId,

                SubAmount = i.Sum(g => g.SubAmount),
                PayDollarCodeId = i.Key.PayDollarCodeId,
                PayDollar = i.Key.PayDollar,
                PayCodeId = i.Key.PayCodeId,
                CloseMonthFrom = i.Min(g => g.CloseMonth1),
                CloseMonthTo = i.Max(g => g.CloseMonth1)
            })
            .ToList();

            items.ForEach(i =>
            {
                // i.PurLocale = company.Where(c => c.Id == i.PurLocaleId).Max(c => c.CompanyNo);
                // i.PaymentLocale = company.Where(c => c.Id == i.PaymentLocaleId).Max(c => c.CompanyNo);
                // i.CompanyId = i.CompanyId == null ? i.PurLocaleId : i.CompanyId; //無訂單以採購地為公司別

                i.PayCodeId = i.PayCodeId > 4 ? 0 : i.PayCodeId; // old system has bug in PayCodeId
                i.PayType = Type.GetPayType().Where(t => t.Id == i.PayCodeId).Max(t => t.NameTw);
            });

            return items.AsQueryable();
        }
        public IQueryable<Models.Views.POItemForVendor> GetMonthAPItemForRecd(string predicate)
        {
            // var company = Company.Get().ToList();
            var items = (
                from rl in ReceivedLog.Get()
                join rla in ReceivedLogAdd.Get() on new { ReceivedLogId = rl.Id, LocaleId = rl.LocaleId } equals new { ReceivedLogId = rla.ReceivedLogId, LocaleId = rla.LocaleId } into rlaGRP
                from rla in rlaGRP.DefaultIfEmpty()
                join v in Vendor.Get() on new { VendorId = rl.ShippingListVendorId, LocaleId = rl.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId }
                join pi in POItem.Get() on new { POItemId = rl.POItemId, LocaleId = rl.RefLocaleId } equals new { POItemId = pi.Id, LocaleId = pi.LocaleId }
                join p in PO.Get() on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = p.Id, LocaleId = p.LocaleId }
                join pur in PurBatchItem.Get() on new { POItemId = pi.Id, LocaleId = pi.LocaleId } equals new { POItemId = (decimal)pur.POItemId, LocaleId = pur.LocaleId }
                join o in Orders.Get() on new { OrderNo = pi.OrderNo } equals new { OrderNo = o.OrderNo } into oGRP
                from o in oGRP.DefaultIfEmpty()
                select new
                {
                    RefPONo = rla.RefPONo,
                    MaterialId = rla.MaterialId,
                    MaterialNameTw = rla.MaterialNameTw,
                    ReceivedDate = rl.ReceivedDate,
                    UnitPrice = rl.UnitPrice,
                    SubTotalPrice = rl.SubTotalPrice,
                    PurDollarNameTw = rla.PurDollarNameTw,
                    PurUnitNameTw = rla.PurUnitNameTw,
                    PurDollarCodeId = rla.PurDollarCodeId, //pi.DollarCodeId, 改為抓收貨幣別
                    Confirmer = rl.ModifyUserName,
                    IQCResult = v.PaymentPoint == 0 ? rl.IQCResult : 4,
                    TransferInId = rl.TransferInId,
                    IsAccounting = rl.IsAccounting,
                    CloseMonth = Convert.ToInt32(rla.CloseMonth),       // for condition, cause column type is string , query by linq is too slow, covert to interger 
                    CloseMonth1 = rla.CloseMonth,                        // for select column
                    PayQty = rla.PayQty,
                    FreeQty = rla.FreeQty,

                    Vendor = v.NameTw,
                    PaymentPoint = v.PaymentPoint,
                    DayOfMonth = v.DayOfMonth,

                    PODate = p.PODate,
                    PurQty = pi.Qty,
                    IQCGetQty = rl.IQCGetQty,
                    ReceivedQty = rl.ReceivedQty,
                    VendorETD = p.VendorETD,
                    OrdersId = o.Id,
                    OrderNo = o.OrderNo,
                    StyleNo = o.StyleNo,
                    LCSD = o.LCSD,
                    CSD = o.CSD,
                    CompanyId = pi.CompanyId != null ? pi.CompanyId : pi.PurLocaleId,
                    PaymentLocaleId = pi.PaymentLocaleId,
                    PurLocaleId = pi.PurLocaleId,
                    LocaleId = pi.LocaleId,
                    PONo = p.BatchNo + "-" + p.SeqId,

                    POItemId = pi.Id,

                    Purchaser = pi.ModifyUserName,
                    PurPlanQty = pur.PlanQty,
                }
            )
            .Where(i => i.TransferInId == 0 && i.IsAccounting == 0 && i.IQCResult >= 2)
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new Models.Views.POItemForVendor
            {
                Vendor = i.Vendor,
                PaymentPoint = i.PaymentPoint,
                DayOfMonth = i.DayOfMonth,
                RefPONo = i.RefPONo,
                MaterialId = i.MaterialId,
                MaterialNameTw = i.MaterialNameTw,
                PODate = i.PODate,
                ReceivedDate = i.ReceivedDate,
                UnitPrice = i.UnitPrice,
                SubTotalPrice = i.SubTotalPrice,
                PurDollarNameTw = i.PurDollarNameTw,
                PurUnitNameTw = i.PurUnitNameTw,
                PurDollarCodeId = i.PurDollarCodeId,
                PurQty = i.PurQty,
                IQCGetQty = i.IQCGetQty,
                ReceivedQty = i.ReceivedQty,
                VendorETD = i.VendorETD,
                OrdersId = i.OrdersId,
                OrderNo = i.OrderNo,
                StyleNo = i.StyleNo,
                LCSD = i.LCSD,
                CSD = i.CSD,
                CompanyId = i.CompanyId,
                PaymentLocaleId = i.PaymentLocaleId,
                PurLocaleId = i.PurLocaleId,
                LocaleId = i.LocaleId,
                PONo = i.PONo,
                Confirmer = i.Confirmer,
                CloseMonth = i.CloseMonth1,
                POItemId = i.POItemId,
                PayQty = i.PayQty,
                FreeQty = i.FreeQty,
                Purchaser = i.Purchaser,
                PurPlanQty = i.PurPlanQty,
            })
            .ToList();

            if (items.Any())
            {
                var oIds = items.Select(i => i.OrdersId).Distinct().ToList();
                var oNos = items.Select(i => i.OrderNo).Distinct().ToList();
                var mIds = items.Select(i => i.MaterialId).Distinct().ToList();
                var mNames = items.Select(i => i.MaterialNameTw).Distinct().ToList();
                var poItemIds = items.Select(i => i.POItemId).Distinct().ToList();

                var localeId = items.Max(i => i.LocaleId);

                // S2: 找出管制表裡有下單的材料
                // MRPItem
                var mrpItems = (
                    from o in Orders.Get()
                    join mi in MRPItem.Get().Where(i => i.LocaleId == localeId && oIds.Contains(i.OrdersId) && mIds.Contains(i.MaterialId))
                    on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = mi.OrdersId, LocaleId = mi.LocaleId }
                    select new
                    {
                        OrdersId = o.Id,
                        LocaleId = o.LocaleId,
                        OrderNo = o.OrderNo,
                        MaterialId = mi.MaterialId,
                        Material = mi.MaterialNameTw,
                        ParentMaterial = mi.ParentMaterialId,
                        Unit = mi.UnitNameTw,
                        Total = mi.Total,
                        Part = mi.PartId
                    }
                ).ToList();

                // get MRPItemOrders by OrderNo, Material and has POItem 
                var mrpItemOrders = (
                    from o in Orders.Get()
                    join mi in MRPItemOrders.Get().Where(i => i.LocaleId == localeId && oIds.Contains(i.OrdersId) && mIds.Contains(i.MaterialId))
                    on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = mi.OrdersId, LocaleId = mi.LocaleId }
                    select new
                    {
                        OrdersId = o.Id,
                        LocaleId = o.LocaleId,
                        OrderNo = o.OrderNo,
                        MaterialId = mi.MaterialId,
                        Material = mi.MaterialNameTw,
                        ParentMaterial = mi.ParentMaterialId,
                        Unit = mi.UnitNameTw,
                        Total = mi.Total,
                        Part = mi.PartId
                    }
                ).ToList();

                //Combind
                var tmpItems = mrpItems.Union(mrpItemOrders);
                var bomItems = tmpItems
                .GroupBy(g => new { g.OrdersId, g.LocaleId, g.Material, g.Unit, g.MaterialId, g.ParentMaterial })
                .Select(g => new
                {
                    OrderId = g.Key.OrdersId,
                    LocaleId = g.Key.LocaleId,
                    Material = g.Key.Material,
                    MaterialId = g.Key.MaterialId,
                    ParentMaterial = g.Key.ParentMaterial,
                    Unit = g.Key.Unit,
                    SubUsage = g.Sum(i => i.Total)
                })
                .ToList();

                //找出這張單所有的收貨資料
                var receivedItems = (
                    from rl in ReceivedLog.Get()
                    join ra in ReceivedLogAdd.Get() on new { Id = rl.Id, LocaleId = rl.LocaleId } equals new { Id = ra.ReceivedLogId, LocaleId = ra.LocaleId }
                    where oNos.Contains(rl.OrderNo) && mNames.Contains(ra.MaterialNameTw)
                    select new
                    {
                        OrderNo = rl.OrderNo,
                        Material = ra.MaterialNameTw,
                        MaterialId = ra.MaterialId,
                        Unit = ra.PurUnitNameTw,
                        IQCGetQty = rl.IQCGetQty,
                    }
                )
                .GroupBy(g => new { g.OrderNo, g.Material, g.Unit, g.MaterialId })
                .Select(g => new
                {
                    OrderNo = g.Key.OrderNo,
                    Material = g.Key.Material,
                    MaterialId = g.Key.MaterialId,
                    Unit = g.Key.Unit,
                    PayTTL = g.Sum(i => i.IQCGetQty)
                })
                .ToList();

                //最後組合超買%
                items.ForEach(i =>
                {
                    // var mrp = bomItems.Where(m => m.LocaleId == i.LocaleId && m.OrderId == i.OrdersId && m.MaterialId == i.MaterialId).ToList();
                    i.PlanQty = bomItems.Where(m => m.LocaleId == i.LocaleId && m.OrderId == i.OrdersId && m.MaterialId == i.MaterialId).Sum(m => m.SubUsage);
                    i.PurSubTotalPrice = i.PurUnitPrice * i.PurQty;
                    i.PayTTL = receivedItems.Where(c => c.OrderNo == i.OrderNo && c.Material == i.MaterialNameTw && c.Unit == i.PurUnitNameTw).Sum(c => c.PayTTL);
                    i.PayRate = (i.PlanQty == 0 || i.PayTTL == 0) ? 0 : i.PayTTL / i.PlanQty;
                });
            }

            return items.AsQueryable();
        }

        public IQueryable<Models.Views.PayableForPay> GetMonthAPForPay(string predicate)
        {
            /* 
             * step1: 取當期產生的應付帳款(含ForOther) Item APYM ＝ YYYYMM,
             * step2: 取前期產生的應付帳款(含ForOther) APYM <> YYYYMM,
             * step3: 取折讓的應付帳款(含ForOther),
             * APType: 1-當期應付, 2-前期未付, 3-折讓
             */
            // var company = Company.Get().ToList();

            // step1: 取當期產生的應付帳款(含ForOther) Item APYM ＝ YYYYMM,
            var items = (
                from m in APMonth.Get()
                join mi in APMonthItem.Get() on new { Id = m.Id, LocaleId = m.LocaleId } equals new { Id = mi.APMonthId, LocaleId = mi.LocaleId }
                join o in Orders.Get() on new { OrderNo = mi.WONo } equals new { OrderNo = o.OrderNo } into oGRP
                from o in oGRP.DefaultIfEmpty()
                select new
                {
                    Vendor = m.VendorNameTw,
                    WarehouseNo = mi.WarehouseNo,
                    // CloseMonth = m.YYYYMM,
                    CloseMonth = Convert.ToInt32(m.YYYYMM),       // for condition, cause column type is string , query by linq is too slow, covert to interger 
                    CloseMonth1 = m.YYYYMM,                        // for select column
                    Dollar = m.DollarCodeName,
                    PreAPTTL = m.PreAPTTL,
                    APTTL = mi.APTTL,
                    Discount = mi.Discount,
                    APGet = mi.APGet,

                    APMonthId = m.Id,
                    APYM = mi.APYM,
                    CompanyId = o.CompanyId,
                    // Company = o.CompanyNo,
                    PurLocaleId = mi.PurLocaleId,
                    PaymentLocaleId = m.PaymentLocaleId,
                    LocaleId = m.LocaleId,
                    AP = m.AP,
                    Purchaser = (string?)mi.PurUserName
                }
            )
            .Where(i => i.CloseMonth1 == i.APYM)
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .GroupBy(i => new { i.PreAPTTL, i.WarehouseNo, i.APMonthId, i.AP, i.CloseMonth1, i.Vendor, i.Dollar, i.CompanyId, i.PaymentLocaleId, i.PurLocaleId, i.LocaleId })
            .Select(i => new Models.Views.PayableForPay
            {
                Vendor = i.Key.Vendor,
                WarehouseNo = i.Key.WarehouseNo,
                CloseMonth = i.Key.CloseMonth1,
                PayDollar = i.Key.Dollar,
                PreAPTTL = 0,
                APTTL = i.Sum(g => g.APTTL),
                APGetPre = i.Sum(g => g.APGet + g.Discount), // PlanPay
                Discount = i.Sum(g => g.Discount),
                APGet = i.Sum(g => g.APGet), // Pay

                APMonthId = i.Key.APMonthId,
                CompanyId = i.Key.CompanyId,
                // Company = i.Key.Company,
                PaymentLocaleId = i.Key.PaymentLocaleId,
                PurLocaleId = i.Key.PurLocaleId,
                AP = i.Key.AP,
                LocaleId = i.Key.LocaleId,
                APType = 1,
                IsDiscount = 0,
            })
            .ToList();

            var otherItems = (
                from m in APMonthOther.Get()
                join mi in APMonthOtherItem.Get() on new { Id = m.Id, LocaleId = m.LocaleId } equals new { Id = mi.APMonthOtherId, LocaleId = mi.LocaleId }
                join o in Orders.Get() on new { OrderNo = mi.WONo } equals new { OrderNo = o.OrderNo } into oGRP
                from o in oGRP.DefaultIfEmpty()
                select new
                {
                    Vendor = m.VendorNameTw,
                    WarehouseNo = mi.WarehouseNo,
                    // CloseMonth = m.YYYYMM,
                    CloseMonth = Convert.ToInt32(m.YYYYMM),       // for condition, cause column type is string , query by linq is too slow, covert to interger 
                    CloseMonth1 = m.YYYYMM,                        // for select column
                    Dollar = m.DollarCodeName,
                    PreAPTTL = m.PreAPTTL,
                    APTTL = mi.APTTL,
                    Discount = mi.Discount,
                    APGet = mi.APGet,

                    APMonthId = m.Id,
                    APYM = mi.APYM,
                    CompanyId = o.CompanyId,
                    Company = o.CompanyNo,
                    PurLocaleId = mi.PurLocaleId,
                    PaymentLocaleId = m.PaymentLocaleId,
                    LocaleId = m.LocaleId,
                    AP = m.AP,
                    Purchaser = (string?)mi.PurUserName
                }
            )
            .Where(i => i.CloseMonth1 == i.APYM)
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .GroupBy(i => new { i.PreAPTTL, i.WarehouseNo, i.APMonthId, i.AP, i.Company, i.CloseMonth1, i.Vendor, i.Dollar, i.CompanyId, i.PaymentLocaleId, i.PurLocaleId, i.LocaleId })
            .Select(i => new Models.Views.PayableForPay
            {
                Vendor = i.Key.Vendor,
                WarehouseNo = i.Key.WarehouseNo,
                CloseMonth = i.Key.CloseMonth1,
                PayDollar = i.Key.Dollar,
                PreAPTTL = 0,
                APTTL = i.Sum(g => g.APTTL),
                APGetPre = i.Sum(g => g.APGet + g.Discount), // PlanPay
                Discount = i.Sum(g => g.Discount),
                APGet = i.Sum(g => g.APGet), // Pay

                APMonthId = i.Key.APMonthId,
                CompanyId = i.Key.CompanyId,
                // Company = i.Key.Company,
                PaymentLocaleId = i.Key.PaymentLocaleId,
                PurLocaleId = i.Key.PurLocaleId,
                AP = i.Key.AP,
                LocaleId = i.Key.LocaleId,
                APType = 2,
                IsDiscount = 0,
            })
            .ToList();

            // step2: 取前期產生的應付帳款(含ForOther) APYM <> YYYYMM,
            var preItems = (
                from m in APMonth.Get()
                join mi in APMonthItem.Get() on new { Id = m.Id, LocaleId = m.LocaleId } equals new { Id = mi.APMonthId, LocaleId = mi.LocaleId }
                join o in Orders.Get() on new { OrderNo = mi.WONo } equals new { OrderNo = o.OrderNo } into oGRP
                from o in oGRP.DefaultIfEmpty()
                select new
                {
                    Vendor = m.VendorNameTw,
                    WarehouseNo = mi.WarehouseNo,
                    // CloseMonth = m.YYYYMM,
                    CloseMonth = Convert.ToInt32(m.YYYYMM),       // for condition, cause column type is string , query by linq is too slow, covert to interger 
                    CloseMonth1 = m.YYYYMM,                        // for select column
                    Dollar = m.DollarCodeName,
                    PreAPTTL = m.PreAPTTL,
                    APTTL = mi.APTTL,
                    Discount = mi.Discount,
                    APGet = mi.APGet,

                    APMonthId = m.Id,
                    APYM = mi.APYM,
                    CompanyId = o.CompanyId,
                    Company = o.CompanyNo,
                    PurLocaleId = mi.PurLocaleId,
                    PaymentLocaleId = m.PaymentLocaleId,
                    LocaleId = m.LocaleId,
                    AP = m.AP,
                    Purchaser = (string?)mi.PurUserName
                }
            )
            .Where(i => i.CloseMonth1 != i.APYM)
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .GroupBy(i => new { i.PreAPTTL, i.WarehouseNo, i.APMonthId, i.AP, i.Company, i.CloseMonth1, i.Vendor, i.Dollar, i.CompanyId, i.PaymentLocaleId, i.PurLocaleId, i.LocaleId })
            .Select(i => new Models.Views.PayableForPay
            {
                Vendor = i.Key.Vendor,
                WarehouseNo = i.Key.WarehouseNo,
                CloseMonth = i.Key.CloseMonth1,
                PayDollar = i.Key.Dollar,
                PreAPTTL = i.Sum(g => g.APTTL),
                APTTL = 0,
                APGetPre = i.Sum(g => g.APGet + g.Discount), // PlanPay
                Discount = i.Sum(g => g.Discount),
                APGet = i.Sum(g => g.APGet), // Pay

                APMonthId = i.Key.APMonthId,
                CompanyId = i.Key.CompanyId,
                // Company = i.Key.Company,
                PaymentLocaleId = i.Key.PaymentLocaleId,
                PurLocaleId = i.Key.PurLocaleId,
                AP = i.Key.AP,
                LocaleId = i.Key.LocaleId,
                APType = 1,
                IsDiscount = 0,
            })
            .ToList();

            var preOtherItems = (
                from m in APMonthOther.Get()
                join mi in APMonthOtherItem.Get() on new { Id = m.Id, LocaleId = m.LocaleId } equals new { Id = mi.APMonthOtherId, LocaleId = mi.LocaleId }
                join o in Orders.Get() on new { OrderNo = mi.WONo } equals new { OrderNo = o.OrderNo } into oGRP
                from o in oGRP.DefaultIfEmpty()
                select new
                {
                    Vendor = m.VendorNameTw,
                    WarehouseNo = mi.WarehouseNo,
                    // CloseMonth = m.YYYYMM,
                    CloseMonth = Convert.ToInt32(m.YYYYMM),       // for condition, cause column type is string , query by linq is too slow, covert to interger 
                    CloseMonth1 = m.YYYYMM,                        // for select column
                    Dollar = m.DollarCodeName,
                    PreAPTTL = m.PreAPTTL,
                    APTTL = mi.APTTL,
                    Discount = mi.Discount,
                    APGet = mi.APGet,

                    APMonthId = m.Id,
                    APYM = mi.APYM,
                    CompanyId = o.CompanyId,
                    Company = o.CompanyNo,
                    PurLocaleId = mi.PurLocaleId,
                    PaymentLocaleId = m.PaymentLocaleId,
                    LocaleId = m.LocaleId,
                    AP = m.AP,
                    Purchaser = (string?)mi.PurUserName
                }
            )
            .Where(i => i.CloseMonth1 != i.APYM)
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .GroupBy(i => new { i.PreAPTTL, i.WarehouseNo, i.APMonthId, i.AP, i.Company, i.CloseMonth1, i.Vendor, i.Dollar, i.CompanyId, i.PaymentLocaleId, i.PurLocaleId, i.LocaleId })
            .Select(i => new Models.Views.PayableForPay
            {
                Vendor = i.Key.Vendor,
                WarehouseNo = i.Key.WarehouseNo,
                CloseMonth = i.Key.CloseMonth1,
                PayDollar = i.Key.Dollar,
                PreAPTTL = i.Sum(g => g.APTTL),
                APTTL = 0,
                APGetPre = i.Sum(g => g.APGet + g.Discount), // PlanPay
                Discount = i.Sum(g => g.Discount),
                APGet = i.Sum(g => g.APGet), // Pay

                APMonthId = i.Key.APMonthId,
                CompanyId = i.Key.CompanyId,
                // Company = i.Key.Company,
                PaymentLocaleId = i.Key.PaymentLocaleId,
                PurLocaleId = i.Key.PurLocaleId,
                AP = i.Key.AP,
                LocaleId = i.Key.LocaleId,
                APType = 2,
                IsDiscount = 0,
            })
            .ToList();


            // step3: 取折扣的應付帳款(含ForOther),
            var discountItems = (
                from m in APMonth.Get()
                join mi in APMonthItemDiscount.Get() on new { Id = m.Id, LocaleId = m.LocaleId } equals new { Id = mi.APMonthId, LocaleId = mi.LocaleId }
                join o in Orders.Get() on new { OrderNo = mi.WONo } equals new { OrderNo = o.OrderNo } into oGRP
                from o in oGRP.DefaultIfEmpty()
                select new
                {
                    Vendor = m.VendorNameTw,
                    WarehouseNo = mi.WarehouseNo,
                    // CloseMonth = m.YYYYMM,
                    CloseMonth = Convert.ToInt32(m.YYYYMM),       // for condition, cause column type is string , query by linq is too slow, covert to interger 
                    CloseMonth1 = m.YYYYMM,                        // for select column
                    Dollar = m.DollarCodeName,
                    PreAPTTL = m.PreAPTTL,
                    APTTL = mi.APTTL,
                    Discount = mi.Discount,
                    APGet = mi.APGet,

                    APMonthId = m.Id,
                    APYM = mi.APYM,
                    CompanyId = o.CompanyId,
                    Company = o.CompanyNo,
                    PurLocaleId = mi.PurLocaleId,
                    PaymentLocaleId = m.PaymentLocaleId,
                    LocaleId = m.LocaleId,
                    AP = m.AP,
                    Purchaser = (string?)mi.PurUserName
                }
            )
            // .Where(i => i.CloseMonth1 != i.APYM)
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .GroupBy(i => new { i.WarehouseNo, i.APMonthId, i.AP, i.CloseMonth1, i.Vendor, i.Dollar, i.CompanyId, i.PaymentLocaleId, i.PurLocaleId, i.LocaleId })
            .Select(i => new Models.Views.PayableForPay
            {
                Vendor = i.Key.Vendor,
                WarehouseNo = i.Key.WarehouseNo,
                CloseMonth = i.Key.CloseMonth1,
                PayDollar = i.Key.Dollar,
                PreAPTTL = 0,
                APTTL = i.Sum(g => g.Discount),
                APGetPre = i.Sum(g => g.Discount), // PlanPay
                Discount = 0,
                APGet = i.Sum(g => g.Discount), // Pay

                APMonthId = i.Key.APMonthId,
                CompanyId = i.Key.CompanyId,
                // Company = i.Key.Company,
                PaymentLocaleId = i.Key.PaymentLocaleId,
                PurLocaleId = i.Key.PurLocaleId,
                AP = i.Key.AP,
                LocaleId = i.Key.LocaleId,
                APType = 1,
                IsDiscount = 1,
            })
            .ToList();

            var discountOtherItems = (
               from m in APMonthOther.Get()
               join mi in APMonthOtherItemDiscount.Get() on new { Id = m.Id, LocaleId = m.LocaleId } equals new { Id = mi.APMonthOtherId, LocaleId = mi.LocaleId }
               join o in Orders.Get() on new { OrderNo = mi.WONo } equals new { OrderNo = o.OrderNo } into oGRP
               from o in oGRP.DefaultIfEmpty()
               select new
               {
                   Vendor = m.VendorNameTw,
                   WarehouseNo = mi.WarehouseNo,
                   // CloseMonth = m.YYYYMM,
                   CloseMonth = Convert.ToInt32(m.YYYYMM),       // for condition, cause column type is string , query by linq is too slow, covert to interger 
                   CloseMonth1 = m.YYYYMM,                        // for select column
                   Dollar = m.DollarCodeName,
                   PreAPTTL = m.PreAPTTL,
                   APTTL = mi.APTTL,
                   Discount = mi.Discount,
                   APGet = mi.APGet,
                   APMonthId = m.Id,
                   APYM = mi.APYM,
                   PurLocaleId = mi.PurLocaleId,
                   PaymentLocaleId = m.PaymentLocaleId,
                   LocaleId = m.LocaleId,
                   AP = m.AP,
                   Purchaser = (string?)mi.PurUserName,

                   CompanyId = (decimal?)o.CompanyId,
                   Company = (string?)o.CompanyNo,
               }
            )
            // .Where(i => i.CloseMonth1 != i.APYM)
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .GroupBy(i => new { i.WarehouseNo, i.APMonthId, i.AP, i.CloseMonth1, i.Vendor, i.Dollar, i.CompanyId, i.PaymentLocaleId, i.PurLocaleId, i.LocaleId })
            .Select(i => new Models.Views.PayableForPay
            {
                Vendor = i.Key.Vendor,
                WarehouseNo = i.Key.WarehouseNo,
                CloseMonth = i.Key.CloseMonth1,
                PayDollar = i.Key.Dollar,
                PreAPTTL = 0,
                APTTL = i.Sum(g => g.Discount),
                APGetPre = i.Sum(g => g.Discount), // PlanPay
                Discount = 0,
                APGet = i.Sum(g => g.Discount), // Pay

                APMonthId = i.Key.APMonthId,
                CompanyId = i.Key.CompanyId,
                // Company = i.Key.Company,
                PaymentLocaleId = i.Key.PaymentLocaleId,
                PurLocaleId = i.Key.PurLocaleId,
                AP = i.Key.AP,
                LocaleId = i.Key.LocaleId,
                APType = 2,
                IsDiscount = 1,
            })
            .ToList();

            var result = items.Union(preItems).Union(discountItems).Union(otherItems).Union(preOtherItems).Union(discountOtherItems).ToList();
            result.ForEach(i =>
            {
                Int32.TryParse(i.WarehouseNo, out int wId); // because WarehouseNo is string, convert to int
                i.CompanyId = wId != 0 ? wId : i.CompanyId != null ? i.CompanyId : i.PurLocaleId;
                // i.Company = company.Where(c => c.Id == i.CompanyId).Max(c => c.CompanyNo);
                i.CloseMonthFrom = result.Min(g => g.CloseMonth);
                i.CloseMonthTo = result.Max(g => g.CloseMonth);
                i.PayRate = i.IsDiscount == 1 ? 1 : i.APTTL == 0 ? 0 : (decimal)(i.APGetPre / i.APTTL);
            });

            // 因為公司別的資料記錄問題，需要再group, 如以後更正前端資料輸入，可以省略Group by
            var group = result
                // .GroupBy(i => new { i.IsDiscount, i.APMonthId, i.AP, i.CloseMonth, i.CloseMonthFrom, i.CloseMonthTo, i.Vendor, i.PayDollar, i.CompanyId, i.PaymentLocaleId, i.PurLocaleId, i.LocaleId })
                .GroupBy(i => new { i.APMonthId, i.AP, i.CloseMonth, i.CloseMonthFrom, i.CloseMonthTo, i.Vendor, i.PayDollar, i.CompanyId, i.PaymentLocaleId, i.PurLocaleId, i.LocaleId })
                .Select(i => new Models.Views.PayableForPay
                {
                    APMonthId = i.Key.APMonthId,
                    CloseMonth = i.Key.CloseMonth,
                    CloseMonthFrom = i.Key.CloseMonthFrom,
                    CloseMonthTo = i.Key.CloseMonthTo,
                    Vendor = i.Key.Vendor,
                    PayDollar = i.Key.PayDollar,
                    CompanyId = i.Key.CompanyId,
                    // Company = i.Key.Company,
                    PaymentLocaleId = i.Key.PaymentLocaleId,
                    PurLocaleId = i.Key.PurLocaleId,
                    AP = i.Key.AP,
                    LocaleId = i.Key.LocaleId,

                    // PreAPTTL = i.Key.PreAPTTL,
                    PreAPTTL = i.Sum(g => g.PreAPTTL),
                    APTTL = i.Sum(g => g.APTTL),
                    APGetPre = i.Sum(g => g.APGetPre),
                    APGet = i.Sum(g => g.APGet),
                    Discount = i.Sum(g => g.Discount),
                    PayRate = i.Sum(g => g.APTTL) == 0 ? 0 : (decimal)(i.Sum(g => g.APGetPre) / i.Sum(g => g.APTTL))
                    // PayRate = i.Key.IsDiscount == 1 ? 1 : i.Sum(g => g.APTTL) == 0 ? 0 : (decimal)(i.Sum(g => g.APGetPre) / i.Sum(g => g.APTTL))
                })
                .AsQueryable();
            return group;
        }
        public IQueryable<Models.Views.PayableItemForPay> GetMonthAPItemForPay(string predicate)
        {
            // var company = Company.Get().ToList();

            // step1: 取得所有的APItem, otherItem, DiscountItem，再做處理
            var items = (
                from mi in APMonthItem.Get()
                join m in APMonth.Get() on new { APMonthId = mi.APMonthId, LocaleId = mi.LocaleId } equals new { APMonthId = m.Id, LocaleId = m.LocaleId }
                join pi in POItem.Get() on new { POItemId = (decimal)mi.POItemId, PurLocaleId = (decimal)mi.PurLocaleId } equals new { POItemId = pi.Id, PurLocaleId = pi.LocaleId } into piGRP
                from pi in piGRP.DefaultIfEmpty()
                join p in PO.Get() on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = p.Id, LocaleId = p.LocaleId } into pGRP
                from p in pGRP.DefaultIfEmpty()
                join o in Orders.Get() on new { OrderNo = mi.WONo } equals new { OrderNo = o.OrderNo } into oGRP
                from o in oGRP.DefaultIfEmpty()
                join tmi in APMonthItem.Get().Where(i => i.IsGet == 1 || (i.IsGet == 0 && i.APGet != 0))
                            .GroupBy(i => new { i.VendorMaterialNo, i.WONo, i.LocaleId })
                            .Select(i => new { PONo = i.Key.VendorMaterialNo, OrderNo = i.Key.WONo, LocaleId = i.Key.LocaleId, PayQtyTTL = i.Sum(g => g.PayQty) })
                            on new { PONo = mi.VendorMaterialNo, OrderNo = mi.WONo, LocaleId = mi.LocaleId } equals new { PONo = tmi.PONo, OrderNo = tmi.OrderNo, LocaleId = tmi.LocaleId } into tGRP
                from tmi in tGRP.DefaultIfEmpty()
                select new
                {
                    ReceivedDate = mi.ReceiveDate,
                    PONo = mi.VendorMaterialNo,
                    Material = mi.TypeNo,
                    PurUnit = mi.PUnit,
                    PlanQty = mi.Spec,
                    PurRate = 0,
                    PayQty = mi.PayQty,
                    UnitPrice = mi.UnitPrice,
                    APAmount = mi.APAmount,
                    APTax = mi.APTax,
                    PayDollar = m.DollarCodeName,
                    Discount = mi.Discount,
                    PlanPayTTL = mi.APGet,
                    SubPurRate = 0,
                    PurLocaleId = mi.PurLocaleId,
                    OrderNo = mi.WONo,
                    Id = mi.Id,
                    APMonthId = m.Id,
                    APYM = mi.APYM,
                    Vendor = m.VendorNameTw,
                    APTTL = mi.APTTL,
                    APGet = mi.APGet,
                    WarehouseNo = mi.WarehouseNo,
                    PaymentLocaleId = m.PaymentLocaleId,
                    ReceivedLocaleId = mi.ReceivedLocaleId,
                    POItem = mi.POItemId,
                    LocaleId = m.LocaleId,
                    AP = m.AP,
                    PurLocale = mi.PurLocaleId,
                    IsGet = mi.IsGet,
                    TaxRate = mi.TaxRate,
                    // CloseMonth = m.YYYYMM,
                    CloseMonth = Convert.ToInt32(m.YYYYMM),       // for condition, cause column type is string , query by linq is too slow, covert to interger 
                    CloseMonth1 = m.YYYYMM,                        // for select column

                    Purchaser = (string?)mi.PurUserName,
                    StyleNo = (string?)o.StyleNo,
                    CompanyId = (decimal?)o.CompanyId,
                    Company = (string?)o.CompanyNo,
                    LCSD = (DateTime?)o.LCSD,
                    PODate = (DateTime?)p.PODate,
                    PayQtyTTL = (decimal?)tmi.PayQtyTTL,
                }
            )
            .Where(i => i.IsGet == 1 || (i.IsGet == 0 && i.APGet != 0))
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new Models.Views.PayableItemForPay
            {
                ReceivedDate = i.ReceivedDate,
                PONo = i.PONo,
                Material = i.Material,
                PurUnit = i.PurUnit,
                PlanQty = Convert.ToDecimal(i.PlanQty),
                PayQtyTTL = i.PayQtyTTL ?? 0,
                PurRate = i.PurRate,
                PayQty = i.PayQty,
                UnitPrice = i.UnitPrice,
                APAmount = i.APAmount,
                APTax = i.APTax,
                PayDollar = i.PayDollar,
                Discount = i.Discount,
                APGet = i.APGet,
                PlanPayTTL = i.PlanPayTTL,
                SubPurRate = 0,
                PurLocaleId = i.PurLocaleId,
                OrderNo = i.OrderNo,
                LCSD = i.LCSD,

                Id = i.Id,
                APMonthId = i.APMonthId,
                CloseMonth = i.CloseMonth1,
                APYM = i.APYM,
                Vendor = i.Vendor,
                APTTL = i.APTTL,
                WarehouseNo = i.WarehouseNo,
                CompanyId = i.CompanyId,
                Company = i.Company,
                PaymentLocaleId = i.PaymentLocaleId,
                LocaleId = i.LocaleId,
                AP = i.AP,
                IsGet = i.IsGet,
                TaxRate = i.TaxRate,
                StyleNo = i.StyleNo,
                PODate = i.PODate,
                Purchaser = i.Purchaser ?? "",
            })
            .Distinct()
            .ToList();

            // step2: discount
            var discountItems = (
                from mi in APMonthItemDiscount.Get()
                join m in APMonth.Get() on new { APMonthId = mi.APMonthId, LocaleId = mi.LocaleId } equals new { APMonthId = m.Id, LocaleId = m.LocaleId }
                join o in Orders.Get() on new { OrderNo = mi.WONo } equals new { OrderNo = o.OrderNo } into oGRP
                from o in oGRP.DefaultIfEmpty()
                select new
                {
                    Material = mi.TypeNo,
                    PayQtyTTL = 0,
                    PurRate = 0,
                    PayQty = 0,
                    UnitPrice = 0,
                    APAmount = 0,
                    APTax = 0,
                    PayDollar = m.DollarCodeName,
                    Discount = 0,
                    PlanPayTTL = mi.Discount,
                    SubPurRate = 1,
                    PurLocaleId = mi.PurLocaleId,
                    OrderNo = mi.WONo,
                    PaymentLocaleId = m.PaymentLocaleId,
                    LocaleId = m.LocaleId,
                    AP = m.AP,
                    PurLocale = mi.PurLocaleId,
                    IsGet = mi.IsGet,
                    TaxRate = mi.TaxRate,
                    CloseMonth = Convert.ToInt32(m.YYYYMM),       // for condition, cause column type is string , query by linq is too slow, covert to interger 
                    CloseMonth1 = m.YYYYMM,                        // for select column
                    Purchaser = (string?)mi.PurUserName,
                    Id = mi.Id,
                    APMonthId = m.Id,
                    APYM = mi.APYM,
                    Vendor = m.VendorNameTw,
                    APTTL = mi.APTTL,
                    APGet = mi.APGet,
                    WarehouseNo = mi.WarehouseNo,

                    ReceivedDate = mi.ReceiveDate,
                    PONo = (string?)mi.VendorMaterialNo,
                    PurUnit = (string?)mi.PUnit,
                    PlanQty = (string?)mi.Spec,

                    LCSD = (DateTime?)o.LCSD,
                    OrderDate = (DateTime?)o.OrderDate,
                    CompanyId = (decimal?)o.CompanyId,
                    Company = (string?)o.CompanyNo,
                    StyleNo = (string?)o.StyleNo,
                }
            )
            .Where(i => i.IsGet == 1 || (i.IsGet == 0 && i.APGet != 0))
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new Models.Views.PayableItemForPay
            {

                Material = i.Material,

                PayQtyTTL = i.PayQtyTTL,
                PurRate = i.PurRate,
                PayQty = i.PayQty,
                UnitPrice = i.UnitPrice,
                APAmount = i.APAmount,
                APTax = i.APTax,
                PayDollar = i.PayDollar,
                Discount = i.Discount,
                APGet = i.APGet,
                PlanPayTTL = i.PlanPayTTL,
                SubPurRate = 0,
                PurLocaleId = i.PurLocaleId,
                OrderNo = i.OrderNo,
                Id = i.Id,
                APMonthId = i.APMonthId,
                CloseMonth = i.CloseMonth1,
                APYM = i.APYM,
                Vendor = i.Vendor,
                APTTL = i.APTTL,
                WarehouseNo = i.WarehouseNo,
                PaymentLocaleId = i.PaymentLocaleId,
                LocaleId = i.LocaleId,
                AP = i.AP,
                IsGet = (int)i.IsGet,
                TaxRate = i.TaxRate,
                Purchaser = i.Purchaser ?? "",

                ReceivedDate = i.ReceivedDate,
                PONo = i.PONo ?? "",
                PurUnit = i.PurUnit ?? "",
                PlanQty = Convert.ToDecimal(i.PlanQty ?? "0"),

                LCSD = i.LCSD,
                OrderDate = i.OrderDate,
                CompanyId = i.CompanyId ?? 0,
                Company = i.Company ?? "",
                StyleNo = i.StyleNo ?? "",
            })
            .Distinct()
            .ToList();

            var otherItems = (
                from mi in APMonthOtherItem.Get()
                join m in APMonthOther.Get() on new { APMonthId = mi.APMonthOtherId, LocaleId = mi.LocaleId } equals new { APMonthId = m.Id, LocaleId = m.LocaleId }
                join pi in POItem.Get() on new { POItemId = (decimal)mi.POItemId, PurLocaleId = (decimal)mi.PurLocaleId } equals new { POItemId = pi.Id, PurLocaleId = pi.LocaleId } into piGRP
                from pi in piGRP.DefaultIfEmpty()
                join p in PO.Get() on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = p.Id, LocaleId = p.LocaleId } into pGRP
                from p in pGRP.DefaultIfEmpty()
                join o in Orders.Get() on new { OrderNo = mi.WONo } equals new { OrderNo = o.OrderNo } into oGRP
                from o in oGRP.DefaultIfEmpty()
                join tmi in APMonthOtherItem.Get().Where(i => i.IsGet == 1 || (i.IsGet == 0 && i.APGet != 0))
                            .GroupBy(i => new { i.VendorMaterialNo, i.WONo, i.LocaleId })
                            .Select(i => new { PONo = i.Key.VendorMaterialNo, OrderNo = i.Key.WONo, LocaleId = i.Key.LocaleId, PayQtyTTL = i.Sum(g => g.PayQty) })
                            on new { PONo = mi.VendorMaterialNo, OrderNo = mi.WONo, LocaleId = mi.LocaleId } equals new { PONo = tmi.PONo, OrderNo = tmi.OrderNo, LocaleId = tmi.LocaleId } into tGRP
                from tmi in tGRP.DefaultIfEmpty()
                select new
                {
                    ReceivedDate = mi.ReceiveDate,
                    PONo = mi.VendorMaterialNo,
                    Material = mi.TypeNo,
                    PurUnit = mi.PUnit,
                    PlanQty = mi.Spec,
                    PayQtyTTL = tmi.PayQtyTTL,
                    PurRate = 0,
                    PayQty = mi.PayQty,
                    UnitPrice = mi.UnitPrice,
                    APAmount = mi.APAmount,
                    APTax = mi.APTax,
                    PayDollar = m.DollarCodeName,
                    Discount = mi.Discount,
                    PlanPayTTL = mi.APGet,
                    SubPurRate = 0,
                    PurLocaleId = mi.PurLocaleId,
                    OrderNo = mi.WONo,
                    Id = mi.Id,
                    APMonthId = m.Id,
                    APYM = mi.APYM,
                    Vendor = m.VendorNameTw,
                    APTTL = mi.APTTL,
                    APGet = mi.APGet,
                    WarehouseNo = mi.WarehouseNo,
                    PaymentLocaleId = m.PaymentLocaleId,
                    LocaleId = m.LocaleId,
                    AP = m.AP,
                    PurLocale = mi.PurLocaleId,
                    IsGet = mi.IsGet,
                    TaxRate = mi.TaxRate,
                    // CloseMonth = m.YYYYMM,
                    CloseMonth = Convert.ToInt32(m.YYYYMM),       // for condition, cause column type is string , query by linq is too slow, covert to interger 
                    CloseMonth1 = m.YYYYMM,                        // for select column
                    Purchaser = (string?)mi.PurUserName,

                    StyleNo = (string?)o.StyleNo,
                    PODate = (DateTime?)p.PODate,
                    LCSD = (DateTime?)o.LCSD,
                    CompanyId = (decimal?)o.CompanyId,
                    Company = (string?)o.CompanyNo,
                }
            )
            .Where(i => i.IsGet == 1 || (i.IsGet == 0 && i.APGet != 0))
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new Models.Views.PayableItemForPay
            {
                ReceivedDate = i.ReceivedDate,
                PONo = i.PONo,
                Material = i.Material,
                PurUnit = i.PurUnit,
                PlanQty = Convert.ToDecimal(i.PlanQty),
                PayQtyTTL = i.PayQtyTTL,
                PurRate = i.PurRate,
                PayQty = i.PayQty,
                UnitPrice = i.UnitPrice,
                APAmount = i.APAmount,
                APTax = i.APTax,
                PayDollar = i.PayDollar,
                Discount = i.Discount,
                APGet = i.APGet,
                PlanPayTTL = i.PlanPayTTL,
                SubPurRate = 0,
                PurLocaleId = i.PurLocaleId,
                OrderNo = i.OrderNo,

                Id = i.Id,
                APMonthId = i.APMonthId,
                CloseMonth = i.CloseMonth1,
                APYM = i.APYM,
                Vendor = i.Vendor,
                APTTL = i.APTTL,
                WarehouseNo = i.WarehouseNo,

                PaymentLocaleId = i.PaymentLocaleId,
                LocaleId = i.LocaleId,
                AP = i.AP,
                IsGet = i.IsGet,
                TaxRate = i.TaxRate,

                Purchaser = i.Purchaser ?? "",

                LCSD = i.LCSD,
                StyleNo = i.StyleNo ?? "",
                PODate = i.PODate,
                CompanyId = i.CompanyId ?? 0,
                Company = i.Company ?? "",
            })
            .Distinct()
            .ToList();

            var discountOtherItems = (
                from mi in APMonthOtherItemDiscount.Get()
                join m in APMonthOther.Get() on new { APMonthId = mi.APMonthOtherId, LocaleId = mi.LocaleId } equals new { APMonthId = m.Id, LocaleId = m.LocaleId }
                join o in Orders.Get() on new { OrderNo = mi.WONo } equals new { OrderNo = o.OrderNo } into oGRP
                from o in oGRP.DefaultIfEmpty()
                select new
                {
                    ReceivedDate = (DateTime?)mi.ReceiveDate,
                    PONo = (string?)mi.VendorMaterialNo,
                    Material = mi.TypeNo,
                    PurUnit = (string?)mi.PUnit,
                    PlanQty = (string?)mi.Spec,
                    PayDollar = m.DollarCodeName,
                    PlanPayTTL = mi.Discount,
                    PurLocaleId = mi.LocaleId,
                    OrderNo = (string?)mi.WONo,
                    Id = mi.Id,
                    APMonthId = m.Id,
                    APYM = mi.APYM,
                    Vendor = m.VendorNameTw,
                    APTTL = mi.APTTL,
                    APGet = mi.APGet,
                    WarehouseNo = mi.WarehouseNo,
                    PaymentLocaleId = m.PaymentLocaleId,
                    LocaleId = m.LocaleId,
                    AP = m.AP,
                    PurLocale = mi.PurLocaleId,
                    IsGet = mi.IsGet,
                    TaxRate = mi.TaxRate,
                    CloseMonth = Convert.ToInt32(m.YYYYMM),       // for condition, cause column type is string , query by linq is too slow, covert to interger 
                    CloseMonth1 = m.YYYYMM,                        // for select column
                    Purchaser = (string?)mi.PurUserName,

                    PayQtyTTL = 0,
                    PurRate = 0,
                    PayQty = 0,
                    UnitPrice = 0,
                    APAmount = 0,
                    APTax = 0,
                    Discount = 0,
                    SubPurRate = 1,

                    LCSD = (DateTime?)o.LCSD,
                    OrderDate = (DateTime?)o.OrderDate,
                    CompanyId = (decimal?)o.CompanyId,
                    Company = (string?)o.CompanyNo,
                    StyleNo = (string?)o.StyleNo,
                }
            )
            .Where(i => i.IsGet == 1 || (i.IsGet == 0 && i.APGet != 0))
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new Models.Views.PayableItemForPay
            {
                ReceivedDate = i.ReceivedDate,
                PONo = i.PONo ?? "",
                Material = i.Material,
                PurUnit = i.PurUnit ?? "",
                PlanQty = Convert.ToDecimal(i.PlanQty ?? "0"),
                PayDollar = i.PayDollar,
                APGet = i.APGet,
                PlanPayTTL = i.PlanPayTTL,
                SubPurRate = 0,
                PurLocaleId = i.PurLocaleId,
                OrderNo = i.OrderNo ?? "",
                Id = i.Id,
                APMonthId = i.APMonthId,
                CloseMonth = i.CloseMonth1,
                APYM = i.APYM,
                Vendor = i.Vendor,
                APTTL = i.APTTL,
                WarehouseNo = i.WarehouseNo,
                PaymentLocaleId = i.PaymentLocaleId,
                LocaleId = i.LocaleId,
                AP = i.AP,
                IsGet = (int)i.IsGet,
                TaxRate = i.TaxRate,
                // PODate = i.PODate
                Purchaser = i.Purchaser ?? "",
                Discount = i.Discount,
                PayQtyTTL = i.PayQtyTTL,
                PurRate = i.PurRate,
                PayQty = i.PayQty,
                UnitPrice = i.UnitPrice,
                APAmount = i.APAmount,
                APTax = i.APTax,

                LCSD = i.LCSD,
                OrderDate = i.OrderDate,
                CompanyId = i.CompanyId ?? 0,
                Company = i.Company ?? "",
                StyleNo = i.StyleNo ?? "",
            })
            .Distinct()
            .ToList();

            var result = items.Union(otherItems).Union(discountItems).Union(discountOtherItems).ToList();
            result.ForEach(i =>
            {
                Int32.TryParse(i.WarehouseNo, out int wId); // because WarehouseNo is string, convert to int
                i.CompanyId = wId != 0 ? wId : i.CompanyId != null ? i.CompanyId : i.PurLocaleId;
                // i.Company = company.Where(c => c.Id == i.CompanyId).Max(c => c.CompanyNo);
                // i.PaymentLocale = company.Where(c => c.Id == i.PaymentLocaleId).Max(c => c.CompanyNo);
                // i.PurLocale = company.Where(c => c.Id == i.PurLocaleId).Max(c => c.CompanyNo);
                i.PurRate = (i.PlanQty == 0 || i.PayQtyTTL == 0) ? 0 : i.PayQtyTTL / i.PlanQty;
                i.SubPurRate = i.APTTL == 0 ? 0 : (i.APGet + i.Discount) / i.APTTL;

            });
            return result.AsQueryable();
        }

    }
}