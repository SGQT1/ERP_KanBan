using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Diamond.DataSource.Extensions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views.Common;
using ERP.Services.Bases;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ERP.Services.Business.Entities
{
    public class POItemService : BusinessService
    {
        private ERP.Services.Entities.POService PO { get; set; }
        private ERP.Services.Entities.POItemService POItem { get; set; }
        private ERP.Services.Entities.MaterialService Material { get; set; }
        private ERP.Services.Entities.CodeItemService CodeItem { get; set; }
        private ERP.Services.Entities.VendorService Vendor { get; set; }
        private ERP.Services.Entities.PurBatchItemService PurBatchItem { get; set; }
        private ERP.Services.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Entities.CompanyService Company { get; set; }
        private ERP.Services.Entities.ReceivedLogService ReceivedLog { get; set; }
        private ERP.Services.Entities.POItemPrintLogService POItemPrintLog { get; set; }
        private ERP.Services.Entities.MaterialQuotService Quotation { get; set; }

        public POItemService(
            ERP.Services.Entities.POService poService,
            ERP.Services.Entities.POItemService poItemService,
            ERP.Services.Entities.PurBatchItemService purBatchItemService,
            ERP.Services.Entities.MaterialService materialService,
            ERP.Services.Entities.CodeItemService codeItemServcie,
            ERP.Services.Entities.VendorService vendorService,
            ERP.Services.Entities.POItemPrintLogService poItemPrintLogService,
            ERP.Services.Entities.CompanyService companyService,
            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Entities.ReceivedLogService receivedLogService,
            ERP.Services.Entities.MaterialQuotService quotationService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            PO = poService;
            POItem = poItemService;
            PurBatchItem = purBatchItemService;
            Material = materialService;
            CodeItem = codeItemServcie;
            Vendor = vendorService;
            POItemPrintLog = poItemPrintLogService;
            Orders = ordersService;
            Company = companyService;
            ReceivedLog = receivedLogService;
            Quotation = quotationService;
        }
        public IQueryable<Models.Views.POItem> Get1()
        {
            var result = (
                from poi in POItem.Get()
                join po in PO.Get() on new { POId = poi.POId, LocaleId = poi.LocaleId } equals new { POId = po.Id, LocaleId = po.LocaleId }
                join m in Material.Get() on new { MId = poi.MaterialId, LocaleId = poi.LocaleId } equals new { MId = m.Id, LocaleId = m.LocaleId } into mGRP
                from m in mGRP.DefaultIfEmpty()
                join pur in PurBatchItem.Get() on new { POItemId = poi.Id, LocaleId = poi.LocaleId } equals new { POItemId = (decimal)pur.POItemId, LocaleId = pur.LocaleId } into purGRP
                from pur in purGRP.DefaultIfEmpty()
                select new Models.Views.POItem
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
                    PONo = poi.PONo,
                    // PONo = po.BatchNo + '-' + po.SeqId.ToString(),
                    AlternateType = poi.AlternateType == null ? 0 : poi.AlternateType,
                    PurBatchItemId = poi.PurBatchItemId,
                    RefOrderNo = poi.OrderNo.Contains("-") ? poi.OrderNo.Substring(0, poi.OrderNo.IndexOf("-")) : poi.OrderNo,
                    // ReceivedBarcode = poi.LocaleId.ToString() + "*" + poi.Id.ToString(),
                    ReceivedBarcode = (poi.Id == null) ? null : (poi.LocaleId.ToString() + "*" + poi.Id.ToString()),
                    Currency = CodeItem.Get().Where(c => c.Id == poi.DollarCodeId && c.LocaleId == poi.LocaleId && c.CodeType == "02").Select(c => c.NameTW).FirstOrDefault(),
                    Unit = CodeItem.Get().Where(c => c.Id == poi.UnitCodeId && c.LocaleId == poi.LocaleId && c.CodeType == "21").Select(c => c.NameTW).FirstOrDefault(),
                    ReceivedLogId = ReceivedLog.Get().Where(i => i.POItemId == poi.Id && i.RefLocaleId == poi.LocaleId).Max(i => i.Id),
                    ReceivedQty = ReceivedLog.Get().Where(i => i.POItemId == poi.Id && i.RefLocaleId == poi.LocaleId).Sum(i => i.IQCGetQty),
                    NewPONo = poi.PONo,

                    VendorId = po.VendorId,
                    IsAllowPartial = po.IsAllowPartial,
                    VendorETD = po.VendorETD,
                    POItemVendorETD = po.VendorETD,
                    IsShowSizeRun = po.IsShowSizeRun,
                    // RefPONo = po.BatchNo + "-" + po.SeqId.ToString(),
                    RefPONo = (po.BatchNo == null || po.SeqId == null) ? null : (po.BatchNo + "-" + po.SeqId.ToString()),
                    PODate = po.PODate,
                    SeqId = po.SeqId,
                    BatchNo = po.BatchNo,
                    POTeam = po.POTeam,

                    VendorNameTw = Vendor.Get().Where(v => v.Id == po.VendorId && v.LocaleId == po.LocaleId).Max(v => v.NameTw),

                    Material = m.MaterialName,
                    MaterialEng = m.MaterialNameEng,
                    CategoryCodeId = m.CategoryCodeId,
                    SemiGoods = m.SemiGoods,

                    PCLUnitCodeId = pur.PlanUnitCodeId,
                    PCLUnitNameTw = CodeItem.Get().Where(c => c.Id == pur.PlanUnitCodeId && c.LocaleId == pur.LocaleId && c.CodeType == "21").Select(c => c.NameTW).FirstOrDefault(),

                    PlanQty = pur.PlanQty,
                    PurQty = pur.PurQty,

                    // 報價單單價，以有效日期的單價作為最新報價
                    QuotUnitPrice = Quotation.Get().Where(i => i.VendorId == po.VendorId && i.LocaleId == po.LocaleId && i.MaterialId == poi.MaterialId && i.EffectiveDate <= po.PODate && i.Enable == 1).OrderByDescending(i => i.EffectiveDate).Select(i => i.UnitPrice).FirstOrDefault() == null ? 0 :
                                    Quotation.Get().Where(i => i.VendorId == po.VendorId && i.LocaleId == po.LocaleId && i.MaterialId == poi.MaterialId && i.EffectiveDate <= po.PODate && i.Enable == 1).OrderByDescending(i => i.EffectiveDate).Select(i => i.UnitPrice).FirstOrDefault(),
                    // LastQuotUnitPrice = Quotation.Get().Where(i => i.VendorId == po.VendorId && i.LocaleId == po.LocaleId && i.MaterialId == poi.MaterialId &&i.Enable == 1).OrderByDescending(i => i.EffectiveDate).Select(i => i.UnitPrice).FirstOrDefault() == null ? 0 :
                    //                 Quotation.Get().Where(i => i.VendorId == po.VendorId && i.LocaleId == po.LocaleId && i.MaterialId == poi.MaterialId && i.Enable == 1).OrderByDescending(i => i.EffectiveDate).Select(i => i.UnitPrice).FirstOrDefault(),
                }
            );
            return result;
        }
        public IQueryable<Models.Views.POItem> Get()
        {
            var poItems = POItem.Get();
            var pos = PO.Get();
            var materials = Material.Get();
            var purBatch = PurBatchItem.Get();
            var vendors = Vendor.Get();
            var codeItems = CodeItem.Get();
            var quotations = Quotation.Get();

            // 收料彙總（避免在 Select 內對同一組資料反覆 Max/Sum，且完全可轉 SQL）
            var rlAgg =
                from r in ReceivedLog.Get()
                group r by new { r.POItemId, r.RefLocaleId } into g
                select new
                {
                    g.Key.POItemId,
                    g.Key.RefLocaleId,
                    ReceivedLogId = g.Max(x => x.Id),
                    ReceivedQty = g.Sum(x => (decimal?)x.IQCGetQty) ?? 0m
                };

            var query =
                from poi in poItems
                join po in pos on new { poi.POId, poi.LocaleId } equals new { POId = po.Id, po.LocaleId }
                join m0 in materials on new { MId = poi.MaterialId, poi.LocaleId } equals new { MId = m0.Id, m0.LocaleId } into mGRP
                from m in mGRP.DefaultIfEmpty()
                join pur0 in purBatch on new { POItemId = (decimal?)poi.Id, poi.LocaleId } equals new { POItemId = (decimal?)pur0.POItemId, pur0.LocaleId } into purGRP
                from pur in purGRP.DefaultIfEmpty()
                join v0 in vendors on new { VendorId = po.VendorId, po.LocaleId } equals new { VendorId = v0.Id, v0.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                join cur0 in codeItems.Where(c => c.CodeType == "02") on new { Id = (decimal?)poi.DollarCodeId, poi.LocaleId } equals new { Id = (decimal?)cur0.Id, cur0.LocaleId } into curGRP
                from currency in curGRP.DefaultIfEmpty()
                join u0 in codeItems.Where(c => c.CodeType == "21") on new { Id = (decimal?)poi.UnitCodeId, poi.LocaleId } equals new { Id = (decimal?)u0.Id, u0.LocaleId } into uGRP
                from unit in uGRP.DefaultIfEmpty()
                join pu0 in codeItems.Where(c => c.CodeType == "21") on new { Id = (decimal?)pur.PlanUnitCodeId, LocaleId = (decimal?)pur.LocaleId } equals new { Id = (decimal?)pu0.Id, LocaleId = (decimal?)pu0.LocaleId } into puGRP
                from pclUnit in puGRP.DefaultIfEmpty()
                join rl in rlAgg on new { POItemId = (decimal?)poi.Id, RefLocaleId = (decimal?)poi.LocaleId } equals new { POItemId = (decimal?)rl.POItemId, RefLocaleId = (decimal?)rl.RefLocaleId } into rlJoin
                from rlx in rlJoin.DefaultIfEmpty()
                from latestQuot in quotations
                    .Where(q => q.VendorId == po.VendorId
                                && q.LocaleId == po.LocaleId
                                && q.MaterialId == poi.MaterialId
                                && q.EffectiveDate <= po.PODate
                                && q.Enable == 1)
                    .OrderByDescending(q => q.EffectiveDate)
                    .Select(q => new { q.UnitPrice })
                    .Take(1)
                    .DefaultIfEmpty()
                select new Models.Views.POItem
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

                    // 你原本直接拿 PONo（保留）
                    PONo = poi.PONo,
                    NewPONo = poi.PONo,

                    // 來源 PO
                    VendorId = po.VendorId,
                    IsAllowPartial = po.IsAllowPartial,
                    VendorETD = po.VendorETD,
                    POItemVendorETD = po.VendorETD,
                    IsShowSizeRun = po.IsShowSizeRun,
                    RefPONo = (po.BatchNo == null || po.SeqId == null) ? null : (po.BatchNo + "-" + po.SeqId.ToString()),
                    PODate = po.PODate,
                    SeqId = po.SeqId,
                    BatchNo = po.BatchNo,
                    POTeam = po.POTeam,

                    // 原本子查詢 → 直接取 JOIN 欄位
                    VendorNameTw = v.NameTw,

                    Material = m.MaterialName,
                    MaterialEng = m.MaterialNameEng,
                    CategoryCodeId = m.CategoryCodeId,
                    SemiGoods = m.SemiGoods,

                    PCLUnitCodeId = pur.PlanUnitCodeId,
                    PCLUnitNameTw = pclUnit.NameTW,

                    PlanQty = pur.PlanQty,
                    PurQty = pur.PurQty,

                    // 原本子查詢：Currency / Unit
                    Currency = currency.NameTW,
                    Unit = unit.NameTW,

                    // 收料彙總（避免空→給預設）
                    ReceivedLogId = rlx.ReceivedLogId,
                    ReceivedQty = rlx.ReceivedQty,

                    // 最新報價（若無 → 0）
                    QuotUnitPrice = (decimal?)latestQuot.UnitPrice ?? 0m,

                    // 你的條碼邏輯
                    ReceivedBarcode = poi.Id == null ? null : (poi.LocaleId.ToString() + "*" + poi.Id.ToString()),

                    AlternateType = poi.AlternateType ?? 0,
                    PurBatchItemId = poi.PurBatchItemId,
                    RefOrderNo = poi.OrderNo.Contains("-") ? poi.OrderNo.Substring(0, poi.OrderNo.IndexOf("-")) : poi.OrderNo
                };

            return query; // 你的 DbContext 預設已 NoTracking；如果要再保險可加 .AsNoTracking()
        }

        public IQueryable<Models.Views.POItem> GetSimple()
        {
            var result = (
                from poi in POItem.Get()
                join po in PO.Get() on new { POId = poi.POId, LocaleId = poi.LocaleId } equals new { POId = po.Id, LocaleId = po.LocaleId }
                select new Models.Views.POItem
                {
                    Id = poi.Id,
                    LocaleId = poi.LocaleId,
                    MaterialId = poi.MaterialId,
                    PODate = po.PODate,
                    ParentMaterialId = poi.ParentMaterialId,
                    VendorId = po.VendorId,
                    UnitCodeId = poi.UnitCodeId,
                    UnitPrice = poi.UnitPrice,
                    DollarCodeId = poi.DollarCodeId,
                    Status = poi.Status,
                    NewPONo = poi.PONo,
                    OrdersId = poi.OrdersId,
                    OrderNo = poi.OrderNo,
                    Qty = poi.Qty,
                    POType = poi.POType,    // 必要，有採購計劃S2判斷類型用到
                }
            );
            return result;
        }
        public IQueryable<Models.Views.POItem> GetLatest()
        {
            // Step 1: 建一個只有 MaterialId + LocaleId + MaxDate 的查詢
            var latestDateQuery = (
                from poi in POItem.Get()
                join po in PO.Get() on new { poi.POId, poi.LocaleId } equals new { POId = po.Id, po.LocaleId }
                where poi.Status != 2
                group po by new { poi.MaterialId, poi.LocaleId } into g
                select new
                {
                    g.Key.MaterialId,
                    g.Key.LocaleId,
                    MaxDate = g.Max(x => x.PODate)
                }
            );

            // Step 2: 用 MaxDate 再 join POItem + PO，挑出該日期下 ID 最大的那筆
            var latestInfo = (
                from poi in POItem.Get()
                join po in PO.Get() on new { poi.POId, poi.LocaleId } equals new { POId = po.Id, po.LocaleId }
                join max in latestDateQuery on new { poi.MaterialId, poi.LocaleId, PODate = po.PODate }
                    equals new { max.MaterialId, max.LocaleId, PODate = max.MaxDate }
                where poi.Status != 2
                group poi by new { poi.MaterialId, poi.LocaleId } into g
                select new
                {
                    g.Key.MaterialId,
                    g.Key.LocaleId,
                    MaxPOItemId = g.Max(x => x.Id)
                }
            );

            // Step 3: 回到主查詢，Join 回 POItem + PO
            var result = (
                from poi in POItem.Get()
                join po in PO.Get() on new { poi.POId, poi.LocaleId } equals new { POId = po.Id, po.LocaleId }
                join lpoi in latestInfo on new { poi.Id, poi.MaterialId, poi.LocaleId } equals new { Id = lpoi.MaxPOItemId, lpoi.MaterialId, lpoi.LocaleId }
                select new Models.Views.POItem
                {
                    Id = poi.Id,
                    LocaleId = poi.LocaleId,
                    MaterialId = poi.MaterialId,
                    PODate = po.PODate,
                    ParentMaterialId = poi.ParentMaterialId,
                    VendorId = po.VendorId,
                    UnitCodeId = poi.UnitCodeId,
                    UnitPrice = poi.UnitPrice,
                    DollarCodeId = poi.DollarCodeId,
                    Status = poi.Status,
                    NewPONo = poi.PONo,
                    OrdersId = poi.OrdersId,
                    OrderNo = poi.OrderNo,
                    Qty = poi.Qty,
                }
            );
            return result;
        }

        public IQueryable<Models.Entities.POItem> GetEntity()
        {
            return POItem.Get();
        }
        public IQueryable<Models.Views.POItem> GetPOReceivedLog()
        {
            var result = (
                from poi in POItem.Get()
                join po in PO.Get() on new { POId = poi.POId, LocaleId = poi.LocaleId } equals new { POId = po.Id, LocaleId = po.LocaleId }
                join m in Material.Get() on new { MId = poi.MaterialId, LocaleId = poi.LocaleId } equals new { MId = m.Id, LocaleId = m.LocaleId }
                join pur in PurBatchItem.Get() on new { POItemId = poi.Id, LocaleId = poi.LocaleId } equals new { POItemId = (decimal)pur.POItemId, LocaleId = pur.LocaleId } into purGRP
                from pur in purGRP.DefaultIfEmpty()
                select new Models.Views.POItem
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
                    VendorNameTw = Vendor.Get().Where(v => v.Id == po.VendorId && v.LocaleId == po.LocaleId).Max(v => v.NameTw),
                    IsAllowPartial = po.IsAllowPartial,
                    VendorETD = po.VendorETD,
                    POItemVendorETD = po.VendorETD,
                    PONo = poi.PONo,
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
                    ReceivedLogId = ReceivedLog.Get().Where(r => r.POItemId == poi.Id && r.RefLocaleId == poi.LocaleId).Max(r => r.Id),
                    ReceivedQty = ReceivedLog.Get().Where(i => i.POItemId == poi.Id && i.RefLocaleId == poi.LocaleId).Sum(i => i.IQCGetQty),
                    NewPONo = poi.PONo,
                }
            );
            return result;
        }
        public IQueryable<Models.Views.POItem> GetForRemove()
        {
            var result = (
                from poi in POItem.Get()
                join po in PO.Get() on new { POId = poi.POId, LocaleId = poi.LocaleId } equals new { POId = po.Id, LocaleId = po.LocaleId } into pGRP
                from po in pGRP.DefaultIfEmpty()
                join m in Material.Get() on new { MId = poi.MaterialId, LocaleId = poi.LocaleId } equals new { MId = m.Id, LocaleId = m.LocaleId } into mGRP
                from m in mGRP.DefaultIfEmpty()
                join pur in PurBatchItem.Get() on new { POItemId = poi.Id, LocaleId = poi.LocaleId } equals new { POItemId = (decimal)pur.POItemId, LocaleId = pur.LocaleId } into purGRP
                from pur in purGRP.DefaultIfEmpty()
                select new Models.Views.POItem
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
                    VendorNameTw = Vendor.Get().Where(v => v.Id == po.VendorId && v.LocaleId == po.LocaleId).Max(v => v.NameTw),
                    IsAllowPartial = po.IsAllowPartial,
                    VendorETD = po.VendorETD,
                    POItemVendorETD = po.VendorETD,
                    PONo = poi.PONo,
                    // PONo = po.BatchNo + '-' + po.SeqId.ToString(),
                    IsShowSizeRun = po.IsShowSizeRun,
                    AlternateType = poi.AlternateType == null ? 0 : poi.AlternateType,
                    PurBatchItemId = poi.PurBatchItemId,
                    RefPONo = po.BatchNo + "-" + po.SeqId.ToString(),
                    Material = m.MaterialName,
                    MaterialEng = m.MaterialNameEng,
                    Currency = CodeItem.Get().Where(c => c.Id == poi.DollarCodeId && c.LocaleId == poi.LocaleId && c.CodeType == "02").Select(c => c.NameTW).FirstOrDefault(),
                    Unit = CodeItem.Get().Where(c => c.Id == poi.UnitCodeId && c.LocaleId == poi.LocaleId && c.CodeType == "21").Select(c => c.NameTW).FirstOrDefault(),

                    PCLUnitCodeId = pur.PlanUnitCodeId,
                    PCLUnitNameTw = CodeItem.Get().Where(c => c.Id == pur.PlanUnitCodeId && c.LocaleId == pur.LocaleId && c.CodeType == "21").Select(c => c.NameTW).FirstOrDefault(),

                    RefOrderNo = poi.OrderNo.Contains("-") ? poi.OrderNo.Substring(0, poi.OrderNo.IndexOf("-")) : poi.OrderNo,
                    ReceivedBarcode = poi.LocaleId.ToString() + "*" + poi.Id.ToString(),
                    PODate = po.PODate,
                    PlanQty = pur.PlanQty,
                    PurQty = pur.PurQty,
                    SeqId = po.SeqId,
                    BatchNo = po.BatchNo,
                    POTeam = po.POTeam,
                    ReceivedLogId = ReceivedLog.Get().Where(i => i.POItemId == poi.Id && i.RefLocaleId == poi.LocaleId).Max(i => i.Id),
                    ReceivedQty = ReceivedLog.Get().Where(i => i.POItemId == poi.Id && i.RefLocaleId == poi.LocaleId).Sum(i => i.IQCGetQty),
                    NewPONo = poi.PONo,
                    CategoryCodeId = m.CategoryCodeId,
                }
            );
            return result;
        }
        public void CreateRange(IEnumerable<Models.Views.POItem> items)
        {
            POItem.CreateRange(BuildRange(items));
        }
        public void UpdateRange(IEnumerable<Models.Views.POItem> items)
        {
            POItem.UpdateRange(BuildRange(items));
        }
        public void UpdateRange(IEnumerable<Models.Entities.POItem> items)
        {
            POItem.UpdateRange(items);
        }

        public Models.Views.POItem Create(Models.Views.POItem item)
        {
            var _item = POItem.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }

        //批次採購，用來更新最Pubatch的資料
        // public Models.Views.POItem UpdateWithPurBatchItem(Models.Views.POItem item)
        // {
        //     var _item = POItem.Update(Build(item));

        //     PurBatchItem.UpdateRange(
        //         i => i.POItemId == _item.Id && i.LocaleId == _item.LocaleId,
        //         // u => new Models.Entities.PurBatchItem
        //         // {
        //         //     OrdersId = _item.OrdersId,
        //         //     PlanUnitCodeId = _item.UnitCodeId,
        //         //     PurUnitPrice = (decimal)_item.UnitPrice,
        //         //     DollarCodeId = _item.DollarCodeId,
        //         //     PayCodeId = _item.PayCodeId,
        //         //     PurUnitCodeId = _item.UnitCodeId,
        //         //     PurQty = _item.Qty,
        //         //     PurLocaleId = _item.LocaleId, //_item.PurLocaleId,
        //         //     ReceivingLocaleId = _item.ReceivingLocaleId,
        //         //     PaymentLocaleId = _item.PaymentLocaleId,
        //         //     ModifyUserName = _item.ModifyUserName,
        //         //     LastUpdateTime = _item.LastUpdateTime,
        //         //     PayDollarCodeId = _item.PayDollarCodeId,
        //         //     AlternateType = _item.AlternateType,
        //         // }
        //         u => u.SetProperty(p => p.OrdersId, v => _item.OrdersId).SetProperty(p => p.PlanUnitCodeId, v => _item.UnitCodeId).SetProperty(p => p.PurUnitPrice, v => (decimal)_item.UnitPrice).SetProperty(p => p.DollarCodeId, v => _item.DollarCodeId)
        //               .SetProperty(p => p.PayCodeId, v => _item.PayCodeId).SetProperty(p => p.PurUnitCodeId, v => _item.UnitCodeId).SetProperty(p => p.PurQty, v => _item.Qty).SetProperty(p => p.PurLocaleId, v => _item.LocaleId)
        //               .SetProperty(p => p.ReceivingLocaleId, v => _item.ReceivingLocaleId).SetProperty(p => p.PaymentLocaleId, v => _item.PaymentLocaleId).SetProperty(p => p.ModifyUserName, v => _item.ModifyUserName).SetProperty(p => p.LastUpdateTime, v => _item.LastUpdateTime)
        //               .SetProperty(p => p.PayDollarCodeId, v => _item.PayDollarCodeId).SetProperty(p => p.AlternateType, v => _item.AlternateType)
        //     );
        //     return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        // }
        public Models.Views.POItem Update(Models.Views.POItem item)
        {
            var _item = POItem.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        //其他採購，只更新PO的PurbatchItemId
        // public void UpdatePart(Models.Views.POItem item)
        // {
        //     // var _item = POItem.Update(Build(item));
        //     POItem.UpdateRange(
        //         i => i.Id == item.Id && i.LocaleId == item.LocaleId,
        //         // u => new Models.Entities.POItem { PurBatchItemId = item.PurBatchItemId }
        //         u => u.SetProperty(p => p.PurBatchItemId, v => item.PurBatchItemId)
        //     );
        // }

        private Models.Entities.POItem Build(Models.Views.POItem item)
        {
            return new Models.Entities.POItem()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                POId = item.POId,
                OrdersId = item.OrdersId,
                OrderNo = item.OrderNo,
                StyleNo = item.StyleNo,
                POType = item.POType,
                MaterialId = item.MaterialId,
                UnitPrice = item.UnitPrice,
                DollarCodeId = item.DollarCodeId,
                Qty = item.Qty,
                UnitCodeId = item.UnitCodeId,
                PayCodeId = item.PayCodeId,
                PurLocaleId = item.LocaleId, //item.PurLocaleId,
                ReceivingLocaleId = item.ReceivingLocaleId,
                PaymentLocaleId = item.PaymentLocaleId,
                PaymentCodeId = item.PaymentCodeId,
                PaymentPoint = item.PaymentPoint,
                ParentMaterialId = item.ParentMaterialId,
                IsOverQty = item.IsOverQty,
                SamplingMethod = item.SamplingMethod,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                PayDollarCodeId = item.PayDollarCodeId,
                Status = item.Status,
                FactoryETD = item.FactoryETD,
                Remark = item.Remark,
                CompanyId = item.LocaleId, //item.CompanyId,
                PONo = item.PONo,
                PurBatchItemId = item.PurBatchItemId,
                AlternateType = item.AlternateType,
                MRPVersion = item.MRPVersion,
            };
        }
        private IEnumerable<Models.Entities.POItem> BuildRange(IEnumerable<Models.Views.POItem> items)
        {
            return items.Select(item => new Models.Entities.POItem()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                POId = item.POId,
                OrdersId = item.OrdersId,
                OrderNo = item.OrderNo,
                StyleNo = item.StyleNo,
                POType = item.POType,
                MaterialId = item.MaterialId,
                UnitPrice = item.UnitPrice,
                DollarCodeId = item.DollarCodeId,
                Qty = item.Qty,
                UnitCodeId = item.UnitCodeId,
                PayCodeId = item.PayCodeId,
                PurLocaleId = item.LocaleId, //item.PurLocaleId,
                ReceivingLocaleId = item.ReceivingLocaleId,
                PaymentLocaleId = item.PaymentLocaleId,
                PaymentCodeId = item.PaymentCodeId,
                PaymentPoint = item.PaymentPoint,
                ParentMaterialId = item.ParentMaterialId,
                IsOverQty = item.IsOverQty,
                SamplingMethod = item.SamplingMethod,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                PayDollarCodeId = item.PayDollarCodeId,
                Status = item.Status,
                FactoryETD = item.FactoryETD,
                Remark = item.Remark,
                CompanyId = item.LocaleId, //item.CompanyId,
                PONo = item.PONo,
                PurBatchItemId = item.PurBatchItemId,
                AlternateType = item.AlternateType,
                MRPVersion = item.MRPVersion,
            });
        }

        public void Remove(Models.Views.POItem item)
        {
            POItem.Remove(Build(item));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.POItem, bool>> predicate)
        {
            POItem.RemoveRange(predicate);
        }

        public void UpdateStatus(int localeId, string userName, IEnumerable<decimal> closeIds, IEnumerable<decimal> activeIds, IEnumerable<decimal> cancelIds)
        {
            // update close poitem
            POItem.UpdateRange(
                i => closeIds.Contains(i.Id) && i.LocaleId == localeId,
                // u => new Models.Entities.POItem { Status = 0, ModifyUserName = userName, LastUpdateTime = DateTime.Now }
                u => u.SetProperty(p => p.Status, v => 0).SetProperty(p => p.ModifyUserName, v => userName).SetProperty(p => p.LastUpdateTime, v => DateTime.Now)
            );

            PurBatchItem.UpdateRange(
                i => closeIds.Contains((decimal)i.POItemId) && i.LocaleId == localeId,
                // u => new Models.Entities.PurBatchItem { Status = 0, ModifyUserName = userName, LastUpdateTime = DateTime.Now }
                u => u.SetProperty(p => p.Status, v => 0).SetProperty(p => p.ModifyUserName, v => userName).SetProperty(p => p.LastUpdateTime, v => DateTime.Now)
            );

            // update active poitem
            POItem.UpdateRange(
                i => activeIds.Contains(i.Id) && i.LocaleId == localeId,
                // u => new Models.Entities.POItem { Status = 1, ModifyUserName = userName, LastUpdateTime = DateTime.Now }
                u => u.SetProperty(p => p.Status, v => 1).SetProperty(p => p.ModifyUserName, v => userName).SetProperty(p => p.LastUpdateTime, v => DateTime.Now)
            );
            PurBatchItem.UpdateRange(
                i => activeIds.Contains((decimal)i.POItemId) && i.LocaleId == localeId,
                // u => new Models.Entities.PurBatchItem { Status = 1, ModifyUserName = userName, LastUpdateTime = DateTime.Now }
                u => u.SetProperty(p => p.Status, v => 1).SetProperty(p => p.ModifyUserName, v => userName).SetProperty(p => p.LastUpdateTime, v => DateTime.Now)
            );

            // update close poitem
            POItem.UpdateRange(
                i => cancelIds.Contains(i.Id) && i.LocaleId == localeId,
                // u => new Models.Entities.POItem { Status = 2, ModifyUserName = userName, LastUpdateTime = DateTime.Now }
                u => u.SetProperty(p => p.Status, v => 2).SetProperty(p => p.ModifyUserName, v => userName).SetProperty(p => p.LastUpdateTime, v => DateTime.Now)
            );
            PurBatchItem.UpdateRange(
                i => cancelIds.Contains((decimal)i.POItemId) && i.LocaleId == localeId,
                // u => new Models.Entities.PurBatchItem { Status = 2, ModifyUserName = userName, LastUpdateTime = DateTime.Now }
                u => u.SetProperty(p => p.Status, v => 2).SetProperty(p => p.ModifyUserName, v => userName).SetProperty(p => p.LastUpdateTime, v => DateTime.Now)
            );
        }
        public void UpdatePrice(IEnumerable<Models.Views.POItem> items)
        {
            // update close poitem
            POItem.UpdateRange(BuildRange(items));
        }

        //採購單列印 --加入join PurBatchItem，有PurBatchItem 才是有效的採購單
        public IQueryable<Models.Views.POItemForPrint> GetPOForPrint(string predicate, string[] filters)
        {
            var company = Company.Get().Select(i => new { i.Id, i.CompanyNo, i.ChineseName, i.ChineseAddress }).ToList();

            var groupBy = false;
            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                groupBy = (bool)extenFilters.Field9;
            }

            var items = (
                from pi in POItem.Get()
                join p in PO.Get() on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = p.Id, LocaleId = p.LocaleId }
                join pb in PurBatchItem.Get() on new { POId = (decimal?)pi.Id, LocaleId = pi.LocaleId } equals new { POId = pb.POItemId, LocaleId = pb.LocaleId }
                join o in Orders.Get() on new { OrderNo = pi.OrderNo } equals new { OrderNo = o.OrderNo } into oGRP
                from o in oGRP.DefaultIfEmpty()
                select new Models.Views.POItemForPrint
                {
                    VendorId = p.VendorId,
                    Vendor = Vendor.Get().Where(i => i.Id == p.VendorId && i.LocaleId == p.LocaleId).Max(i => i.ShortNameTw),
                    MaterialId = pi.MaterialId,
                    MaterialName = Material.Get().Where(i => i.Id == pi.MaterialId && i.LocaleId == pi.LocaleId).Max(i => i.MaterialName),
                    MaterialNameEng = Material.Get().Where(i => i.Id == pi.MaterialId && i.LocaleId == pi.LocaleId).Max(i => i.MaterialNameEng),
                    PODate = p.PODate,
                    PurDollarNameTw = CodeItem.Get().Where(i => i.Id == pi.DollarCodeId && i.LocaleId == pi.LocaleId && i.CodeType == "02").Max(i => i.NameTW),
                    PurUnitNameTw = CodeItem.Get().Where(i => i.Id == pi.UnitCodeId && i.LocaleId == pi.LocaleId && i.CodeType == "21").Max(i => i.NameTW),
                    PurQty = pi.Qty,
                    VendorETD = p.VendorETD,
                    POVendorETD = pi.FactoryETD,
                    OrderNo = pi.OrderNo,
                    StyleNo = pi.StyleNo,
                    POType = (int)pi.POType,
                    CompanyId = o.CompanyId,
                    CompanyNo = o.CompanyNo,
                    PaymentLocaleId = pi.PaymentLocaleId,
                    PurLocaleId = pi.PurLocaleId,
                    ReceivedLocaleId = pi.ReceivingLocaleId,
                    LocaleId = pi.LocaleId,
                    // PONo = p.BatchNo + "-" + p.SeqId,
                    PONo = pi.PONo,
                    PrintCount = POItemPrintLog.Get().Where(i => i.RefPOItemId == pi.Id && i.RefLocaleId == pi.LocaleId).Count(),
                    PrintTime = POItemPrintLog.Get().Where(i => i.RefPOItemId == pi.Id && i.RefLocaleId == pi.LocaleId).Max(i => i.PrintTime),
                    ModifyUserName = pi.ModifyUserName,
                    LastUpdateTime = pi.LastUpdateTime,
                    UnitPrice = pi.UnitPrice,
                    DollarId = pi.PayDollarCodeId,
                    Status = pi.Status,
                    POId = p.Id,
                    POItemId = pi.Id,
                    LCSD = o.LCSD,
                    ReceivedBarcode = pi.LocaleId.ToString() + "*" + pi.Id.ToString(),
                    Customer = o.Customer,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            if (groupBy)
            {
                items = items
                    .GroupBy(g => new
                    {
                        g.POId,
                        // g.POItemId,
                        g.LocaleId,
                        g.MaterialId,
                        g.MaterialName,
                        g.MaterialNameEng,
                        g.VendorId,
                        g.Vendor,
                        g.PONo,
                        g.POType,
                        g.Status,
                        g.PODate,
                        g.PurDollarNameTw,
                        g.PurUnitNameTw,
                        // g.PurQty,

                        // g.OrdersId,
                        // g.OrderNo,
                        // g.StyleNo,
                        g.POVendorETD,
                        // g.VendorETD,
                        g.CompanyId,
                        g.CompanyNo,
                        g.PaymentLocaleId,
                        g.PurLocaleId,
                        g.PaymentLocale,
                        g.PurLocale,
                        g.ReceivedLocaleId,
                        g.ReceivedLocale,
                        // g.PrintCount,
                        // g.PrintTime,
                        g.ModifyUserName,
                        // g.LastUpdateTime,
                        g.UnitPrice,
                        g.DollarId,
                        // g.ReceivedBarcode,
                        // g.Customer,
                    })
                    .Select(i => new Models.Views.POItemForPrint
                    {
                        POId = i.Key.POId,
                        POItemId = i.Max(g => g.POItemId),
                        LocaleId = i.Key.LocaleId,
                        MaterialId = i.Key.MaterialId,
                        MaterialName = i.Key.MaterialName,
                        MaterialNameEng = i.Key.MaterialNameEng,
                        VendorId = i.Key.VendorId,
                        Vendor = i.Key.Vendor,
                        PONo = i.Key.PONo,
                        POType = i.Key.POType,
                        Status = i.Key.Status,

                        PODate = i.Key.PODate,
                        PurDollarNameTw = i.Key.PurDollarNameTw,
                        PurUnitNameTw = i.Key.PurUnitNameTw,
                        PurQty = i.Sum(g => g.PurQty),

                        OrdersId = 0,
                        OrderNo = string.Join(", ", i.Select(g => g.OrderNo).Distinct()),
                        StyleNo = string.Join(", ", i.Select(g => g.StyleNo).Distinct()),//i.Key.StyleNo,
                        POVendorETD = i.Key.POVendorETD,
                        VendorETD = i.Max(g => g.VendorETD),
                        CompanyId = i.Key.CompanyId,
                        CompanyNo = i.Key.CompanyNo,
                        PaymentLocaleId = i.Key.PaymentLocaleId,
                        PurLocaleId = i.Key.PurLocaleId,
                        ReceivedLocaleId = i.Key.ReceivedLocaleId,
                        PaymentLocale = i.Key.PaymentLocale,
                        PurLocale = i.Key.PurLocale,
                        ReceivedLocale = i.Key.ReceivedLocale,
                        // i.Key.PrintCount,
                        // i.Key.PrintTime,
                        ModifyUserName = i.Key.ModifyUserName,
                        LastUpdateTime = i.Max(g => g.LastUpdateTime),
                        UnitPrice = i.Key.UnitPrice,
                        DollarId = i.Key.DollarId,
                        LCSD = i.Min(g => g.LCSD),
                        ReceivedBarcode = string.Join(", ", i.Select(g => g.ReceivedBarcode).Distinct()), //i.Key.ReceivedBarcode
                        Customer = string.Join(", ", i.Select(g => g.Customer).Distinct()), //i.Key.ReceivedBarcode
                    })
                    .ToList();
            }

            items.ForEach(i =>
            {
                i.ReceivedLocale = company.Where(c => c.Id == i.ReceivedLocaleId).Max(c => c.CompanyNo);
                i.PaymentLocale = company.Where(c => c.Id == i.PaymentLocaleId).Max(c => c.CompanyNo);
                i.PurLocaleTitle = company.Where(c => c.Id == i.PurLocaleId).Max(c => c.ChineseName);
                i.PurLocaleAddress = company.Where(c => c.Id == i.PurLocaleId).Max(c => c.ChineseAddress);
                i.PaymentLocaleTitle = company.Where(c => c.Id == i.PaymentLocaleId).Max(c => c.ChineseName);
                i.PaymentLocaleAddress = company.Where(c => c.Id == i.PaymentLocaleId).Max(c => c.ChineseAddress);
            });

            return items.AsQueryable();
        }

        // 來料收貨(拖外)匯入
        public IQueryable<Models.Views.POItem> GetPOForReceived()
        {
            var result = (
                from poi in POItem.Get()
                join po in PO.Get() on new { POId = poi.POId, LocaleId = poi.LocaleId } equals new { POId = po.Id, LocaleId = po.LocaleId }
                join pur in PurBatchItem.Get() on new { POItemId = poi.Id, LocaleId = poi.LocaleId } equals new { POItemId = (decimal)pur.POItemId, LocaleId = pur.LocaleId } into purGRP
                from pur in purGRP.DefaultIfEmpty()
                select new Models.Views.POItem
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
                    VendorNameTw = Vendor.Get().Where(v => v.Id == po.VendorId && v.LocaleId == po.LocaleId).Max(v => v.ShortNameTw),
                    IsAllowPartial = po.IsAllowPartial,
                    VendorETD = po.VendorETD,
                    POItemVendorETD = po.VendorETD,
                    PONo = poi.PONo,
                    RefPONo = po.BatchNo + "-" + po.SeqId.ToString(),

                    Material = Material.Get().Where(m => m.Id == poi.MaterialId && m.LocaleId == poi.LocaleId).Select(m => m.MaterialName).FirstOrDefault(),
                    ParentMaterial = Material.Get().Where(m => m.Id == poi.ParentMaterialId && m.LocaleId == poi.LocaleId).Select(m => m.MaterialName).FirstOrDefault(),

                    Currency = CodeItem.Get().Where(c => c.Id == poi.DollarCodeId && c.LocaleId == poi.LocaleId && c.CodeType == "02").Select(c => c.NameTW).FirstOrDefault(),
                    Unit = CodeItem.Get().Where(c => c.Id == poi.UnitCodeId && c.LocaleId == poi.LocaleId && c.CodeType == "21").Select(c => c.NameTW).FirstOrDefault(),
                    RefOrderNo = poi.OrderNo,
                    ReceivedBarcode = poi.LocaleId.ToString() + "*" + poi.Id.ToString(),
                    PCLUnitCodeId = pur.PlanUnitCodeId,
                    PODate = po.PODate,
                    PlanQty = pur.PlanQty,
                    ReceivedQty = ReceivedLog.Get().Where(i => i.POItemId == poi.Id && i.RefLocaleId == poi.LocaleId && i.TransferInId == 0 && i.IQCResult != 1).Sum(i => i.IQCGetQty),
                    StockQty = ReceivedLog.Get().Where(i => i.POItemId == poi.Id && i.RefLocaleId == poi.LocaleId && i.TransferInId == 0 && i.IQCResult != 1).Sum(i => i.StockQty),
                }
            );
            return result;
        }

    }
}