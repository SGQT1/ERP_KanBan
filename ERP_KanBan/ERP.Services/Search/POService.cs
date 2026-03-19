using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Models.Views.Common;
using ERP.Services.Bases;
using Newtonsoft.Json;
using NPOI.SS.Formula;

namespace ERP.Services.Search
{
    public class POService : SearchService
    {
        private ERP.Services.Entities.OrdersService Orders { get; set; }

        private ERP.Services.Entities.PurBatchService PurBatch { get; set; }
        private ERP.Services.Entities.PurOrdersItemService PurOrdersItem { get; set; }
        private ERP.Services.Entities.PurBatchItemService PurBatchItem { get; set; }

        private ERP.Services.Entities.POService PO { get; set; }
        private ERP.Services.Entities.POItemService POItem { get; set; }
        private ERP.Services.Entities.POItemSizeService POItemSize { get; set; }
        private ERP.Services.Entities.ReceivedLogService ReceivedLog { get; set; }
        private ERP.Services.Entities.ReceivedLogAddService ReceivedLogAdd { get; set; }

        private ERP.Services.Entities.MRPItemService MRPItem { get; set; }
        private ERP.Services.Entities.MRPItemOrdersService MRPItemOrders { get; set; }
        private ERP.Services.Entities.PCLService PCL { get; set; }

        private ERP.Services.Entities.MaterialService Material { get; set; }
        private ERP.Services.Entities.VendorService Vendor { get; set; }
        private ERP.Services.Entities.ArticleSizeRunService ArticleSizeRun { get; set; }
        private ERP.Services.Entities.CodeItemService CodeItem { get; set; }
        // private ERP.Services.Entities.CompanyService Company { get; set; }

        private ERP.Services.Business.Entities.ReceivedLogService ReceivedLogAll { get; set; }
        public POService(
            // ERP.Services.Business.Entities.TypeService typeService,
            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Entities.POService poService,
            ERP.Services.Entities.POItemService poItemService,
            ERP.Services.Entities.POItemPrintLogService pOItemPrintLogService,
            ERP.Services.Entities.POItemSizeService poItemSizeService,
            ERP.Services.Entities.PurBatchService purBatchService,
            ERP.Services.Entities.PurOrdersItemService purOrdersItemService,
            ERP.Services.Entities.PurBatchItemService purBatchItemService,
            ERP.Services.Entities.ReceivedLogService receivedLogService,
            ERP.Services.Entities.ReceivedLogAddService receivedLogAddService,

            ERP.Services.Entities.MRPItemService mrpItemService,
            ERP.Services.Entities.MRPItemOrdersService mrpItemOrdersService,
            ERP.Services.Entities.PCLService pclService,

            ERP.Services.Entities.MaterialService materialService,
            ERP.Services.Entities.VendorService vendorService,
            ERP.Services.Entities.ArticleSizeRunService articleSizeRunService,
            ERP.Services.Entities.CodeItemService codeItemService,
            // ERP.Services.Entities.CompanyService companyService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Orders = ordersService;
            PurBatch = purBatchService;
            PurOrdersItem = purOrdersItemService;

            PO = poService;
            POItem = poItemService;
            POItemSize = poItemSizeService;
            PurBatchItem = purBatchItemService;
            ReceivedLog = receivedLogService;
            ReceivedLogAdd = receivedLogAddService;

            MRPItem = mrpItemService;
            MRPItemOrders = mrpItemOrdersService;
            PCL = pclService;

            Material = materialService;
            Vendor = vendorService;
            CodeItem = codeItemService;
            ArticleSizeRun = articleSizeRunService;
            // Company = companyService;
        }
        // 採購下單狀態查詢 2
        // step1: 找出所有的訂單，條件為管制表、採購批號、採購日期
        // step2: 透過OrderId找出所有MRPItem, MRPItemOrders的材料
        // Step3: 透過MRPItem找到該訂單所有計畫、採購單
        // Step4: 透過MRPItem找到所有材料最近一次的採購單
        // Step5:組合
        public IQueryable<Models.Views.PurchaseStatus> GetPOProcess(string predicate, string[] filters)
        {
            var localeId = 0;
            bool withoutPO = false;

            List<Models.Views.PurchaseStatus> poProcess = new List<Models.Views.PurchaseStatus> { };
            List<decimal> orderIds = new List<decimal> { };
            List<decimal> batchIds = new List<decimal> { };

            // step1: 找出所有的訂單，條件為管制表、採購批號、採購日期 ===========
            var extendPredicate = "";
            if (filters != null && filters.Length > 0)
            {
                //condition
                var tmpFilters = filters.Where(i => i.Contains("LocaleId") || i.Contains("BatchDate") || i.Contains("PurLocaleId") || i.Contains("Brand") || i.Contains("OrderNo") ||
                                                    i.Contains("BatchNo") || i.Contains("StyleNo")).ToArray();
                extendPredicate = tmpFilters.Length > 0 ? String.Join(" && ", tmpFilters) : "1=1";

                var hasPOFilters = filters.Where(i => i.Contains("Field9")).ToArray();
                if (hasPOFilters.Any())
                {
                    withoutPO = (JsonConvert.DeserializeObject<ExtentionItem>(hasPOFilters[0])).Field9;
                }
            }

            var orders = (
                from o in Orders.Get()
                join p in (
                    from pb in PurBatch.Get()
                    join po in PurOrdersItem.Get() on new { PurBatchId = pb.Id, LocaleId = pb.LocaleId } equals new { PurBatchId = po.PurBatchId, LocaleId = po.LocaleId }
                    select new
                    {
                        pb.Id,
                        pb.BatchDate,
                        pb.BatchNo,
                        pb.LocaleId,
                        pb.RefLocaleId,
                        po.OrdersId,
                    }
                ) on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = p.OrdersId, LocaleId = p.LocaleId } into pGRP
                from p in pGRP.DefaultIfEmpty()
                select new
                {
                    OrdersId = o.Id,
                    OrderNo = o.OrderNo,
                    StyleNo = o.StyleNo,
                    LocaleId = o.LocaleId,
                    PurLocaleId = o.LocaleId,
                    BatchId = p.Id,
                    BatchNo = p.BatchNo,
                    BatchDate = p.BatchDate,
                })
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, extendPredicate)
            .Distinct()
            .ToList();

            if (orders.Count() == 0)
            {
                return poProcess.AsQueryable();
            }
            localeId = (int)orders[0].LocaleId;
            orderIds = orders.Select(i => i.OrdersId).Distinct().ToList();
            batchIds = orders.Select(i => i.BatchId).Distinct().ToList();

            // step2: 透過OrderId找出所有MRPItem, MRPItemOrders的材料
            var mrpItem = (
                // from o in Orders.Get() 
                // join mrp in MRPItem.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId } 
                from o in Orders.Get()
                join mrp in MRPItem.Get().GroupBy(i => new { i.MaterialId, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw }).Select(i => new { i.Key.MaterialId, i.Key.OrdersId, i.Key.LocaleId, i.Key.ParentMaterialId, i.Key.SizeDivision, i.Key.UnitCodeId, i.Key.UnitNameTw, Usage = i.Sum(g => g.Total) }) on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId }
                join m in Material.Get() on new { MaterialId = mrp.MaterialId, LocaleId = mrp.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGrp
                from m in mGrp.DefaultIfEmpty()
                select new
                {
                    MaterialId = m.Id,
                    LocaleId = m.LocaleId,
                    MaterialNameTw = m.MaterialName,
                    MaterialNameEn = m.MaterialNameEng,
                    SamplingMethod = m.SamplingMethod,
                    CategoryCodeId = m.CategoryCodeId,
                    SemiGoods = m.SemiGoods,
                    TextureCodeId = m.TextureCodeId,
                    OrderNo = o.OrderNo,
                    OrdersId = o.Id,
                    ParentMaterialId = mrp.ParentMaterialId,
                    Total = mrp.Usage,
                    // POItemId = lastPOItems.Where(p => p.Status != 2 && p.MaterialId == mrp.MaterialId && p.LocaleId == mrp.LocaleId).OrderByDescending(p => p.PODate).ThenByDescending(p => p.Id).Select(i => i.Id).FirstOrDefault(),
                    AlternateType = mrp.SizeDivision,
                    UnitCodeId = mrp.UnitCodeId,
                    UnitNameTw = mrp.UnitNameTw,
                    StyleNo = o.StyleNo,
                }
            )
            .Where(i => i.LocaleId == localeId && orderIds.Contains(i.OrdersId))
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Distinct()
            .ToList();

            var mrpItemOrders = (
                // from o in Orders.Get() 
                // join mrp in MRPItemOrders.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId } 
                from o in Orders.Get()
                join mrp in MRPItemOrders.Get().GroupBy(i => new { i.MaterialId, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw }).Select(i => new { i.Key.MaterialId, i.Key.OrdersId, i.Key.LocaleId, i.Key.ParentMaterialId, i.Key.SizeDivision, i.Key.UnitCodeId, i.Key.UnitNameTw, Usage = i.Sum(g => g.Total) }) on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId }
                join m in Material.Get() on new { MaterialId = mrp.MaterialId, LocaleId = mrp.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGrp
                from m in mGrp.DefaultIfEmpty()
                select new
                {
                    MaterialId = m.Id,
                    LocaleId = m.LocaleId,
                    MaterialNameTw = m.MaterialName,
                    MaterialNameEn = m.MaterialNameEng,
                    SamplingMethod = m.SamplingMethod,
                    CategoryCodeId = m.CategoryCodeId,
                    SemiGoods = m.SemiGoods,
                    TextureCodeId = m.TextureCodeId,
                    OrderNo = o.OrderNo,
                    OrdersId = o.Id,
                    ParentMaterialId = mrp.ParentMaterialId,
                    Total = mrp.Usage,
                    // POItemId = lastPOItems.Where(p => p.Status != 2 && p.MaterialId == mrp.MaterialId && p.LocaleId == mrp.LocaleId).OrderByDescending(p => p.PODate).ThenByDescending(p => p.Id).Select(i => i.Id).FirstOrDefault(),
                    AlternateType = mrp.SizeDivision,
                    UnitCodeId = mrp.UnitCodeId,
                    UnitNameTw = mrp.UnitNameTw,
                    StyleNo = o.StyleNo,
                }
            )
            .Where(i => i.LocaleId == localeId && orderIds.Contains(i.OrdersId))
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Distinct()
            .ToList();

            var items = mrpItem.Union(mrpItemOrders);
            var bomItems = items
                .GroupBy(i => new { i.LocaleId, i.OrdersId, i.OrderNo, i.MaterialId, i.MaterialNameTw, i.MaterialNameEn, i.ParentMaterialId, i.UnitCodeId, i.CategoryCodeId, i.AlternateType, i.StyleNo })
                .Select(i => new
                {
                    LocaleId = i.Key.LocaleId,
                    OrdersId = i.Key.OrdersId,
                    OrderNo = i.Key.OrderNo,
                    MaterialId = i.Key.MaterialId,
                    MaterialNameTw = i.Key.MaterialNameTw,
                    MaterialNameEn = i.Key.MaterialNameEn,
                    ParentMaterialId = i.Key.ParentMaterialId,
                    UnitCodeId = i.Key.UnitCodeId,
                    CategoryCodeId = i.Key.CategoryCodeId,
                    SizeDivision = i.Key.AlternateType,
                    TotalUsage = i.Sum(g => g.Total),
                    // POItemId = i.Key.POItemId,
                    StyleNo = i.Key.StyleNo,
                })
                .ToList();

            // Step3: 透過MRPItem找到該訂單所有計畫、採購單
            var mIds = bomItems.Select(i => i.MaterialId).Distinct();
            var purBatchItems = PurBatchItem.Get().Where(i => i.Status != 2 && i.LocaleId == localeId && batchIds.Contains(i.BatchId)).ToList();
            var poItems = POItem.Get().Where(i => i.Status != 2 && i.LocaleId == localeId && orderIds.Contains(i.OrdersId) && mIds.Contains(i.MaterialId)).ToList();

            // Step4: 透過MRPItem找到所有材料最近一次的採購單
            var lPOItems = (
                from poi in POItem.Get()
                join po in PO.Get() on new { POId = poi.POId, LocaleId = poi.LocaleId } equals new { POId = po.Id, LocaleId = po.LocaleId }
                where poi.Status != 2 && poi.LocaleId == localeId && mIds.Contains(poi.MaterialId)
                group new { poi, po } by new { poi.MaterialId, poi.LocaleId } into g
                select g.OrderByDescending(x => x.po.PODate).ThenByDescending(x => x.poi.Id).Select(x => new Models.Views.POItem
                {
                    Id = x.poi.Id,
                    LocaleId = x.poi.LocaleId,
                    MaterialId = x.poi.MaterialId,
                    ParentMaterialId = x.poi.ParentMaterialId,
                    PODate = x.po.PODate,
                    VendorId = x.po.VendorId,
                    UnitCodeId = x.poi.UnitCodeId,
                    UnitPrice = x.poi.UnitPrice,
                    DollarCodeId = x.poi.DollarCodeId,
                })
                .FirstOrDefault()
            )
            .ToList();

            // Step5:組合
            poProcess = bomItems.Select(m => new Models.Views.PurchaseStatus
            {
                Id = m.MaterialId,
                LocaleId = m.LocaleId,
                OrdersId = m.OrdersId,
                OrderNo = m.OrderNo,
                StyleNo = m.StyleNo,
                MaterialId = m.MaterialId,
                ParentMaterialId = m.ParentMaterialId,
                MaterialNameTw = m.MaterialNameTw,
                MaterialNameEn = m.MaterialNameEn,
                PlanUnitCodeId = m.UnitCodeId,
                PlanQty = m.TotalUsage,
                AlternateType = m.SizeDivision,
                Seq = m.ParentMaterialId > 0 ? m.ParentMaterialId.ToString() + m.OrdersId.ToString() + m.MaterialId.ToString() :
                      m.MaterialId.ToString() + m.OrdersId.ToString(),
                CategoryCodeId = m.CategoryCodeId,

                PurQty = 0,
                POQty = 0,
                LastVendor = "",
                LastVendorId = 0,
                OnHandQty = m.ParentMaterialId > 0 ? 1 : 0,
                // POItemId = m.POItemId,

            })
            .ToList();

            poProcess.ForEach(i =>
            {
                i.PurQty = purBatchItems.Where(p => p.OrdersId == i.OrdersId && p.LocaleId == i.LocaleId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId).Sum(p => p.PurQty);
                i.POQty = poItems.Where(p => p.OrdersId == i.OrdersId && p.LocaleId == i.LocaleId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId).Sum(p => p.Qty);

                // var _poItem = lPOItems.Where(p => p.Id == i.POItemId && p.LocaleId == i.LocaleId).FirstOrDefault();
                var _poItem = lPOItems.Where(p => p.MaterialId == i.MaterialId && p.LocaleId == i.LocaleId).FirstOrDefault();
                if (_poItem != null)
                {
                    i.PurUnitPrice = _poItem.UnitPrice;
                    i.PurUnitCodeId = _poItem.UnitCodeId;
                    i.DollarCodeId = _poItem.DollarCodeId;
                    i.LastVendorId = _poItem.VendorId;
                }
            });

            if (withoutPO)
            {
                poProcess = poProcess.Where(i => i.POQty == 0).ToList();
            }

            return poProcess.AsQueryable();
        }
        //採購單查詢
        public IQueryable<Models.Views.POItemForDiff> GetPOItem(string predicate)
        {
            var items = (
                from poi in POItem.Get()
                join po in PO.Get() on new { POId = poi.POId, LocaleId = poi.LocaleId } equals new { POId = po.Id, LocaleId = po.LocaleId }
                join m in Material.Get() on new { MId = poi.MaterialId, LocaleId = poi.LocaleId } equals new { MId = m.Id, LocaleId = m.LocaleId }
                join pur in PurBatchItem.Get() on new { POItemId = poi.Id, LocaleId = poi.LocaleId } equals new { POItemId = (decimal)pur.POItemId, LocaleId = pur.LocaleId } into purGRP
                from pur in purGRP.DefaultIfEmpty()
                join o in Orders.Get() on new { OrdersId = poi.OrdersId, LocaleId = poi.LocaleId } equals new { OrdersId = o.Id, LocaleId = o.LocaleId } into oGRP
                from o in oGRP.DefaultIfEmpty()
                join v in Vendor.Get() on new { VendorId = po.VendorId, LocaleId = po.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId }
                select new Models.Views.POItemForDiff
                {
                    Id = poi.Id,
                    LocaleId = poi.LocaleId,
                    POId = poi.POId,
                    OrdersId = poi.OrdersId,
                    OrderNo = poi.OrderNo,
                    StyleNo = poi.StyleNo,
                    POType = poi.POType,
                    MaterialId = poi.MaterialId,
                    UnitPrice = poi.UnitPrice,
                    DollarCodeId = poi.DollarCodeId,
                    Qty = poi.Qty,
                    UnitCodeId = poi.UnitCodeId,
                    PayCodeId = poi.PayCodeId,
                    PurLocaleId = poi.PurLocaleId,
                    ReceivingLocaleId = poi.ReceivingLocaleId,
                    PaymentLocaleId = poi.PaymentLocaleId,
                    PaymentCodeId = poi.PaymentCodeId,
                    PaymentPoint = poi.PaymentPoint,
                    ParentMaterialId = poi.ParentMaterialId,
                    IsOverQty = poi.IsOverQty,
                    SamplingMethod = poi.SamplingMethod,
                    ModifyUserName = poi.ModifyUserName,
                    LastUpdateTime = poi.LastUpdateTime,
                    PayDollarCodeId = poi.PayDollarCodeId,
                    Status = poi.Status,
                    FactoryETD = poi.FactoryETD,
                    Remark = poi.Remark,
                    CompanyId = poi.CompanyId,
                    VendorId = po.VendorId,
                    VendorNameTw = v.NameTw,
                    IsAllowPartial = po.IsAllowPartial,
                    VendorETD = po.VendorETD,
                    POItemVendorETD = po.VendorETD,
                    // PONo = poi.PONo,
                    PONo = po.BatchNo + '-' + po.SeqId.ToString(),
                    IsShowSizeRun = po.IsShowSizeRun,
                    AlternateType = poi.AlternateType == null ? 0 : poi.AlternateType,
                    PurBatchItemId = poi.PurBatchItemId,
                    RefPONo = po.BatchNo + "-" + po.SeqId.ToString(),
                    Material = m.MaterialName,
                    MaterialEng = m.MaterialNameEng,
                    Currency = CodeItem.Get().Where(c => c.Id == poi.DollarCodeId && c.LocaleId == poi.LocaleId && c.CodeType == "02").Select(c => c.NameTW).FirstOrDefault(),
                    Unit = CodeItem.Get().Where(c => c.Id == poi.UnitCodeId && c.LocaleId == poi.LocaleId && c.CodeType == "21").Select(c => c.NameTW).FirstOrDefault(),
                    RefOrderNo = poi.OrderNo.Contains("-") ? poi.OrderNo.Substring(0, poi.OrderNo.IndexOf("-")) : poi.OrderNo,
                    ReceivedBarcode = poi.LocaleId.ToString() + "*" + poi.Id.ToString(),
                    PCLUnitCodeId = pur.PlanUnitCodeId,
                    PCLUnitNameTw = CodeItem.Get().Where(c => c.Id == pur.PlanUnitCodeId && c.LocaleId == pur.LocaleId && c.CodeType == "21").Select(c => c.NameTW).FirstOrDefault(),
                    PODate = po.PODate,
                    PlanQty = pur.PlanQty,
                    PurQty = pur.PurQty,
                    SeqId = po.SeqId,
                    BatchNo = po.BatchNo,
                    POTeam = po.POTeam,
                    ReceivedLogId = ReceivedLog.Get().Where(i => i.POItemId == poi.Id && i.RefLocaleId == poi.LocaleId && i.TransferInId == 0).Max(i => i.Id),
                    ReceivedQty = ReceivedLog.Get().Where(i => i.POItemId == poi.Id && i.RefLocaleId == poi.LocaleId && i.TransferInId == 0).Sum(i => i.IQCGetQty),
                    NewPONo = poi.PONo,
                    ShoeName = o.ShoeName,
                    CategoryCodeId = m.CategoryCodeId,
                    LCSD = o.LCSD,
                    ParentMaterial = Material.Get().Where(i => i.Id == poi.ParentMaterialId && i.LocaleId == poi.LocaleId).Max(i => i.MaterialName),
                    BatchId = pur.BatchId,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            if (items.Any())
            {
                var oIds = items.Select(i => i.OrdersId).Distinct().ToList();
                var oNos = items.Select(i => i.OrderNo).Distinct().ToList();
                var mIds = items.Select(i => i.MaterialId).Distinct().ToList();
                var mNames = items.Select(i => i.Material).Distinct().ToList();
                var poItemIds = items.Select(i => i.Id).Distinct().ToList();

                var localeId = items.Max(i => i.LocaleId);

                // S2: 找出管制表裡有下單的材料
                // MRPItem
                var mrpItems = MRPItem.Get().Where(i => i.LocaleId == localeId && oIds.Contains(i.OrdersId) && mIds.Contains(i.MaterialId))
                    .GroupBy(i => new { i.MaterialId, i.MaterialNameEn, i.MaterialNameTw, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw })
                    .Select(i => new { i.Key.MaterialId, i.Key.MaterialNameEn, i.Key.MaterialNameTw, i.Key.OrdersId, i.Key.LocaleId, i.Key.ParentMaterialId, i.Key.SizeDivision, i.Key.UnitCodeId, i.Key.UnitNameTw, Usage = i.Sum(g => g.Total) })
                    .Select(i => new
                    {
                        OrdersId = i.OrdersId,
                        LocaleId = i.LocaleId,
                        // OrderNo = i.OrderNo,
                        MaterialId = i.MaterialId,
                        Material = i.MaterialNameTw,
                        ParentMaterialId = i.ParentMaterialId,
                        Unit = i.UnitNameTw,
                        Total = i.Usage,
                        // Part = i.PartId
                    })
                    .ToList();

                // MRPOrderItem
                var mrpItemOrders = MRPItemOrders.Get().Where(i => i.LocaleId == localeId && oIds.Contains(i.OrdersId) && mIds.Contains(i.MaterialId))
                    .GroupBy(i => new { i.MaterialId, i.MaterialNameEn, i.MaterialNameTw, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw })
                    .Select(i => new { i.Key.MaterialId, i.Key.MaterialNameEn, i.Key.MaterialNameTw, i.Key.OrdersId, i.Key.LocaleId, i.Key.ParentMaterialId, i.Key.SizeDivision, i.Key.UnitCodeId, i.Key.UnitNameTw, Usage = i.Sum(g => g.Total) })
                    .Select(i => new
                    {
                        OrdersId = i.OrdersId,
                        LocaleId = i.LocaleId,
                        // OrderNo = i.OrderNo,
                        MaterialId = i.MaterialId,
                        Material = i.MaterialNameTw,
                        ParentMaterialId = i.ParentMaterialId,
                        Unit = i.UnitNameTw,
                        Total = i.Usage,
                        // Part = i.PartId
                    }).ToList();

                //Combind
                var tmpItems = mrpItems.Union(mrpItemOrders);
                var bomItems = tmpItems
                .GroupBy(g => new { g.OrdersId, g.LocaleId, g.Material, g.Unit, g.MaterialId, g.ParentMaterialId })
                .Select(g => new
                {
                    OrderId = g.Key.OrdersId,
                    LocaleId = g.Key.LocaleId,
                    Material = g.Key.Material,
                    MaterialId = g.Key.MaterialId,
                    ParentMaterialId = g.Key.ParentMaterialId,
                    Unit = g.Key.Unit,
                    SubUsage = g.Sum(i => i.Total)
                })
                .ToList();

                //最後組合超買%
                items.ForEach(i =>
                {
                    var bQty = bomItems.Where(m => m.LocaleId == i.LocaleId && m.OrderId == i.OrdersId && m.MaterialId == i.MaterialId && m.ParentMaterialId == i.ParentMaterialId).Sum(m => m.SubUsage);
                    i.BOMQty = bQty;
                    i.Diff = i.BOMQty == i.PlanQty ? 0 : 1;
                    i.Amount = i.PurQty * i.UnitPrice;
                });
            }
            return items.AsQueryable();
        }

        //採購單查詢(by size)
        public IQueryable<Models.Views.POItemForSizeRun> GetPOItemForSize(string predicate)
        {
            var sizeruns = new List<Models.Views.POItemForSizeRun>();
            var pos = (
                from pi in POItem.Get().Where(i => i.Status != 2)
                join p in PO.Get().Where(i => i.IsShowSizeRun == 1) on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = p.Id, LocaleId = p.LocaleId }
                join o in Orders.Get() on new { OrderId = pi.OrdersId, LocaleId = pi.LocaleId } equals new { OrderId = o.Id, LocaleId = o.LocaleId }
                join v in Vendor.Get() on new { VendorId = p.VendorId, LocaleId = p.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId }
                join m in Material.Get() on new { MaterialId = pi.MaterialId, LocaleId = pi.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
                join c in CodeItem.Get() on new { CodeItemId = pi.UnitCodeId, LocaleId = pi.LocaleId } equals new { CodeItemId = c.Id, LocaleId = c.LocaleId } into cGRP
                from c in cGRP.DefaultIfEmpty()
                select new
                {
                    Id = pi.Id,
                    LocaleId = pi.LocaleId,
                    Customer = o.LocaleNo,
                    VendorId = p.VendorId,
                    Vendor = v.ShortNameTw,
                    OrderNo = pi.OrderNo,
                    BatchNo = p.BatchNo,
                    SeqId = p.SeqId,
                    PONo = p.BatchNo + "-" + p.SeqId.ToString(),
                    OutsoleNo = o.OutsoleNo,
                    StyleNo = pi.StyleNo,
                    ShoeName = o.ShoeName,
                    POTypeId = pi.POType,
                    OrderQty = o.OrderQty,
                    PurchaseQty = pi.Qty,
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    PODate = p.PODate,
                    // Material = Material.Get().Where(m => m.Id == pi.MaterialId && m.LocaleId == pi.LocaleId).Max(m => m.MaterialName),
                    // MaterialEng = Material.Get().Where(m => m.Id == pi.MaterialId && m.LocaleId == pi.LocaleId).Max(m => m.MaterialNameEng),
                    Material = m.MaterialName,
                    MaterialEng = m.MaterialNameEng,
                    MaterialId = pi.MaterialId,
                    Price = pi.UnitPrice,
                    Amount = (pi.Qty * pi.UnitPrice),
                    Purchaser = pi.ModifyUserName,
                    Brand = o.Brand,
                    Company = o.CompanyNo,
                    CompanyId = o.CompanyId,
                    ArticleId = o.ArticleId,
                    BatchSeq = p.SeqId,
                    UnitCodeId = pi.UnitCodeId,
                    Unit = c.NameTW,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate);

            var poItems = (
                from pi in pos
                join ps in POItemSize.Get() on new { POItemId = pi.Id, LocaleId = pi.LocaleId } equals new { POItemId = ps.POItemId, LocaleId = ps.LocaleId }
                // join asr in ArticleSizeRun.Get() on new { ArticleId = pi.ArticleId, LocaleId = pi.LocaleId, SeqNo = (double)ps.SeqNo } equals
                //                                     new { ArticleId = asr.ArticleId, LocaleId = asr.LocaleId, SeqNo = asr.ArticleInnerSize }
                select new
                {
                    POItemId = ps.POItemId,
                    PODisplaySize = ps.DisplaySize,
                    Qty = ps.Qty,
                    SeqNo = ps.SeqNo,
                    PreQty = ps.PreQty,
                    ArticleId = pi.ArticleId,
                    LocaleId = pi.LocaleId
                    // ArticleSize = asr.ArticleSize,
                    // ArticleDisplaySize = asr.ArticleDisplaySize,
                    // ArticleInnerSize = asr.ArticleInnerSize,
                    // ArticleSizeSuffix = asr.ArticleSizeSuffix,
                    // OutsoleSize = asr.OutsoleSize,
                    // OutsoleDisplaySize = asr.OutsoleDisplaySize,
                    // OutsoleInnerSize = asr.OutsoleInnerSize,
                    // OutsoleSizeSuffix = asr.OutsoleSizeSuffix,
                }
            )
            .ToList();

            var poHeads = pos.ToList();
            var articleIds = poHeads.Select(i => i.ArticleId).Distinct().ToList();
            var localeIds = poHeads.Select(i => i.LocaleId).Distinct().ToList();
            var articleSizeRuns = ArticleSizeRun.Get().Where(i => articleIds.Contains(i.ArticleId) && localeIds.Contains(i.LocaleId)).ToList();

            // get all AlternateType size
            var allSizeRuns = articleSizeRuns.Select(i => new SizeRun
            {
                Size = i.ArticleSize,
                SizeSuffix = i.ArticleSizeSuffix,
                InnerSize = i.ArticleInnerSize,
                DisplaySize = i.ArticleDisplaySize.Trim(),
                ArticleId = i.ArticleId,
                LocaleId = i.LocaleId,
            });
            var articleHead = new List<string>();
            var outsoleHead = new List<string>();
            poHeads.ForEach(i =>
            {
                var articleSizeRun = new List<string>();
                var p = new Models.Views.POItemForSizeRun
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    Customer = i.Customer,
                    Vendor = i.Vendor,
                    VendorId = (int)i.VendorId,
                    OrderNo = i.OrderNo,
                    BatchNo = i.BatchNo,
                    BatchSeq = i.BatchSeq,
                    PONo = i.PONo,
                    POTypeId = i.POTypeId,
                    POType = (i.POTypeId == 1 || i.POTypeId == 2 || i.POTypeId == 3 || i.POTypeId == 4) ? "正單" : "補單",
                    OutsoleNo = i.OutsoleNo,
                    StyleNo = i.StyleNo,
                    ShoeName = i.ShoeName,
                    OrderQty = i.OrderQty,
                    PurchaseQty = i.PurchaseQty,
                    CSD = i.CSD,
                    LCSD = i.LCSD,
                    PODate = i.PODate,
                    Material = i.Material,
                    MaterialEng = i.MaterialEng,
                    MaterialId = (int)i.MaterialId,
                    Price = i.Price,
                    Amount = i.Amount,
                    Purchaser = i.Purchaser,
                    Barcode = (i.LocaleId + "*" + i.Id),

                    Brand = i.Brand,
                    Company = i.Company,
                    CompanyId = i.CompanyId,
                    UnitCodeId = i.UnitCodeId,
                    Unit = i.Unit ?? "",
                };

                poItems.Where(pi => pi.POItemId == i.Id).ToList().ForEach(pi =>
                {
                    var _size = Convert.ToDecimal(pi.PODisplaySize);

                    // 組合訂單 =========
                    var field = "";
                    var _sizeRun = allSizeRuns.Where(i => i.ArticleId == pi.ArticleId && i.Size == _size).FirstOrDefault();
                    if (_sizeRun != null)
                    {
                        if (_sizeRun.SizeSuffix != null && (_sizeRun.SizeSuffix.Contains("J") || _sizeRun.SizeSuffix.Contains("j")))
                        {
                            field = "S" + String.Format("{0:000000}", (_sizeRun.InnerSize * 10));
                        }
                        else
                        {
                            field = "S" + String.Format("{0:000000}", (_sizeRun.InnerSize * 10));
                        }
                        articleSizeRun.Add("\"" + field + "\":" + pi.Qty);
                        articleHead.Add("\"" + field + "\":\"" + _sizeRun.DisplaySize + "\"");

                        // 組合大底 =========
                        // outsoleSizeRun.Add("\"" + field + "\":" + pi.Qty);
                        // outsoleHead.Add("\"" + field + "\":\"" + pi.OutsoleDisplaySize + "\"");

                    }
                });
                p.ArticleSizeRun = "{" + string.Join(",", articleSizeRun) + "}";
                // p.OutsoleSizeRun = "{" + string.Join(",", outsoleSizeRun) + "}";
                sizeruns.Add(p);
            });
            sizeruns.ForEach(i =>
            {
                i.ArticleHead = "{" + string.Join(",", articleHead.Distinct().OrderBy(c => c)) + "}";
                // i.OutsoleHead = "{" + string.Join(",", outsoleHead.Distinct().OrderBy(c => c)) + "}";
                // i.ShellHead = "{" + string.Join(",", shellHead.Distinct().OrderBy(c => c)) + "}";
                // i.Other1Head = "{" + string.Join(",", other1Head.Distinct().OrderBy(c => c)) + "}";
                // i.Other2Head = "{" + string.Join(",", other2Head.Distinct().OrderBy(c => c)) + "}";
                // i.LastHead = "{" + string.Join(",", lastHead.Distinct().OrderBy(c => c)) + "}";
                // i.KnifeHead = "{" + string.Join(",", knifeHead.Distinct().OrderBy(c => c)) + "}";
            });

            return sizeruns.AsQueryable();
        }
        // 採購批次作業(劃批號)
        public IQueryable<Models.Views.PurOrdersItem> GetPurBatchOrders(string predicate)
        {
            var items = (
                from p in PurBatch.Get()
                join pi in PurOrdersItem.Get() on new { PurBatchId = p.Id, LocaleId = p.LocaleId } equals new { PurBatchId = pi.PurBatchId, LocaleId = pi.LocaleId }
                join o in Orders.Get() on new { Id = pi.OrdersId, LocaleId = pi.LocaleId } equals new { Id = o.Id, LocaleId = o.LocaleId }
                select new Models.Views.PurOrdersItem
                {
                    Id = pi.Id,
                    LocaleId = pi.LocaleId,
                    PurBatchId = pi.PurBatchId,
                    OrdersId = pi.OrdersId,
                    ModifyUserName = pi.ModifyUserName,
                    LastUpdateTime = pi.LastUpdateTime,
                    OrderNo = o.OrderNo,
                    OrderQty = o.OrderQty,
                    CompanyId = o.CompanyId,
                    ArticleNo = o.ArticleNo,
                    StyleNo = o.StyleNo,
                    CSD = o.CSD,
                    ETD = o.ETD,
                    LCSD = o.LCSD,
                    OrderType = o.OrderType,
                    ProductType = o.ProductType,
                    RefLocaleId = o.LocaleId,
                    BatchDate = p.BatchDate,
                    BatchNo = p.BatchNo,
                    Brand = o.Brand,
                    CustomerOrderNo = o.CustomerOrderNo,
                    Customer = o.Customer,
                    OWD = o.OWD
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            return items.AsQueryable();
        }
        // 採購明細查詢(BOM)
        public IQueryable<Models.Views.POItemForBOM> GetPOForBOM(string predicate)
        {
            // var company = Company.Get().ToList();
            var items = (
                from p in PO.Get()
                join v in Vendor.Get() on new { VendorId = p.VendorId, LocaleId = p.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId }
                join pi in POItem.Get().Where(i => i.Status != 2) on new { POId = p.Id, LocaleId = p.LocaleId } equals new { POId = pi.POId, LocaleId = pi.LocaleId }
                join pbi in PurBatchItem.Get() on new { POItemId = (decimal?)pi.Id, LocaleId = pi.LocaleId } equals new { POItemId = pbi.POItemId, LocaleId = pbi.LocaleId } into pbiGRP
                from pbi in pbiGRP.DefaultIfEmpty()
                join o in Orders.Get() on new { OrderNo = pi.OrderNo } equals new { OrderNo = o.OrderNo } into oGRP
                from o in oGRP.DefaultIfEmpty()
                join m in Material.Get() on new { MaterialId = pi.MaterialId, Localeid = pi.LocaleId } equals new { MaterialId = m.Id, Localeid = m.LocaleId }
                select new Models.Views.POItemForBOM
                {
                    Id = pi.Id,
                    LocaleId = pi.LocaleId,
                    POId = pi.POId,
                    OrdersId = pi.OrdersId,
                    OrderNo = pi.OrderNo,
                    StyleNo = pi.StyleNo,
                    POTypeId = pi.POType,
                    MaterialId = pi.MaterialId,
                    UnitPrice = pi.UnitPrice,
                    DollarCodeId = pi.DollarCodeId,
                    Qty = pi.Qty,
                    UnitCodeId = pi.UnitCodeId,
                    PayCodeId = pi.PayCodeId,
                    PurLocaleId = pi.PurLocaleId,
                    ReceivingLocaleId = pi.ReceivingLocaleId,
                    PaymentLocaleId = pi.PaymentLocaleId,
                    PaymentCodeId = pi.PaymentCodeId,
                    PaymentPoint = pi.PaymentPoint,
                    ParentMaterialId = pi.ParentMaterialId,
                    IsOverQty = pi.IsOverQty,
                    SamplingMethod = pi.SamplingMethod,
                    ModifyUserName = pi.ModifyUserName,
                    LastUpdateTime = pi.LastUpdateTime,
                    PayDollarCodeId = pi.PayDollarCodeId,
                    Status = pi.Status,
                    FactoryETD = pi.FactoryETD,
                    Remark = pi.Remark,
                    // CompanyId = o != null ? o.CompanyId : pi.CompanyId,
                    CompanyId = pi.CompanyId,
                    CustomerOrderNo = o.CustomerOrderNo,
                    IsSub = pi.ParentMaterialId > 0 ? true : false,
                    Material = m.MaterialName, //Material.Get().Where(m => m.Id == pi.MaterialId && m.LocaleId == pi.LocaleId).Max(m => m.MaterialName),
                    MaterialEng = m.MaterialNameEng,
                    Currency = CodeItem.Get().Where(c => c.Id == pi.DollarCodeId && c.LocaleId == pi.LocaleId && c.CodeType == "02").Max(c => c.NameTW),
                    Unit = CodeItem.Get().Where(c => c.Id == pi.UnitCodeId && c.LocaleId == pi.LocaleId && c.CodeType == "21").Max(c => c.NameTW),

                    PONo = p.BatchNo + "-" + p.SeqId.ToString(),
                    PlanQty = pbi == null ? 0 : pbi.PlanQty,
                    OnHandQty = pbi == null ? 0 : pbi.OnHandQty,
                    PlanUnitNameTw = pbi == null ? "" : CodeItem.Get().Where(c => c.Id == pbi.PlanUnitCodeId && c.LocaleId == pbi.LocaleId && c.CodeType == "21").Max(c => c.NameTW),

                    Vendor = v.NameTw,
                    VendorEng = v.NameEn,
                    VendorETD = p.VendorETD,
                    PODate = p.PODate,
                    Brand = o.Brand,
                    POType = (pi.POType == 1 || pi.POType == 2 || pi.POType == 3 || pi.POType == 4) ? "正單" : "補單",
                    POTypeEng = (pi.POType == 1 || pi.POType == 2 || pi.POType == 3 || pi.POType == 4) ? "Regular" : "Addition",
                    Purchaser = pi.ModifyUserName,
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    ReceivedBarcode = pi.LocaleId.ToString() + "*" + pi.Id.ToString(),
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();
            return items.AsQueryable();
        }

        //採購收貨明細表(廠商)
        public IQueryable<Models.Views.POItemForVendor> GetPOForVendor(string predicate)
        {
            // S1: 找出POItem的訂單、收貨、廠商資料
            // S2: 找出管制表裡有下單的材料
            // S3: 產生管制表細項（MRPItem+MRPItemOrders）(為了知道管制用量)
            // S4: 比對POItem的收貨資訊及管制表(為了知道管制用量)
            // S5: 整合相關的資料

            // S1: 找出POItem的訂單、收貨
            var baseQuery = (
                from pi in POItem.Get()
                join p in PO.Get() on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = p.Id, LocaleId = p.LocaleId }
                join pur in PurBatchItem.Get() on new { POItemId = pi.Id, LocaleId = pi.LocaleId } equals new { POItemId = (decimal)pur.POItemId, LocaleId = pur.LocaleId }
                join v in Vendor.Get() on new { VId = p.VendorId, LocaleId = p.LocaleId } equals new { VId = v.Id, LocaleId = v.LocaleId }
                join m in Material.Get() on new { MaterialId = pi.MaterialId, LocaleId = pi.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
                join o in Orders.Get() on new { OrdersId = pi.OrdersId, LocaleId = pi.LocaleId } equals new { OrdersId = o.Id, LocaleId = o.LocaleId } into oGRP
                from o in oGRP.DefaultIfEmpty()
                join rl in ReceivedLog.Get() on new { POItemId = pi.Id, LocaleId = pi.LocaleId } equals new { POItemId = rl.POItemId, LocaleId = rl.RefLocaleId } into rlGRP
                from rl in rlGRP.DefaultIfEmpty()
                select new Models.Views.POItemForVendor
                {
                    POItemId = pi.Id,
                    Vendor = v.NameTw,
                    PaymentPoint = v.PaymentPoint,
                    DayOfMonth = v.DayOfMonth,
                    MaterialId = pi.MaterialId,
                    MaterialNameTw = m.MaterialName,
                    MaterialNameEng = m.MaterialNameEng,
                    PODate = p.PODate,
                    PurQty = pi.Qty,
                    PurPlanQty = pur.PlanQty,
                    VendorETD = p.VendorETD,

                    Status = pi.Status,
                    POType = pi.POType,
                    PaymentLocaleId = pi.PaymentLocaleId,
                    PurLocaleId = pi.PurLocaleId,
                    LocaleId = pi.LocaleId,
                    PONo = p.BatchNo + "-" + p.SeqId,
                    ReceivedBarcode = pi.LocaleId.ToString() + "*" + pi.Id.ToString(),
                    Purchaser = pi.ModifyUserName,
                    PurUnitPrice = pi.UnitPrice,
                    OrdersId = pi.OrdersId,
                    OrderNo = pi.OrderNo,
                    // OrdersId = o.Id,
                    // OrderNo = o.OrderNo,
                    PurDollarNameTw = CodeItem.Get().Where(i => i.LocaleId == pi.LocaleId && i.Id == pi.DollarCodeId && i.CodeType == "02").Select(i => i.NameTW).FirstOrDefault(),
                    PurUnitNameTw = CodeItem.Get().Where(i => i.LocaleId == pi.LocaleId && i.Id == pi.UnitCodeId && i.CodeType == "21").Select(i => i.NameTW).FirstOrDefault(),


                    StyleNo = o.StyleNo ?? "",
                    ShoeName = o.ShoeName ?? "",
                    CSD = (DateTime?)o.CSD,
                    LCSD = (DateTime?)o.LCSD,
                    ReceivedDate = (DateTime?)rl.ReceivedDate,
                    UnitPrice = (decimal?)rl.UnitPrice ?? 0,
                    SubTotalPrice = (decimal?)rl.SubTotalPrice ?? 0,
                    IQCGetQty = (decimal?)rl.IQCGetQty ?? 0,
                    ReceivedQty = (decimal?)rl.ReceivedQty ?? 0,
                    Confirmer = (string?)rl.ModifyUserName ?? "",
                    ReceivedId = (decimal?)rl.Id ?? 0,
                    QCResult = (int?)rl.IQCResult ?? 0,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate);

            var items = baseQuery.ToList();
            if (items.Any())
            {
                var oIds = items.Select(i => i.OrdersId).Distinct().ToList();
                var oNos = items.Select(i => i.OrderNo).Distinct().ToList();
                var mIds = items.Select(i => i.MaterialId).Distinct().ToList();
                var mNames = items.Select(i => i.MaterialNameTw).Distinct().ToList();
                var poItemIds = items.Select(i => i.POItemId).Distinct().ToList();
                var localeIds = items.Select(i => i.LocaleId).Distinct().ToList();

                var localeId = items.Max(i => i.LocaleId);

                // S2: 找出管制表裡有下單的材料

                var filteredOrders = Orders.Get().Where(o => oNos.Contains(o.OrderNo));
                var filteredMRPItem = MRPItem.Get().Where(m => localeIds.Contains(m.LocaleId) && oIds.Contains(m.OrdersId) && mIds.Contains(m.MaterialId));
                var filteredMRPItemOrders = MRPItemOrders.Get().Where(m => localeIds.Contains(m.LocaleId) && oIds.Contains(m.OrdersId) && mIds.Contains(m.MaterialId));

                // 用 LINQ query syntax 撰寫 JOIN + GROUP BY
                var mrpItems = (
                    from o in filteredOrders
                    join m in filteredMRPItem on new { o.Id, o.LocaleId } equals new { Id = m.OrdersId, m.LocaleId }
                    select new { o.OrderNo, m.MaterialId, m.MaterialNameEn, m.MaterialNameTw, m.OrdersId, m.LocaleId, m.ParentMaterialId, m.SizeDivision, m.UnitCodeId, m.UnitNameTw, m.Total }
                )
                .GroupBy(i => new { i.OrderNo, i.MaterialId, i.MaterialNameEn, i.MaterialNameTw, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw })
                .Select(g => new { OrdersId = g.Key.OrdersId, LocaleId = g.Key.LocaleId, OrderNo = g.Key.OrderNo, MaterialId = g.Key.MaterialId, Material = g.Key.MaterialNameTw, ParentMaterial = g.Key.ParentMaterialId, Unit = g.Key.UnitNameTw, Total = g.Sum(x => x.Total) })
                .ToList();

                var mrpItemOrders = (
                    from o in filteredOrders
                    join m in filteredMRPItemOrders on new { o.Id, o.LocaleId } equals new { Id = m.OrdersId, m.LocaleId }
                    select new { o.OrderNo, m.MaterialId, m.MaterialNameEn, m.MaterialNameTw, m.OrdersId, m.LocaleId, m.ParentMaterialId, m.SizeDivision, m.UnitCodeId, m.UnitNameTw, m.Total }
                )
                .GroupBy(i => new { i.OrderNo, i.MaterialId, i.MaterialNameEn, i.MaterialNameTw, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw })
                .Select(g => new { OrdersId = g.Key.OrdersId, LocaleId = g.Key.LocaleId, OrderNo = g.Key.OrderNo, MaterialId = g.Key.MaterialId, Material = g.Key.MaterialNameTw, ParentMaterial = g.Key.ParentMaterialId, Unit = g.Key.UnitNameTw, Total = g.Sum(x => x.Total) })
                .ToList();

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

        public IQueryable<Models.Views.ChemPOSizeRun> GetChemPOSizeRun(string predicate)
        {
            // FOR IDM CHEM
            var sizeruns = new List<Models.Views.ChemPOSizeRun>();
            var pos = (
                    from pi in POItem.Get()
                    join p in PO.Get() on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = p.Id, LocaleId = p.LocaleId }
                    // join o in Orders.Get() on new { OrderNo = pi.OrderNo, LocaleId = pi.LocaleId } equals new { OrderNo = o.OrderNo, LocaleId = o.LocaleId }
                    join o in Orders.Get() on new { OrderId = pi.OrdersId, LocaleId = pi.LocaleId } equals new { OrderId = o.Id, LocaleId = o.LocaleId }
                    join v in Vendor.Get() on new { VendorId = p.VendorId, LocaleId = p.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId }
                    select new
                    {
                        Id = pi.Id,
                        LocaleId = pi.LocaleId,
                        Customer = o.LocaleNo,
                        // Vendor = Vendor.Get().Where(v => v.Id == p.VendorId && v.LocaleId == p.LocaleId).Max(v => v.NameTw),
                        VendorId = p.VendorId,
                        Vendor = v.NameTw,
                        OrderNo = pi.OrderNo,
                        BatchNo = p.BatchNo,
                        SeqId = p.SeqId,
                        PONo = p.BatchNo + "-" + p.SeqId.ToString(),
                        Component = "",
                        OutsoleNo = o.OutsoleNo,
                        StyleNo = pi.StyleNo,
                        ShoeName = o.ShoeName,
                        Color = "",
                        POTypeId = pi.POType,
                        POType = "",
                        OrderQty = o.OrderQty,
                        PurchaseQty = pi.Qty,
                        CSD = o.CSD,
                        LCSD = o.LCSD,
                        PODate = p.PODate,
                        RequestDate = pi.FactoryETD,
                        DeliveryDate = "",
                        Unit = CodeItem.Get().Where(c => c.Id == pi.UnitCodeId && c.LocaleId == pi.LocaleId && c.CodeType == "21").Max(c => c.NameTW),
                        Material = Material.Get().Where(m => m.Id == pi.MaterialId && m.LocaleId == pi.LocaleId).Max(m => m.MaterialName),
                        MaterialEng = Material.Get().Where(m => m.Id == pi.MaterialId && m.LocaleId == pi.LocaleId).Max(m => m.MaterialNameEng),
                        MaterialId = pi.MaterialId,
                        Price = pi.UnitPrice,
                        Amount = (pi.Qty * pi.UnitPrice),
                        Currency = CodeItem.Get().Where(c => c.Id == pi.DollarCodeId && c.LocaleId == pi.LocaleId && c.CodeType == "02").Max(c => c.NameTW),
                        Purchaser = pi.ModifyUserName,
                        // Barcode = (pi.LocaleId+"*"+pi.Id),
                        Remark = pi.Remark,
                        Status = pi.Status,
                        Brand = o.Brand,
                        Company = o.CompanyNo,
                        CompanyId = o.CompanyId,
                        ArticleId = o.ArticleId,
                        BatchSeq = p.SeqId,
                    }
                )
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate);

            var poItems = (
                from pi in pos
                join ps in POItemSize.Get() on new { POItemId = pi.Id, LocaleId = pi.LocaleId } equals new { POItemId = ps.POItemId, LocaleId = ps.LocaleId }
                join asr in ArticleSizeRun.Get() on new { ArticleId = pi.ArticleId, LocaleId = pi.LocaleId, SeqNo = (double)ps.SeqNo } equals
                                                    new { ArticleId = asr.ArticleId, LocaleId = asr.LocaleId, SeqNo = asr.ArticleInnerSize }
                select new
                {
                    POItemId = ps.POItemId,
                    PODisplaySize = ps.DisplaySize,
                    Qty = ps.Qty,
                    SeqNo = ps.SeqNo,
                    PreQty = ps.PreQty,
                    ArticleSize = asr.ArticleSize,
                    ArticleDisplaySize = asr.ArticleDisplaySize,
                    ArticleInnerSize = asr.ArticleInnerSize,
                    ArticleSizeSuffix = asr.ArticleSizeSuffix,
                    OutsoleSize = asr.OutsoleSize,
                    OutsoleDisplaySize = asr.OutsoleDisplaySize,
                    OutsoleInnerSize = asr.OutsoleInnerSize,
                    OutsoleSizeSuffix = asr.OutsoleSizeSuffix,
                }
            )
            .ToList();

            var poHeads = pos.ToList();

            poHeads.ForEach(i =>
            {
                var p = new Models.Views.ChemPOSizeRun
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    Customer = i.Customer,
                    Vendor = i.Vendor,
                    VendorId = (int)i.VendorId,
                    OrderNo = i.OrderNo,
                    BatchNo = i.BatchNo,
                    BatchSeq = i.BatchSeq,
                    PONo = i.PONo,
                    POTypeId = i.POTypeId,
                    POType = (i.POTypeId == 1 || i.POTypeId == 2 || i.POTypeId == 3 || i.POTypeId == 4) ? "正單" : "補單",
                    Component = i.Component,
                    OutsoleNo = i.OutsoleNo,
                    StyleNo = i.StyleNo,
                    ShoeName = i.ShoeName,
                    Color = i.Color,
                    OrderQty = i.OrderQty,
                    PurchaseQty = i.PurchaseQty,
                    CSD = i.CSD,
                    LCSD = i.LCSD,
                    PODate = i.PODate,
                    RequestDate = i.RequestDate,
                    Unit = i.Unit == null ? "" : i.Unit,
                    Material = i.Material,
                    MaterialEng = i.MaterialEng,
                    MaterialId = (int)i.MaterialId,
                    Price = i.Price,
                    Amount = i.Amount,
                    Currency = i.Currency == null ? "" : i.Currency,
                    Purchaser = i.Purchaser,
                    Barcode = (i.LocaleId + "*" + i.Id),
                    Remark = i.Remark,
                    Status = i.Status,

                    Brand = i.Brand,
                    Company = i.Company,
                    CompanyId = i.CompanyId,
                };
                poItems.Where(pi => pi.POItemId == i.Id).ToList().ForEach(pi =>
                {
                    var field = "";
                    var size = pi.OutsoleSize;
                    if (pi.OutsoleSizeSuffix.Contains("J") || pi.OutsoleSizeSuffix.Contains("j"))
                    {
                        field = "SJ" + String.Format("{0:000}", (Convert.ToDecimal(size) * 10));
                    }
                    else
                    {
                        field = "S" + String.Format("{0:000}", (Convert.ToDecimal(size) * 10));
                    }

                    if (p.GetType().GetProperty(field) != null)
                    {
                        p.GetType().GetProperty(field).SetValue(p, Math.Round(pi.Qty, 0));
                    }
                });
                sizeruns.Add(p);
            });

            return sizeruns.AsQueryable();
        }

        // 採購待收明細查詢(表頭),只取進行中的，結案就是未達收貨量，但是不用再收
        public IQueryable<Models.Views.PeddingReceiptForVendor> GetVendorForPeddingReceipt(string predicate)
        {
            var receivedLogs = (
                from r in ReceivedLog.Get()
                where r.TransferInId == 0
                group r by new { r.POItemId, r.RefLocaleId } into g
                select new
                {
                    POItemId = g.Key.POItemId,
                    RefLocaleId = g.Key.RefLocaleId,
                    PayQty = g.Sum(x => x.IQCGetQty)   // 非 null decimal
                });

            var items = (
                from pi in POItem.Get().Where(i => i.Status == 1)
                join p in PO.Get() on new { POId = pi.POId, pi.LocaleId } equals new { POId = p.Id, p.LocaleId }
                join v in Vendor.Get() on new { VId = p.VendorId, p.LocaleId } equals new { VId = v.Id, v.LocaleId }
                join r in receivedLogs on new { Id = pi.Id, LocaleId = pi.LocaleId } equals new { Id = r.POItemId, LocaleId = r.RefLocaleId } into rg
                from r in rg.DefaultIfEmpty()
                select new
                {
                    // POItemId = pi.Id,
                    // POId = p.Id,
                    p.PODate,
                    Qty = pi.Qty,
                    pi.LocaleId,
                    pi.PurLocaleId,
                    pi.PaymentLocaleId,
                    ReceivedLocaleId = pi.ReceivingLocaleId,
                    PayQty = (decimal?)r.PayQty ?? 0,
                    pi.OrderNo,
                    Vendor = v.NameTw,
                    VendorId = p.VendorId,
                    CompanyId = pi.CompanyId,
                    pi.ModifyUserName
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Where(i => i.Qty > i.PayQty)
            .GroupBy(i => new { i.Vendor, i.VendorId, i.CompanyId, i.PaymentLocaleId, i.ReceivedLocaleId, i.PurLocaleId })
            .Select(i => new Models.Views.PeddingReceiptForVendor
            {
                Vendor = i.Key.Vendor,
                VendorId = i.Key.VendorId,
                PaymentLocaleId = i.Key.PaymentLocaleId,
                PurLocaleId = i.Key.PurLocaleId,
                POLocaleId = i.Key.PurLocaleId,
                ReceivedLocaleId = i.Key.ReceivedLocaleId,
            })
            .Distinct()
            .ToList();

            return items.AsQueryable();
        }
        public IQueryable<Models.Views.PeddingReceiptForVendor> GetVendorForPeddingReceipt1(string predicate)
        {
            var items = (
                from pi in POItem.Get().Where(i => i.Status == 1)
                join p in PO.Get() on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = p.Id, LocaleId = p.LocaleId }
                join v in Vendor.Get() on new { VId = p.VendorId, LocaleId = p.LocaleId } equals new { VId = v.Id, LocaleId = v.LocaleId }
                join r in ReceivedLog.Get().Where(i => i.TransferInId == 0) on new { POItemId = pi.Id, LocaleId = pi.LocaleId } equals new { POItemId = r.POItemId, LocaleId = r.RefLocaleId } into rGPR
                from r in rGPR.DefaultIfEmpty()
                select new
                {
                    POItemId = pi.Id,
                    POId = p.Id,
                    PODate = p.PODate,
                    Qty = pi.Qty,
                    LocaleId = pi.LocaleId,
                    PurLocaleId = pi.PurLocaleId,
                    PaymentLocaleId = pi.PaymentLocaleId,
                    ReceivedLocaleId = pi.ReceivingLocaleId,
                    PayQty = r == null ? 0 : r.IQCGetQty,
                    OrderNo = pi.OrderNo,
                    Vendor = v.NameTw,
                    VendorId = p.VendorId,
                    CompanyId = pi.CompanyId,
                    ModifyUserName = pi.ModifyUserName,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .GroupBy(i => new { i.Vendor, i.VendorId, i.CompanyId, i.PaymentLocaleId, i.ReceivedLocaleId, i.PurLocaleId, i.POItemId, i.Qty })
            .Select(i => new
            {
                Vendor = i.Key.Vendor,
                VendorId = i.Key.VendorId,
                PaymentLocaleId = i.Key.PaymentLocaleId,
                PurLocaleId = i.Key.PurLocaleId,
                POLocaleId = i.Key.PurLocaleId,
                ReceivedLocaleId = i.Key.ReceivedLocaleId,
                Qty = i.Key.Qty,
                PayQty = i.Sum(g => g.PayQty),
                POItemId = i.Key.POItemId,
            })
            .ToList();

            var result = items
                .Where(i => i.Qty > i.PayQty)
                .GroupBy(i => new { i.Vendor, i.VendorId, i.PaymentLocaleId, i.PurLocaleId, i.POLocaleId, i.ReceivedLocaleId })
                .Select(i => new Models.Views.PeddingReceiptForVendor
                {
                    Vendor = i.Key.Vendor,
                    VendorId = i.Key.VendorId,
                    PaymentLocaleId = i.Key.PaymentLocaleId,
                    PurLocaleId = i.Key.PurLocaleId,
                    POLocaleId = i.Key.PurLocaleId,
                    ReceivedLocaleId = i.Key.ReceivedLocaleId,
                })
                .ToList()
                .AsQueryable();

            return result;
        }
        // 採購待收明細查詢(表身),只取進行中的，結案就是未達收貨量，但是不用再收
        public IQueryable<Models.Views.POItem> GetPOForPeddingReceipt(string predicate, string[] filters)
        {
            var hasPart = false;
            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                hasPart = (bool)extenFilters.Field9;
            }
            var receivedLogs = (
                from r in ReceivedLog.Get()
                where r.TransferInId == 0
                group r by new { r.POItemId, r.RefLocaleId } into g
                select new
                {
                    POItemId = g.Key.POItemId,
                    RefLocaleId = g.Key.RefLocaleId,
                    PayQty = g.Sum(x => x.IQCGetQty)   // 非 null decimal
                });

            var items = (
                from pi in POItem.Get().Where(i => i.Status == 1)
                join p in PO.Get() on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = p.Id, LocaleId = p.LocaleId }
                join v in Vendor.Get() on new { VId = p.VendorId, LocaleId = p.LocaleId } equals new { VId = v.Id, LocaleId = v.LocaleId }
                join o in Orders.Get() on new { OrdersId = pi.OrdersId, LocaleId = pi.LocaleId } equals new { OrdersId = o.Id, LocaleId = o.LocaleId } into oGRP
                from o in oGRP.DefaultIfEmpty()
                join m in Material.Get() on new { MaterialId = pi.MaterialId, LocaleId = pi.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGRP
                from m in mGRP.DefaultIfEmpty()
                join c in CodeItem.Get() on new { CodeItemId = pi.UnitCodeId, LocaleId = pi.LocaleId } equals new { CodeItemId = c.Id, LocaleId = c.LocaleId } into cGRP
                from c in cGRP.DefaultIfEmpty()
                join r in receivedLogs on new { Id = pi.Id, LocaleId = pi.LocaleId } equals new { Id = r.POItemId, LocaleId = r.RefLocaleId } into rg
                from r in rg.DefaultIfEmpty()
                select new Models.Views.POItem
                {
                    PurLocaleId = pi.PurLocaleId,
                    ReceivingLocaleId = pi.ReceivingLocaleId,
                    PaymentLocaleId = pi.PaymentLocaleId,
                    OrderNo = pi.OrderNo,
                    VendorId = p.VendorId,
                    VendorNameTw = v.NameTw,
                    Material = m.MaterialName,
                    PODate = p.PODate,
                    ReceivedBarcode = pi.LocaleId.ToString() + "*" + pi.Id,
                    MaterialId = pi.MaterialId,
                    UnitCodeId = pi.UnitCodeId,
                    Unit = c.NameTW,
                    Qty = pi.Qty,
                    UnitPrice = pi.UnitPrice,
                    DollarCodeId = pi.DollarCodeId,
                    // PayQty = ReceivedLog.Get().Where(i => i.POItemId == pi.Id && i.RefLocaleId == pi.LocaleId && i.TransferInId == 0).Sum(i => i.IQCGetQty),
                    PayQty = (decimal?)r.PayQty ?? 0,
                    StyleNo = pi.StyleNo,
                    ShoeName = o.ArticleName,
                    LCSD = o.LCSD,
                    POItemVendorETD = p.VendorETD,
                    CompanyId = pi.CompanyId,
                    ModifyUserName = pi.ModifyUserName,
                    PONo = p.BatchNo + "-" + p.SeqId.ToString(),
                    NewPONo = pi.PONo,
                    Id = pi.Id,
                    LocaleId = pi.LocaleId,
                    POId = pi.POId,
                    OrdersId = pi.OrdersId,
                    ParentMaterialId = pi.ParentMaterialId,
                    POType = pi.POType,

                    // PayCodeId = pi.PayCodeId,
                    // PaymentCodeId = pi.PaymentCodeId,
                    // PaymentPoint = pi.PaymentPoint,

                    // IsOverQty = pi.IsOverQty,
                    // SamplingMethod = pi.SamplingMethod,
                    // LastUpdateTime = pi.LastUpdateTime,
                    // PayDollarCodeId = pi.PayDollarCodeId,
                    // Status = pi.Status,
                    // FactoryETD = pi.FactoryETD,
                    // Remark = pi.Remark,
                    // IsAllowPartial = p.IsAllowPartial,
                    // VendorETD = p.VendorETD,

                    // Material = Material.Get().Where(m => m.Id == pi.MaterialId && m.LocaleId == pi.LocaleId).Select(m => m.MaterialName).FirstOrDefault(),
                    // Unit = CodeItem.Get().Where(c => c.Id == pi.UnitCodeId && c.LocaleId == pi.LocaleId && c.CodeType == "21").Select(c => c.NameTW).FirstOrDefault(),
                    // RefOrderNo = pi.OrderNo.Contains("-") ? pi.OrderNo.Substring(0, pi.OrderNo.IndexOf("-")) : pi.OrderNo,

                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            if (hasPart)
            {
                var oIds = items.Select(i => i.OrdersId).Distinct().ToList();
                var mIds = items.Select(i => i.MaterialId).Distinct().ToList();
                var localeId = items.Max(i => i.LocaleId);

                var mrpItems = MRPItem.Get()
                    .Where(i => oIds.Contains(i.OrdersId) && mIds.Contains(i.MaterialId) && i.LocaleId == localeId)
                    .Select(i => new { i.OrdersId, i.LocaleId, i.MaterialId, i.ParentMaterialId, i.PartNameTw })
                    .Distinct()
                    .ToList();

                var mrpItemOrders = MRPItemOrders.Get()
                    .Where(i => oIds.Contains(i.OrdersId) && mIds.Contains(i.MaterialId) && i.LocaleId == localeId)
                    .Select(i => new { i.OrdersId, i.LocaleId, i.MaterialId, i.ParentMaterialId, i.PartNameTw })
                    .Distinct()
                    .ToList();

                items.ForEach(i =>
                {
                    i.PayQty = i.PayQty ?? 0;
                    i.Part = string.Join("+", mrpItems.Where(m => m.OrdersId == i.OrdersId && m.LocaleId == i.LocaleId && m.MaterialId == i.MaterialId && m.ParentMaterialId == i.ParentMaterialId).Select(m => m.PartNameTw).ToArray()) +
                            string.Join("+", mrpItemOrders.Where(m => m.OrdersId == i.OrdersId && m.LocaleId == i.LocaleId && m.MaterialId == i.MaterialId && m.ParentMaterialId == i.ParentMaterialId).Select(m => m.PartNameTw).ToArray())
                    ;
                });
            }
            else
            {
                items.ForEach(i =>
               {
                   i.PayQty = i.PayQty ?? 0;
               });
            }

            // var result = items.Where(i => i.Qty > i.PayQty).ToList();
            return items.Where(i => i.Qty > i.PayQty).AsQueryable();
        }
        // 採購待收明細查詢(表身),全部不分廠商，只取進行中的，結案就是未達收貨量，但是不用再收
        public IQueryable<Models.Views.POItem> GetALLPOForPeddingReceipt(string predicate, string[] filters)
        {
            var hasPart = false;
            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                hasPart = (bool)extenFilters.Field9;
            }

            var receivedLogs = (
                from r in ReceivedLog.Get()
                where r.TransferInId == 0
                group r by new { r.POItemId, r.RefLocaleId } into g
                select new
                {
                    POItemId = g.Key.POItemId,
                    RefLocaleId = g.Key.RefLocaleId,
                    PayQty = g.Sum(x => x.IQCGetQty)   // 非 null decimal
                });

            var items = (
                from pi in POItem.Get().Where(i => i.Status == 1)
                join p in PO.Get() on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = p.Id, LocaleId = p.LocaleId }
                join v in Vendor.Get() on new { VId = p.VendorId, LocaleId = p.LocaleId } equals new { VId = v.Id, LocaleId = v.LocaleId }
                join o in Orders.Get() on new { OrdersId = pi.OrdersId, LocaleId = pi.LocaleId } equals new { OrdersId = o.Id, LocaleId = o.LocaleId } into oGRP
                from o in oGRP.DefaultIfEmpty()
                join m in Material.Get() on new { MaterialId = pi.MaterialId, LocaleId = pi.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGRP
                from m in mGRP.DefaultIfEmpty()
                join c in CodeItem.Get() on new { CodeItemId = pi.UnitCodeId, LocaleId = pi.LocaleId } equals new { CodeItemId = c.Id, LocaleId = c.LocaleId } into cGRP
                from c in cGRP.DefaultIfEmpty()
                join r in receivedLogs on new { Id = pi.Id, LocaleId = pi.LocaleId } equals new { Id = r.POItemId, LocaleId = r.RefLocaleId } into rg
                from r in rg.DefaultIfEmpty()
                select new Models.Views.POItem
                {
                    CompanyId = pi.CompanyId,
                    PurLocaleId = pi.PurLocaleId,
                    ReceivingLocaleId = pi.ReceivingLocaleId,
                    PaymentLocaleId = pi.PaymentLocaleId,
                    OrderNo = pi.OrderNo,
                    VendorId = p.VendorId,
                    VendorNameTw = v.NameTw,
                    Material = m.MaterialName,
                    PODate = p.PODate,
                    ReceivedBarcode = pi.LocaleId.ToString() + "*" + pi.Id,
                    MaterialId = pi.MaterialId,
                    ParentMaterialId = pi.ParentMaterialId,
                    UnitCodeId = pi.UnitCodeId,
                    Unit = c.NameTW,
                    Qty = pi.Qty,
                    UnitPrice = pi.UnitPrice,
                    DollarCodeId = pi.DollarCodeId,
                    // PayQty = ReceivedLog.Get().Where(i => i.POItemId == pi.Id && i.RefLocaleId == pi.LocaleId && i.TransferInId == 0).Sum(i => i.IQCGetQty),
                    PayQty = (decimal?)r.PayQty ?? 0,
                    StyleNo = pi.StyleNo,
                    ShoeName = o.ArticleName,
                    LCSD = o.LCSD,
                    POItemVendorETD = p.VendorETD,
                    ModifyUserName = pi.ModifyUserName,
                    PONo = p.BatchNo + "-" + p.SeqId.ToString(),
                    NewPONo = pi.PONo,
                    Id = pi.Id,
                    LocaleId = pi.LocaleId,
                    POId = pi.POId,
                    OrdersId = pi.OrdersId,
                    POType = pi.POType,
                    // PayCodeId = pi.PayCodeId,
                    // PaymentCodeId = pi.PaymentCodeId,
                    // PaymentPoint = pi.PaymentPoint,

                    // IsOverQty = pi.IsOverQty,
                    // SamplingMethod = pi.SamplingMethod,
                    // LastUpdateTime = pi.LastUpdateTime,
                    // PayDollarCodeId = pi.PayDollarCodeId,
                    // Status = pi.Status,
                    // FactoryETD = pi.FactoryETD,
                    // Remark = pi.Remark,
                    // IsAllowPartial = p.IsAllowPartial,
                    // VendorETD = p.VendorETD,

                    // Material = Material.Get().Where(m => m.Id == pi.MaterialId && m.LocaleId == pi.LocaleId).Select(m => m.MaterialName).FirstOrDefault(),
                    // Unit = CodeItem.Get().Where(c => c.Id == pi.UnitCodeId && c.LocaleId == pi.LocaleId && c.CodeType == "21").Select(c => c.NameTW).FirstOrDefault(),
                    // RefOrderNo = pi.OrderNo.Contains("-") ? pi.OrderNo.Substring(0, pi.OrderNo.IndexOf("-")) : pi.OrderNo,

                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            // var items = (
            //     from pi in POItem.Get().Where(i => i.Status == 1)
            //     join p in PO.Get() on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = p.Id, LocaleId = p.LocaleId }
            //     join v in Vendor.Get() on new { VId = p.VendorId, LocaleId = p.LocaleId } equals new { VId = v.Id, LocaleId = v.LocaleId }
            //     join o in Orders.Get() on new { OrdersId = pi.OrdersId, LocaleId = pi.LocaleId } equals new { OrdersId = o.Id, LocaleId = o.LocaleId } into oGRP
            //     from o in oGRP.DefaultIfEmpty()
            //     select new Models.Views.POItem
            //     {
            //         VendorId = p.VendorId,
            //         VendorNameTw = v.NameTw,
            //         PONo = p.BatchNo + "-" + p.SeqId.ToString(),
            //         NewPONo = pi.PONo,
            //         ReceivedBarcode = pi.LocaleId.ToString() + "*" + pi.Id,
            //         Material = Material.Get().Where(m => m.Id == pi.MaterialId && m.LocaleId == pi.LocaleId).Select(m => m.MaterialName).FirstOrDefault(),
            //         Unit = CodeItem.Get().Where(c => c.Id == pi.UnitCodeId && c.LocaleId == pi.LocaleId && c.CodeType == "21").Select(c => c.NameTW).FirstOrDefault(),
            //         Qty = pi.Qty,
            //         PayQty = ReceivedLog.Get().Where(i => i.POItemId == pi.Id && i.RefLocaleId == pi.LocaleId && i.TransferInId == 0).Sum(i => i.IQCGetQty) == null ? 0 :
            //                  ReceivedLog.Get().Where(i => i.POItemId == pi.Id && i.RefLocaleId == pi.LocaleId && i.TransferInId == 0).Sum(i => i.IQCGetQty),
            //         ReceivingLocaleId = pi.ReceivingLocaleId,
            //         OrdersId = pi.OrdersId,
            //         OrderNo = pi.OrderNo,
            //         StyleNo = pi.StyleNo,
            //         ShoeName = o.ArticleName,
            //         CompanyId = pi.CompanyId,
            //         LCSD = o.LCSD,
            //         // o.ETD,
            //         PODate = p.PODate,
            //         VendorETD = p.VendorETD,


            //         Id = pi.Id,
            //         LocaleId = pi.LocaleId,
            //         POId = pi.POId,
            //         POType = pi.POType,
            //         MaterialId = pi.MaterialId,
            //         ParentMaterialId = pi.ParentMaterialId,
            //         UnitPrice = pi.UnitPrice,
            //         DollarCodeId = pi.DollarCodeId,
            //         UnitCodeId = pi.UnitCodeId,
            //         PayCodeId = pi.PayCodeId,
            //         PurLocaleId = pi.PurLocaleId,
            //         PaymentLocaleId = pi.PaymentLocaleId,
            //         ModifyUserName = pi.ModifyUserName,
            //         LastUpdateTime = pi.LastUpdateTime,
            //         PayDollarCodeId = pi.PayDollarCodeId,
            //         Status = pi.Status,

            //         // PaymentCodeId = pi.PaymentCodeId,
            //         // PaymentPoint = pi.PaymentPoint,

            //         // IsOverQty = pi.IsOverQty,
            //         // SamplingMethod = pi.SamplingMethod,

            //         // FactoryETD = pi.FactoryETD,
            //         // Remark = pi.Remark,

            //         // IsAllowPartial = p.IsAllowPartial,

            //         // POItemVendorETD = p.VendorETD,
            //         // RefOrderNo = pi.OrderNo.Contains("-") ? pi.OrderNo.Substring(0, pi.OrderNo.IndexOf("-")) : pi.OrderNo,
            //     }
            // )
            // .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            // .Where(i => i.Qty > i.PayQty)
            // .ToList();

            if (hasPart)
            {
                var oIds = items.Select(i => i.OrdersId).Distinct().ToList();
                var mIds = items.Select(i => i.MaterialId).Distinct().ToList();
                var localeId = items.Max(i => i.LocaleId);

                var mrpItems = MRPItem.Get()
                    .Where(i => oIds.Contains(i.OrdersId) && mIds.Contains(i.MaterialId) && i.LocaleId == localeId)
                    .Select(i => new { i.OrdersId, i.LocaleId, i.MaterialId, i.ParentMaterialId, i.PartNameTw })
                    .Distinct()
                    .ToList();

                var mrpItemOrders = MRPItemOrders.Get()
                    .Where(i => oIds.Contains(i.OrdersId) && mIds.Contains(i.MaterialId) && i.LocaleId == localeId)
                    .Select(i => new { i.OrdersId, i.LocaleId, i.MaterialId, i.ParentMaterialId, i.PartNameTw })
                    .Distinct()
                    .ToList();

                items.ForEach(i =>
                {
                    i.PayQty = i.PayQty ?? 0;
                    i.Part = string.Join("+", mrpItems.Where(m => m.OrdersId == i.OrdersId && m.LocaleId == i.LocaleId && m.MaterialId == i.MaterialId && m.ParentMaterialId == i.ParentMaterialId).Select(m => m.PartNameTw).ToArray()) +
                            string.Join("+", mrpItemOrders.Where(m => m.OrdersId == i.OrdersId && m.LocaleId == i.LocaleId && m.MaterialId == i.MaterialId && m.ParentMaterialId == i.ParentMaterialId).Select(m => m.PartNameTw).ToArray())
                    ;
                });
            }
            else
            {
                items.ForEach(i =>
               {
                   i.PayQty = i.PayQty ?? 0;
               });
            }

            // var result = items.Where(i => i.Qty > i.PayQty).ToList();
            // return items.Where(i => i.Qty > i.PayQty).AsQueryable();

            return items.Where(i => i.Qty > i.PayQty).AsQueryable();
        }
        //管制表SizeRun查詢
        public IQueryable<Models.Views.PCLSizeRun> GetPCLSizeRun1(string predicate)
        {
            var sizeruns = new List<Models.Views.PCLSizeRun>();

            //取回所有資料
            var pclItem = (
                from o in Orders.Get()
                join pcl in PCL.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = (decimal)pcl.OrdersId, LocaleId = pcl.LocaleId }
                select new
                {
                    OrdersId = o.Id,
                    LocaleId = o.LocaleId,
                    Customer = o.Customer,
                    CustomerId = o.CustomerId,
                    CustomerOrderNo = o.CustomerOrderNo,
                    GBSPOReferenceNo = o.GBSPOReferenceNo,
                    CompanyId = o.CompanyId,
                    CompanyNo = o.CompanyNo,
                    ProductType = o.ProductType,
                    OrderQty = o.OrderQty,
                    OrderNo = o.OrderNo,
                    OrderDate = o.OrderDate,
                    OWD = o.OWD,
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    BrandCodeId = o.BrandCodeId,
                    Brand = o.Brand,
                    ArticleId = o.ArticleId,
                    ArticleNo = o.ArticleNo,
                    StyleNo = o.StyleNo,
                    ShoeName = o.ShoeName,
                    ArticleSizeCountry = o.ArticleSizeCountryCode,
                    OrderSizeCountry = o.OrderSizeCountryCode,
                    OutsoleNo = o.OutsoleNo,
                    LastNo = o.LastNo,

                    ArticleSize = pcl.ArticleSize,
                    ArticleSizeSuffix = pcl.ArticleSizeSuffix,
                    ArticleInnerSize = pcl.ArticleInnerSize,
                    ArticleDisplay = pcl.DisplaySize,

                    KnifeSize = pcl.KnifeSize,
                    KnifeSizeSuffix = pcl.KnifeSizeSuffix,
                    KnifeDisplaySize = pcl.KnifeDisplaySize,
                    KnifeInnerSize = pcl.KnifeInnerSize,

                    OutsoleSize = pcl.OutsoleSize,
                    OutsoleSizeSuffix = pcl.OutsoleSizeSuffix,
                    OutsoleInnerSize = pcl.OutsoleInnerSize,
                    OutsoleDisplaySize = pcl.OutsoleDisplaySize,

                    LastSize = pcl.LastSize,
                    LastInnerSize = pcl.LastInnerSize,
                    LastSizeSuffix = pcl.LastSizeSuffix,
                    LastDisplaySize = pcl.LastDisplaySize,

                    ShellSize = pcl.ShellSize,
                    ShellDisplaySize = pcl.ShellDisplaySize,
                    ShellSizeSuffix = pcl.ShellSizeSuffix,
                    ShellInnerSize = pcl.ShellInnerSize,

                    Other1Size = pcl.Other1Size,
                    Other1SizeSuffix = pcl.Other1SizeSuffix,
                    Other1InnerSize = pcl.Other1InnerSize,
                    Other1Desc = pcl.Other1Desc,

                    Other2Size = pcl.Other2Size,
                    Other2SizeSuffix = pcl.Other2SizeSuffix,
                    Other2InnerSize = pcl.Other2InnerSize,
                    Other2SizeDesc = pcl.Other2SizeDesc,

                    Qty = pcl.Qty,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            //從PCL取表頭
            var ordersHeads = pclItem.Select(o => new
            {
                OrdersId = o.OrdersId,
                LocaleId = o.LocaleId,
                Customer = o.Customer,
                CustomerId = o.CustomerId,
                CustomerOrderNo = o.CustomerOrderNo,
                GBSPOReferenceNo = o.GBSPOReferenceNo,
                CompanyId = o.CompanyId,
                CompanyNo = o.CompanyNo,
                ProductType = o.ProductType,
                OrderQty = o.OrderQty,
                OrderNo = o.OrderNo,
                OrderDate = o.OrderDate,
                OWD = o.OWD,
                CSD = o.CSD,
                LCSD = o.LCSD,
                BrandCodeId = o.BrandCodeId,
                Brand = o.Brand,
                ArticleId = o.ArticleId,
                ArticleNo = o.ArticleNo,
                StyleNo = o.StyleNo,
                ShoeName = o.ShoeName,
                ArticleSizeCountry = o.ArticleSizeCountry,
                OrderSizeCountry = o.OrderSizeCountry,
                OutsoleNo = o.OutsoleNo,
                LastNo = o.LastNo,
            })
            .Distinct()
            .ToList();

            var articleHead = new List<string>();
            var lastHead = new List<string>();
            var outsoleHead = new List<string>();
            var knifeHead = new List<string>();
            var shellHead = new List<string>();
            var other1Head = new List<string>();
            var other2Head = new List<string>();

            //按訂單組合表身sizerun qty
            ordersHeads.ForEach(i =>
            {
                var s = new Models.Views.PCLSizeRun();
                var articleSizeRun = new List<string>();
                var lastSizeRun = new List<string>();
                var outsoleSizeRun = new List<string>();
                var knifeSizeRun = new List<string>();
                var shellSizeRun = new List<string>();
                var other1SizeRun = new List<string>();
                var other2SizeRun = new List<string>();

                s.OrdersId = i.OrdersId;
                s.LocaleId = i.LocaleId;
                s.CompanyId = i.CompanyId;
                s.CompanyNo = i.CompanyNo;
                s.Customer = i.Customer;
                s.CustomerOrderNo = i.CustomerOrderNo;
                s.GBSPOReferenceNo = i.GBSPOReferenceNo;
                s.ProductType = i.ProductType;
                s.OrderQty = i.OrderQty;
                s.OrderNo = i.OrderNo;
                s.OWD = i.OWD;
                s.OrderDate = i.OrderDate;
                s.CSD = i.CSD;
                s.LCSD = i.LCSD;
                s.BrandCodeId = i.BrandCodeId;
                s.Brand = i.Brand;
                s.ArticleNo = i.ArticleNo;
                s.StyleNo = i.StyleNo;
                s.ShoeName = i.ShoeName;
                s.OrderSizeCountry = i.OrderSizeCountry;
                s.ArticleSizeCountry = i.ArticleSizeCountry;
                s.OutsoleNo = i.OutsoleNo;
                s.LastNo = i.LastNo;

                pclItem.Where(pcl => pcl.OrdersId == i.OrdersId && pcl.LocaleId == i.LocaleId)
                .ToList()
                .ForEach(pcl =>
                {
                    // 組合訂單 =========
                    var field = "";
                    if (pcl.ArticleSizeSuffix != null && (pcl.ArticleSizeSuffix.Contains("J") || pcl.ArticleSizeSuffix.Contains("j")))
                    {
                        field = "S" + String.Format("{0:000000}", (pcl.ArticleInnerSize * 10));
                    }
                    else
                    {
                        field = "S" + String.Format("{0:000000}", (pcl.ArticleInnerSize * 10));
                    }

                    articleSizeRun.Add("\"" + field + "\":" + pcl.Qty);
                    articleHead.Add("\"" + field + "\":\"" + pcl.ArticleDisplay + "\"");


                    // 組合楦頭 =========
                    lastSizeRun.Add("\"" + field + "\":" + pcl.Qty);
                    lastHead.Add("\"" + field + "\":\"" + pcl.LastDisplaySize + "\"");

                    // 組合斬刀 =========
                    knifeSizeRun.Add("\"" + field + "\":" + pcl.Qty);
                    knifeHead.Add("\"" + field + "\":\"" + pcl.KnifeDisplaySize + "\"");

                    // 組合大底 =========
                    outsoleSizeRun.Add("\"" + field + "\":" + pcl.Qty);
                    outsoleHead.Add("\"" + field + "\":\"" + pcl.OutsoleDisplaySize + "\"");

                    // 組合鞋墊 =========
                    shellSizeRun.Add("\"" + field + "\":" + pcl.Qty);
                    shellHead.Add("\"" + field + "\":\"" + pcl.ShellDisplaySize + "\"");

                    // 組合其他1 =========
                    other1SizeRun.Add("\"" + field + "\":" + pcl.Qty);
                    other1Head.Add("\"" + field + "\":\"" + pcl.Other1Desc + "\"");

                    // 組合其他2 =========
                    other2SizeRun.Add("\"" + field + "\":" + pcl.Qty);
                    other2Head.Add("\"" + field + "\":\"" + pcl.Other2SizeDesc + "\"");
                });

                s.LastSizeRun = "{" + string.Join(",", lastSizeRun) + "}";
                s.ArticleSizeRun = "{" + string.Join(",", articleSizeRun) + "}";
                s.KnifeSizeRun = "{" + string.Join(",", knifeSizeRun) + "}";
                s.OutsoleSizeRun = "{" + string.Join(",", outsoleSizeRun) + "}";
                s.ShellSizeRun = "{" + string.Join(",", shellSizeRun) + "}";
                s.Other1SizeRun = "{" + string.Join(",", other1SizeRun) + "}";
                s.Other2SizeRun = "{" + string.Join(",", other2SizeRun) + "}";

                sizeruns.Add(s);
            });
            //組合表頭
            sizeruns.ForEach(i =>
            {
                i.LastHead = "{" + string.Join(",", lastHead.Distinct().OrderBy(c => c)) + "}";
                i.ArticleHead = "{" + string.Join(",", articleHead.Distinct().OrderBy(c => c)) + "}";
                i.KnifeHead = "{" + string.Join(",", knifeHead.Distinct().OrderBy(c => c)) + "}";
                i.OutsoleHead = "{" + string.Join(",", outsoleHead.Distinct().OrderBy(c => c)) + "}";
                i.ShellHead = "{" + string.Join(",", shellHead.Distinct().OrderBy(c => c)) + "}";
                i.Other1Head = "{" + string.Join(",", other1Head.Distinct().OrderBy(c => c)) + "}";
                i.Other2Head = "{" + string.Join(",", other2Head.Distinct().OrderBy(c => c)) + "}";

            });

            return sizeruns.AsQueryable();
        }

        public IQueryable<Models.Views.PCLSizeRun> GetPCLSizeRun(string predicate)
        {
            var sizeruns = new List<Models.Views.PCLSizeRun>();

            //取回所有資料
            var pclItem = (
                from o in Orders.Get()
                join pcl in PCL.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = (decimal)pcl.OrdersId, LocaleId = pcl.LocaleId }
                select new
                {
                    OrdersId = o.Id,
                    LocaleId = o.LocaleId,
                    Customer = o.Customer,
                    CustomerId = o.CustomerId,
                    CustomerOrderNo = o.CustomerOrderNo,
                    GBSPOReferenceNo = o.GBSPOReferenceNo,
                    CompanyId = o.CompanyId,
                    CompanyNo = o.CompanyNo,
                    ProductType = o.ProductType,
                    OrderQty = o.OrderQty,
                    OrderNo = o.OrderNo,
                    OrderDate = o.OrderDate,
                    OWD = o.OWD,
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    BrandCodeId = o.BrandCodeId,
                    Brand = o.Brand,
                    ArticleId = o.ArticleId,
                    ArticleNo = o.ArticleNo,
                    StyleNo = o.StyleNo,
                    ShoeName = o.ShoeName,
                    ArticleSizeCountry = o.ArticleSizeCountryCode,
                    OrderSizeCountry = o.OrderSizeCountryCode,
                    OutsoleNo = o.OutsoleNo,
                    LastNo = o.LastNo,

                    ArticleSize = pcl.ArticleSize,
                    ArticleSizeSuffix = pcl.ArticleSizeSuffix,
                    ArticleInnerSize = pcl.ArticleInnerSize,
                    ArticleDisplay = pcl.DisplaySize,

                    KnifeSize = pcl.KnifeSize,
                    KnifeSizeSuffix = pcl.KnifeSizeSuffix,
                    KnifeDisplaySize = pcl.KnifeDisplaySize,
                    KnifeInnerSize = pcl.KnifeInnerSize,

                    OutsoleSize = pcl.OutsoleSize,
                    OutsoleSizeSuffix = pcl.OutsoleSizeSuffix,
                    OutsoleInnerSize = pcl.OutsoleInnerSize,
                    OutsoleDisplaySize = pcl.OutsoleDisplaySize,

                    LastSize = pcl.LastSize,
                    LastInnerSize = pcl.LastInnerSize,
                    LastSizeSuffix = pcl.LastSizeSuffix,
                    LastDisplaySize = pcl.LastDisplaySize,

                    ShellSize = pcl.ShellSize,
                    ShellDisplaySize = pcl.ShellDisplaySize,
                    ShellSizeSuffix = pcl.ShellSizeSuffix,
                    ShellInnerSize = pcl.ShellInnerSize,

                    Other1Size = pcl.Other1Size,
                    Other1SizeSuffix = pcl.Other1SizeSuffix,
                    Other1InnerSize = pcl.Other1InnerSize,
                    Other1Desc = pcl.Other1Desc,

                    Other2Size = pcl.Other2Size,
                    Other2SizeSuffix = pcl.Other2SizeSuffix,
                    Other2InnerSize = pcl.Other2InnerSize,
                    Other2SizeDesc = pcl.Other2SizeDesc,

                    Qty = pcl.Qty,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            //從PCL取表頭
            var ordersHeads = pclItem.Select(o => new
            {
                OrdersId = o.OrdersId,
                LocaleId = o.LocaleId,
                Customer = o.Customer,
                CustomerId = o.CustomerId,
                CustomerOrderNo = o.CustomerOrderNo,
                GBSPOReferenceNo = o.GBSPOReferenceNo,
                CompanyId = o.CompanyId,
                CompanyNo = o.CompanyNo,
                ProductType = o.ProductType,
                OrderQty = o.OrderQty,
                OrderNo = o.OrderNo,
                OrderDate = o.OrderDate,
                OWD = o.OWD,
                CSD = o.CSD,
                LCSD = o.LCSD,
                BrandCodeId = o.BrandCodeId,
                Brand = o.Brand,
                ArticleId = o.ArticleId,
                ArticleNo = o.ArticleNo,
                StyleNo = o.StyleNo,
                ShoeName = o.ShoeName,
                ArticleSizeCountry = o.ArticleSizeCountry,
                OrderSizeCountry = o.OrderSizeCountry,
                OutsoleNo = o.OutsoleNo,
                LastNo = o.LastNo,
            })
            .Distinct()
            .ToList();

            var articleHead = new List<string>();
            var lastHead = new List<string>();
            var outsoleHead = new List<string>();
            var knifeHead = new List<string>();
            var shellHead = new List<string>();
            var other1Head = new List<string>();
            var other2Head = new List<string>();

            //按訂單組合表身sizerun qty
            ordersHeads.ForEach(i =>
            {
                var s = new Models.Views.PCLSizeRun();
                var articleSizeRun = new List<string>();
                var lastSizeRun = new List<string>();
                var outsoleSizeRun = new List<string>();
                var knifeSizeRun = new List<string>();
                var shellSizeRun = new List<string>();
                var other1SizeRun = new List<string>();
                var other2SizeRun = new List<string>();

                s.OrdersId = i.OrdersId;
                s.LocaleId = i.LocaleId;
                s.CompanyId = i.CompanyId;
                s.CompanyNo = i.CompanyNo;
                s.Customer = i.Customer;
                s.CustomerOrderNo = i.CustomerOrderNo;
                s.GBSPOReferenceNo = i.GBSPOReferenceNo;
                s.ProductType = i.ProductType;
                s.OrderQty = i.OrderQty;
                s.OrderNo = i.OrderNo;
                s.OWD = i.OWD;
                s.OrderDate = i.OrderDate;
                s.CSD = i.CSD;
                s.LCSD = i.LCSD;
                s.BrandCodeId = i.BrandCodeId;
                s.Brand = i.Brand;
                s.ArticleNo = i.ArticleNo;
                s.StyleNo = i.StyleNo;
                s.ShoeName = i.ShoeName;
                s.OrderSizeCountry = i.OrderSizeCountry;
                s.ArticleSizeCountry = i.ArticleSizeCountry;
                s.OutsoleNo = i.OutsoleNo;
                s.LastNo = i.LastNo;

                pclItem.Where(pcl => pcl.OrdersId == i.OrdersId && pcl.LocaleId == i.LocaleId)
                .ToList()
                .ForEach(pcl =>
                {
                    // 組合訂單 =========
                    var field = "";
                    if (pcl.ArticleSizeSuffix != null && (pcl.ArticleSizeSuffix.Contains("J") || pcl.ArticleSizeSuffix.Contains("j")))
                    {
                        field = "SJ" + String.Format("{0:000}", (pcl.ArticleInnerSize * 10));
                    }
                    else
                    {
                        field = "S" + String.Format("{0:000}", (pcl.ArticleInnerSize / 100));
                    }
                    articleSizeRun.Add("\"" + field + "\":" + pcl.Qty);
                    articleHead.Add("\"" + field + "\":\"" + pcl.ArticleDisplay + "\"");

                    // 組合楦頭 =========
                    var lastField = "";
                    if (pcl.LastSizeSuffix != null && (pcl.LastSizeSuffix.Contains("J") || pcl.LastSizeSuffix.Contains("j")))
                    {
                        lastField = "SJ" + String.Format("{0:000}", (pcl.LastInnerSize * 10));
                    }
                    else
                    {
                        lastField = "S" + String.Format("{0:000}", (pcl.LastInnerSize / 100));
                    }
                    lastSizeRun.Add("\"" + lastField + "\":" + pcl.Qty);
                    lastHead.Add("\"" + lastField + "\":\"" + pcl.LastDisplaySize + "\"");

                    // 組合斬刀 =========
                    var knifeField = "";
                    if (pcl.KnifeSizeSuffix != null && (pcl.KnifeSizeSuffix.Contains("J") || pcl.KnifeSizeSuffix.Contains("j")))
                    {
                        knifeField = "SJ" + String.Format("{0:000}", (pcl.KnifeInnerSize * 10));
                    }
                    else
                    {
                        knifeField = "S" + String.Format("{0:000}", (pcl.KnifeInnerSize / 100));
                    }
                    knifeSizeRun.Add("\"" + knifeField + "\":" + pcl.Qty);
                    knifeHead.Add("\"" + knifeField + "\":\"" + pcl.KnifeDisplaySize + "\"");

                    // 組合大底 =========
                    var outsoleField = "";
                    if (pcl.OutsoleSizeSuffix != null && (pcl.OutsoleSizeSuffix.Contains("J") || pcl.OutsoleSizeSuffix.Contains("j")))
                    {
                        outsoleField = "SJ" + String.Format("{0:000}", (pcl.OutsoleInnerSize * 10));
                    }
                    else
                    {
                        outsoleField = "S" + String.Format("{0:000}", (pcl.OutsoleInnerSize / 100));
                    }
                    outsoleSizeRun.Add("\"" + outsoleField + "\":" + pcl.Qty);
                    outsoleHead.Add("\"" + outsoleField + "\":\"" + pcl.OutsoleDisplaySize + "\"");

                    // 組合鞋墊 =========
                    var shellField = "";
                    if (pcl.ShellSizeSuffix != null && (pcl.ShellSizeSuffix.Contains("J") || pcl.ShellSizeSuffix.Contains("j")))
                    {
                        shellField = "SJ" + String.Format("{0:000}", (pcl.ShellInnerSize * 10));
                    }
                    else
                    {
                        shellField = "S" + String.Format("{0:000}", (pcl.ShellInnerSize / 100));
                    }
                    shellSizeRun.Add("\"" + shellField + "\":" + pcl.Qty);
                    shellHead.Add("\"" + shellField + "\":\"" + pcl.ShellDisplaySize + "\"");

                    // 組合其他1 =========
                    var other1Field = "";
                    if (pcl.Other1SizeSuffix != null && (pcl.Other1SizeSuffix.Contains("J") || pcl.Other1SizeSuffix.Contains("j")))
                    {
                        other1Field = "SJ" + String.Format("{0:000}", (pcl.Other1InnerSize * 10));
                    }
                    else
                    {
                        other1Field = "S" + String.Format("{0:000}", (pcl.Other1InnerSize / 100));
                    }
                    other1SizeRun.Add("\"" + other1Field + "\":" + pcl.Qty);
                    other1Head.Add("\"" + other1Field + "\":\"" + pcl.Other1Desc + "\"");

                    // 組合其他2 =========
                    var other2Field = "";
                    if (pcl.Other2SizeSuffix != null && (pcl.Other2SizeSuffix.Contains("J") || pcl.Other2SizeSuffix.Contains("j")))
                    {
                        other2Field = "SJ" + String.Format("{0:000}", (pcl.Other2InnerSize * 10));
                    }
                    else
                    {
                        other2Field = "S" + String.Format("{0:000}", (pcl.Other2InnerSize / 100));
                    }
                    other2SizeRun.Add("\"" + other2Field + "\":" + pcl.Qty);
                    other2Head.Add("\"" + other2Field + "\":\"" + pcl.Other2SizeDesc + "\"");
                });

                s.LastSizeRun = "{" + string.Join(",", lastSizeRun) + "}";
                s.ArticleSizeRun = "{" + string.Join(",", articleSizeRun) + "}";
                s.KnifeSizeRun = "{" + string.Join(",", knifeSizeRun) + "}";
                s.OutsoleSizeRun = "{" + string.Join(",", outsoleSizeRun) + "}";
                s.ShellSizeRun = "{" + string.Join(",", shellSizeRun) + "}";
                s.Other1SizeRun = "{" + string.Join(",", other1SizeRun) + "}";
                s.Other2SizeRun = "{" + string.Join(",", other2SizeRun) + "}";

                sizeruns.Add(s);
            });
            //組合表頭
            sizeruns.ForEach(i =>
            {
                i.LastHead = "{" + string.Join(",", lastHead.Distinct().OrderBy(c => c)) + "}";
                i.ArticleHead = "{" + string.Join(",", articleHead.Distinct().OrderBy(c => c)) + "}";
                i.KnifeHead = "{" + string.Join(",", knifeHead.Distinct().OrderBy(c => c)) + "}";
                i.OutsoleHead = "{" + string.Join(",", outsoleHead.Distinct().OrderBy(c => c)) + "}";
                i.ShellHead = "{" + string.Join(",", shellHead.Distinct().OrderBy(c => c)) + "}";
                i.Other1Head = "{" + string.Join(",", other1Head.Distinct().OrderBy(c => c)) + "}";
                i.Other2Head = "{" + string.Join(",", other2Head.Distinct().OrderBy(c => c)) + "}";

            });

            return sizeruns.AsQueryable();
        }
    }

    public class SizeRun
    {
        public decimal ArticleId { get; set; }
        public decimal LocaleId { get; set; }
        public decimal Size { get; set; }
        public string SizeSuffix { get; set; }
        public double InnerSize { get; set; }
        public string DisplaySize { get; set; }
    }
}