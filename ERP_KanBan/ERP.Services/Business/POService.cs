using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Diamond.DataSource.Extensions;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Models.Views.Common;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;
using Newtonsoft.Json;

namespace ERP.Services.Business
{
    public class POService : BusinessService
    {
        private ERP.Services.Business.Entities.PurBatchService PurBatch { get; set; }
        private ERP.Services.Business.Entities.PurOrdersItemService PurOrdersItem { get; set; }
        private ERP.Services.Business.Entities.PurBatchItemService PurBatchItem { get; set; }
        private ERP.Services.Entities.MRPItemService MRPItem { get; set; }
        private ERP.Services.Entities.MRPItemOrdersService MRPItemOrders { get; set; }
        private ERP.Services.Business.Entities.POItemService POItem { get; set; }
        private ERP.Services.Business.Entities.POService PO { get; set; }
        private ERP.Services.Business.Entities.POItemSizeService POItemSize { get; set; }
        private ERP.Services.Business.Entities.MaterialStockItemPOService ProcessPO { get; set; }
        private ERP.Services.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Business.Entities.OrdersItemSizeRunService OrdersItemSizeRun { get; set; }
        private ERP.Services.Entities.MaterialService Material { get; set; }
        private ERP.Services.Business.Entities.MaterialQuotService MaterialQuot { get; set; }
        private ERP.Services.Entities.ReceivedLogService ReceivedLog { get; set; }
        private ERP.Services.Entities.VendorService Vendor { get; set; }
        private ERP.Services.Entities.CompanyService Company { get; set; }
        private ERP.Services.Entities.MRPItemUsageService MRPItemUsage { get; set; }
        private Services.Entities.ShipmentLogService ShipmentLog { get; }

        public POService
        (
            ERP.Services.Business.Entities.PurBatchService purBatchService,
            ERP.Services.Business.Entities.PurOrdersItemService purOrdersItemService,
            ERP.Services.Business.Entities.PurBatchItemService purBatchItemService,
            ERP.Services.Entities.MRPItemService mrpItemService,
            ERP.Services.Entities.MRPItemOrdersService mrpItemOrdersService,
            ERP.Services.Business.Entities.POItemService poItemService,
            ERP.Services.Business.Entities.POService poService,
            ERP.Services.Business.Entities.POItemSizeService poItemSizeService,
            ERP.Services.Business.Entities.MaterialStockItemPOService processPOService,
            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Business.Entities.OrdersItemSizeRunService ordersItemSizeRunService,
            ERP.Services.Entities.MaterialService materialService,
            ERP.Services.Business.Entities.MaterialQuotService materialQuotService,
            ERP.Services.Entities.ReceivedLogService receivedLogService,
            ERP.Services.Entities.VendorService vendorService,
            ERP.Services.Entities.CompanyService companyService,
            ERP.Services.Entities.MRPItemUsageService mrpItemUsageService,
            Services.Entities.ShipmentLogService shipmentLogService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            PurBatch = purBatchService;
            PurOrdersItem = purOrdersItemService;
            PurBatchItem = purBatchItemService;
            MRPItem = mrpItemService;
            MRPItemOrders = mrpItemOrdersService;
            Orders = ordersService;
            OrdersItemSizeRun = ordersItemSizeRunService;
            POItem = poItemService;
            PO = poService;
            POItemSize = poItemSizeService;
            ProcessPO = processPOService;
            Material = materialService;
            MaterialQuot = materialQuotService;
            ReceivedLog = receivedLogService;
            Vendor = vendorService;
            Company = companyService;
            MRPItemUsage = mrpItemUsageService;
            ShipmentLog = shipmentLogService;
        }
        public IQueryable<Models.Views.PO> GetWithItem(string predicate)
        {
            var result = POItem.Get()
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Select(i => new Models.Views.PO
                {
                    Id = i.POId,
                    LocaleId = i.LocaleId,
                    BatchNo = i.BatchNo,
                    PODate = (DateTime)i.PODate,
                    PONo = i.PONo,
                    SeqId = i.SeqId,
                    VendorId = i.VendorId,
                    Vendor = i.VendorNameTw,
                    // ModifyUserName = i.ModifyUserName,
                    // LastUpdateTime = (DateTime)i.LastUpdateTime
                })
                .Distinct();
            return result;
        }
        public IQueryable<Models.Views.POItem> GetPOItem()
        {
            return POItem.Get();
        }
        // 採購單刪除的查詢，改成Left Join PO, 處理採購不見的異常情況
        public IQueryable<Models.Views.POItem> GetPOItemForRemove()
        {
            return POItem.GetForRemove();
        }

        //採購單狀態維護 ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
        public IQueryable<Models.Views.POItem> GetClosePOItem(int poId, int localeId)
        {
            // var poItems = POItem.GetPOReceivedLog().Where(i => i.POId == poId && i.LocaleId == localeId).OrderBy(i => i.Material).ToList();
            var poItems = POItem.Get().Where(i => i.POId == poId && i.LocaleId == localeId).OrderBy(i => i.Material).ToList();
            return poItems.AsQueryable();
        }
        public List<Models.Views.POItem> ClosePOItem(List<Models.Views.POItem> items)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                if (items != null || items.Count() > 0)
                {
                    var userName = items.Select(i => i.ModifyUserName).FirstOrDefault();
                    var localeId = items.Select(i => i.LocaleId).FirstOrDefault();

                    var closeIds = items.Where(i => i.Status == 0).Select(i => i.Id);
                    var activeIds = items.Where(i => i.Status == 1).Select(i => i.Id);
                    var cancelIds = items.Where(i => i.Status == 2).Select(i => i.Id);

                    POItem.UpdateStatus((int)localeId, userName, closeIds, activeIds, cancelIds);
                }
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
            }
            return items;
        }
        public List<Models.Views.POItem> AdjustPOItem(List<Models.Views.POItem> items)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                if (items != null || items.Count() > 0)
                {

                    POItem.UpdatePrice(items);
                }
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
            }
            return items;
        }
        // 刪除採購單，特殊權限
        public List<Models.Views.POItem> RemovePOItem(List<Models.Views.POItem> items)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                if (items != null || items.Count() > 0)
                {
                    var localeId = items.Select(i => i.LocaleId).FirstOrDefault();
                    var poItemIds = items.Select(i => i.Id).Distinct().ToArray();
                    var poIds = items.Where(i => i.POType != 1).Select(i => i.POId).Distinct().ToArray();

                    POItem.RemoveRange(i => i.LocaleId == localeId && poItemIds.Contains(i.Id));
                    PO.RemoveRange(i => i.LocaleId == localeId && poIds.Contains(i.Id));
                    PurBatchItem.RemoveRange(i => i.LocaleId == localeId && poItemIds.Contains((decimal)i.POItemId));
                }
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
            }
            return items;
        }
        //採購單狀態維護 ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝

        // 取採購單資料 ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
        public Models.Views.POItemGroup GetPOItemGroup(int id, int localeId)
        {
            var group = new ERP.Models.Views.POItemGroup { };
            var poItem = POItem.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (poItem != null)
            {
                group.PO = PO.Get().Where(i => i.Id == poItem.POId && i.LocaleId == poItem.LocaleId).FirstOrDefault();
                group.POItem = poItem;
                group.POItemSize = POItemSize.Get().Where(i => i.POItemId == poItem.Id && i.LocaleId == poItem.LocaleId).OrderBy(i => i.SeqNo).ToList();
                // group.ProcessPO = poItem.POType == 2 ? ProcessPO.Get().Where(i => i.POItemId == poItem.Id && i.LocaleId == poItem.LocaleId).ToList(): null;
            }

            return group;
        }
        public Models.Views.POGroup GetPOGroup(int poId, int localeId)
        {
            var group = new ERP.Models.Views.POGroup { };
            var po = PO.Get().Where(i => i.Id == poId && i.LocaleId == localeId).FirstOrDefault();

            if (po != null)
            {
                var poItem = POItem.Get().Where(i => i.POId == poId && i.LocaleId == localeId).ToList();

                group.PO = po;
                group.POItem = poItem;
            }

            return group;
        }
        // 取採購單資料 ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝

        public IQueryable<Models.Views.POItemSize> BuildPOItemSize(int poItemId, int localeId, int alternateType)
        {
            var ordersItemSizes = (
                from s in POItem.Get().Where(i => i.Id == poItemId && i.LocaleId == localeId)
                join oi in OrdersItemSizeRun.Get() on new { OrderNo = s.OrderNo } equals new { OrderNo = oi.OrderNo }
                select new Models.Views.POItemSize
                {
                    POItemId = s.Id,
                    LocaleId = s.LocaleId,

                    DisplaySize = alternateType == 1 ? oi.KnifeDisplaySize : alternateType == 2 ? oi.OutsoleDisplaySize : alternateType == 3 ? oi.LastDisplaySize : alternateType == 4 ? oi.ShellDisplaySize :
                                    alternateType == 5 ? oi.Other1Desc : alternateType == 6 ? oi.Other2SizeDesc : oi.DisplaySize,

                    Qty = oi.Qty,
                    SeqNo = oi.ArticleInnerSize,
                    ModifyUserName = s.ModifyUserName,
                    LastUpdateTime = s.LastUpdateTime,
                    PreQty = oi.Qty,

                    ArticleInnerSize = oi.ArticleInnerSize,
                    ArticleDisplaySize = oi.DisplaySize,
                    KnifeDisplaySize = oi.KnifeDisplaySize,
                    OutsoleDisplaySize = oi.OutsoleDisplaySize,
                    LastDisplaySize = oi.LastDisplaySize,
                    ShellDisplaySize = oi.ShellDisplaySize,
                    Other1Desc = oi.Other1Desc,
                    Other2SizeDesc = oi.Other2SizeDesc,
                }
            )
            .ToList();

            var sizeUsages = (
                from s in POItem.Get().Where(i => i.Id == poItemId && i.LocaleId == localeId)
                join u in MRPItemUsage.Get() on new { OrdersId = s.OrdersId, LocaleId = s.LocaleId, MaterialId = s.MaterialId, ParentMaterialId = s.ParentMaterialId } equals new { OrdersId = u.OrdersId, LocaleId = u.LocaleId, MaterialId = u.MaterialId, ParentMaterialId = u.ParentMaterialId }
                select new
                {
                    POItemId = s.Id,
                    LocaleId = s.LocaleId,
                    ModifyUserName = s.ModifyUserName,
                    LastUpdateTime = s.LastUpdateTime,
                    MaterialId = s.MaterialId,
                    ParentMaterialId = s.ParentMaterialId,
                    u.SizeUsage,
                }
            ).ToList();

            var mrpUsages = new List<Models.Views.POItemSizeUsage>();

            sizeUsages.ForEach(s =>
            {
                var items = JsonConvert.DeserializeObject<List<Models.Entities.MRPItemSizeUsage>>(s.SizeUsage);
                items.ForEach(i =>
                {
                    mrpUsages.Add(new Models.Views.POItemSizeUsage
                    {
                        Id = s.POItemId,
                        POItemId = s.POItemId,
                        LocaleId = s.LocaleId,
                        MaterialId = s.MaterialId,
                        ParentMaterialId = s.ParentMaterialId,
                        ArticleSize = i.B,
                        ArticleSizeSuffix = i.C,
                        ArticleInnerSize = i.D,
                        Usage = i.F > 0 ? 1 : 0,
                        Qty = i.G,
                    });
                });
            });

            var poItemSizes = (
                from pu in ordersItemSizes
                join mu in mrpUsages.Where(i => i.Usage > 0) on new { POItemId = pu.POItemId, LocaleId = pu.LocaleId, ArticleInnerSize = pu.ArticleInnerSize } equals new { POItemId = mu.POItemId, LocaleId = mu.LocaleId, ArticleInnerSize = mu.ArticleInnerSize }
                select new Models.Views.POItemSize
                {
                    POItemId = pu.POItemId,
                    LocaleId = pu.LocaleId,
                    ModifyUserName = pu.ModifyUserName,
                    LastUpdateTime = pu.LastUpdateTime,

                    DisplaySize = pu.DisplaySize,
                    Qty = pu.Qty,
                    PreQty = pu.Qty,
                    SeqNo = pu.ArticleInnerSize,

                    ArticleInnerSize = pu.ArticleInnerSize,
                    ArticleDisplaySize = pu.DisplaySize,
                    KnifeDisplaySize = pu.KnifeDisplaySize,
                    OutsoleDisplaySize = pu.OutsoleDisplaySize,
                    LastDisplaySize = pu.LastDisplaySize,
                    ShellDisplaySize = pu.ShellDisplaySize,
                    Other1Desc = pu.Other1Desc,
                    Other2SizeDesc = pu.Other2SizeDesc,
                }
            ).ToList();

            return poItemSizes.AsQueryable();
        }
        // public IQueryable<Models.Views.POItemSize> BuildPOItemSize1(int poItemId, int localeId, int alternateType)
        // {
        //     var poItemSizes = (
        //         from s in POItem.Get().Where(i => i.Id == poItemId && i.LocaleId == localeId)
        //         join oi in OrdersItemSizeRun.Get() on new { OrderNo = s.OrderNo } equals new { OrderNo = oi.OrderNo }
        //         select new Models.Views.POItemSize
        //         {
        //             POItemId = s.Id,
        //             LocaleId = s.LocaleId,
        //             DisplaySize = alternateType == 1 ? oi.KnifeDisplaySize : alternateType == 2 ? oi.OutsoleDisplaySize : alternateType == 3 ? oi.LastDisplaySize : alternateType == 4 ? oi.ShellDisplaySize :
        //                             alternateType == 5 ? oi.Other1Desc : alternateType == 6 ? oi.Other2SizeDesc : oi.DisplaySize,
        //             Qty = oi.Qty,
        //             SeqNo = oi.ArticleInnerSize,
        //             ModifyUserName = s.ModifyUserName,
        //             LastUpdateTime = s.LastUpdateTime,
        //             PreQty = oi.Qty,
        //         }
        //     ).ToList();
        //     return poItemSizes.AsQueryable();
        // }
        public IQueryable<Models.Views.POItemSize> BuildPOItemSizeByMRPUsage(int ordersId, int localeId, int mId, int pmId, int alternateType)
        {
            var ordersItemSizes = (
                from oi in OrdersItemSizeRun.Get().Where(i => i.OrdersId == ordersId && i.LocaleId == localeId)
                select new Models.Views.POItemSize
                {
                    POItemId = 0,
                    LocaleId = 0,

                    DisplaySize = alternateType == 1 ? oi.KnifeDisplaySize : alternateType == 2 ? oi.OutsoleDisplaySize : alternateType == 3 ? oi.LastDisplaySize : alternateType == 4 ? oi.ShellDisplaySize :
                                    alternateType == 5 ? oi.Other1Desc : alternateType == 6 ? oi.Other2SizeDesc : oi.DisplaySize,

                    Qty = oi.Qty,
                    SeqNo = oi.ArticleInnerSize,
                    // ModifyUserName = s.ModifyUserName,
                    // LastUpdateTime = s.LastUpdateTime,
                    PreQty = oi.Qty,

                    ArticleInnerSize = oi.ArticleInnerSize,
                    ArticleDisplaySize = oi.DisplaySize,
                    KnifeDisplaySize = oi.KnifeDisplaySize,
                    OutsoleDisplaySize = oi.OutsoleDisplaySize,
                    LastDisplaySize = oi.LastDisplaySize,
                    ShellDisplaySize = oi.ShellDisplaySize,
                    Other1Desc = oi.Other1Desc,
                    Other2SizeDesc = oi.Other2SizeDesc,
                }
            )
            .ToList();

            var sizeUsages = (
                from u in MRPItemUsage.Get().Where(i => i.OrdersId == ordersId && i.LocaleId == localeId && i.MaterialId == mId && i.ParentMaterialId == pmId)
                select new
                {
                    MaterialId = mId,
                    ParentMaterialId = pmId,
                    u.SizeUsage,
                }
            ).ToList();

            var mrpUsages = new List<Models.Views.POItemSizeUsage>();

            sizeUsages.ForEach(s =>
            {
                var items = JsonConvert.DeserializeObject<List<Models.Entities.MRPItemSizeUsage>>(s.SizeUsage);
                items.ForEach(i =>
                {
                    mrpUsages.Add(new Models.Views.POItemSizeUsage
                    {
                        Id = 0,
                        POItemId = 0,
                        LocaleId = localeId,
                        MaterialId = s.MaterialId,
                        ParentMaterialId = s.ParentMaterialId,
                        ArticleSize = i.B,
                        ArticleSizeSuffix = i.C,
                        ArticleInnerSize = i.D,
                        Usage = i.F,
                        Qty = i.G,
                    });
                });
            });

            var poItemSizes = (
                from pu in ordersItemSizes
                join mu in mrpUsages.Where(i => i.Usage > 0) on new { ArticleInnerSize = pu.ArticleInnerSize } equals new { ArticleInnerSize = mu.ArticleInnerSize }
                select new Models.Views.POItemSize
                {
                    POItemId = pu.POItemId,
                    LocaleId = pu.LocaleId,
                    ModifyUserName = pu.ModifyUserName,
                    LastUpdateTime = pu.LastUpdateTime,

                    DisplaySize = pu.DisplaySize,
                    // Qty = pu.Qty,
                    // PreQty = pu.Qty,
                    Qty = (decimal)(pu.Qty * mu.Usage),
                    PreQty = (decimal)(pu.Qty * mu.Usage),
                    SeqNo = pu.ArticleInnerSize,

                    ArticleInnerSize = pu.ArticleInnerSize,
                    ArticleDisplaySize = pu.DisplaySize,
                    KnifeDisplaySize = pu.KnifeDisplaySize,
                    OutsoleDisplaySize = pu.OutsoleDisplaySize,
                    LastDisplaySize = pu.LastDisplaySize,
                    ShellDisplaySize = pu.ShellDisplaySize,
                    Other1Desc = pu.Other1Desc,
                    Other2SizeDesc = pu.Other2SizeDesc,
                }
            ).ToList();

            return poItemSizes.AsQueryable();
        }
        public IQueryable<Models.Views.POItemSize> BuildPOItemSizeByOrderQty(string orderNo, int alternateType)
        {
            var poItemSizes = (
                from oi in OrdersItemSizeRun.Get().Where(i => i.OrderNo == orderNo)
                select new Models.Views.POItemSize
                {
                    POItemId = 0,
                    LocaleId = 0,
                    DisplaySize = alternateType == 1 ? oi.KnifeDisplaySize : alternateType == 2 ? oi.OutsoleDisplaySize : alternateType == 3 ? oi.LastDisplaySize : alternateType == 4 ? oi.ShellDisplaySize :
                                    alternateType == 5 ? oi.Other1Desc : alternateType == 6 ? oi.Other2SizeDesc : oi.DisplaySize,
                    Qty = oi.Qty,
                    SeqNo = oi.ArticleInnerSize,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,
                    PreQty = oi.Qty,
                }
            ).ToList();
            return poItemSizes.AsQueryable();
        }


        // 採購單異動，新增、儲存 ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
        // 批次採購的單筆處理
        public ERP.Models.Views.POItemGroup SavePOItemGroup(POItemGroup group)
        {
            try
            {
                var po = group.PO;
                var poItem = group.POItem;
                var poItemSize = group.POItemSize;

                UnitOfWork.BeginTransaction();
                if (poItem != null)
                {
                    {
                        var _po = PO.Get().Where(i => i.LocaleId == po.LocaleId && i.Id == po.Id).FirstOrDefault();
                        var _poItem = POItem.Get().Where(i => i.LocaleId == poItem.LocaleId && i.Id == poItem.Id).FirstOrDefault();
                        var _purBatchItem = PurBatchItem.Get().Where(i => i.LocaleId == poItem.LocaleId && i.POItemId == poItem.Id).FirstOrDefault();

                        if (_po == null || _poItem == null)
                        {
                            throw null;
                        }
                        else
                        {
                            poItem.CompanyId = poItem.CompanyId == null ? poItem.LocaleId : poItem.CompanyId;

                            poItem = POItem.Update(poItem);
                            po = PO.Update(po);

                            POItemSize.RemoveRange(i => i.POItemId == poItem.Id && i.LocaleId == poItem.LocaleId);
                            POItemSize.CreateRange(poItemSize);

                            if (_purBatchItem != null)
                            {
                                _purBatchItem.PurQty = poItem.Qty;
                                _purBatchItem.Status = poItem.Status;
                                _purBatchItem.PlanUnitCodeId = _purBatchItem.PlanUnitCodeId == 0 ? poItem.UnitCodeId : _purBatchItem.PlanUnitCodeId;
                                _purBatchItem.PayDollarCodeId = poItem.PayDollarCodeId;
                                _purBatchItem.PurUnitPrice = (decimal)poItem.UnitPrice;

                                PurBatchItem.Update(_purBatchItem);
                            }
                        }
                    }
                }
                UnitOfWork.Commit();
                return this.GetPOItemGroup((int)poItem.Id, (int)poItem.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        // 其他採購 單筆/拖外/補單採購
        public ERP.Models.Views.POItemGroup SaveOthersPOItemGroup(POItemGroup group)
        {
            try
            {
                var po = group.PO;
                var poItem = group.POItem;
                var poItemSize = group.POItemSize.ToList();

                UnitOfWork.BeginTransaction();
                if (poItem != null)
                {
                    var planQty = poItem.PlanQty;
                    var remark = "※交貨之品質不符或數量不足交貨期延誤，而影響本公司生產時，售方應負擔一切損失。\n※交貨時請於簽單及材料包裝上註明採購單號及管制編號，請款時務必列明並附上採購單。\n※送貨時間請於上班時間內(週一到週五，上午8點~12點與下午1點~5點。\n※禁含致癌AZO偶氮染料.鉛等八大重金屬致癌物質。\n※提前交貨應事先徵得我方之書面同意，否則將拒絕收貨。";
                    // remark = remark + "\t※交貨地點：" + ReceivingLocale.ChineseAddress + "\tTEL：" + ReceivingLocale.TelNo + "\tINVOICE 抬頭：" +  ReceivingLocale.InvoiceTitle + "\tINVOICE 地址：" +  ReceivingLocale.InvoiceAddress;

                    //整理資料
                    {
                        poItem.OrderNo = poItem.OrdersId == 0 ? "" : poItem.OrderNo;
                        poItem.StyleNo = poItem.OrdersId == 0 ? "" : poItem.StyleNo;
                    }

                    if (poItem.Id > 0)  // for Update
                    {
                        var _po = PO.Get().Where(i => i.LocaleId == po.LocaleId && i.Id == po.Id).FirstOrDefault();
                        var _poItem = POItem.Get().Where(i => i.LocaleId == poItem.LocaleId && i.Id == poItem.Id).FirstOrDefault();

                        if (_po == null || _poItem == null)
                        {
                            throw null;
                        }
                        else
                        {
                            po.Remark = remark;
                            po.IsShowSizeRun = poItemSize.Count() > 0 ? 1 : 0; // 是否為有Size訂單

                            poItem.PONo = po.PONo;
                            poItem = POItem.Update(poItem);
                            po = PO.Update(po);

                            poItemSize.ForEach(i =>
                            {
                                i.POItemId = poItem.Id;
                                i.LocaleId = poItem.LocaleId;
                                i.ModifyUserName = poItem.ModifyUserName;
                                i.LastUpdateTime = poItem.LastUpdateTime;
                            });

                            POItemSize.RemoveRange(i => i.POItemId == poItem.Id && i.LocaleId == poItem.LocaleId);
                            POItemSize.CreateRange(poItemSize);
                        }
                    }
                    else // for Inster
                    {
                        // step 0 : 分辨單號前墜，補單＝A，單筆＝S
                        var poTypeStr = poItem.POType == 5 ? "A" : poItem.POType == 2 ? "OP" : poItem.POType == 6 ? "AOP" : "S";
                        var _batch = poTypeStr + poItem.LocaleId + DateTime.Now.ToString("yyyyMMdd");

                        // step 1: 取得該批次最大的序號
                        var items = PO.Get().Where(i => i.BatchNo == _batch && i.LocaleId == poItem.LocaleId).Select(i => i.SeqId).ToList();
                        var seqId = items.Count() == 0 ? 0 : items.Max();

                        // step 2: 生成PO, 序號為自動編號(S+LocaleId+yyMMdd)，從批號中最大的加1，
                        var _po = new Models.Views.PO
                        {
                            LocaleId = poItem.LocaleId,
                            PODate = DateTime.Now.Date,
                            BatchNo = _batch,
                            SeqId = seqId + 1,
                            VendorId = poItem.VendorId,
                            Remark = remark,
                            ModifyUserName = poItem.ModifyUserName,
                            POTeam = poItem.POTeam,
                            IsAllowPartial = 1,
                            IsShowSizeRun = poItemSize.Count() > 0 ? 1 : 0,  // 是否為有Size訂單
                        };

                        // step 3:新增PO
                        po = PO.Create(_po);

                        // step 4: 新增後PO後，把POId更新到表身，POItem裡
                        poItem.POId = po.Id;
                        poItem.PONo = po.BatchNo + "-" + po.SeqId.ToString();

                        // step 5: add POItem
                        poItem = POItem.Create(poItem);

                        poItemSize.ForEach(i =>
                        {
                            i.POItemId = poItem.Id;
                            i.LocaleId = poItem.LocaleId;
                            i.ModifyUserName = poItem.ModifyUserName;
                            i.LastUpdateTime = poItem.LastUpdateTime;
                        });

                        POItemSize.RemoveRange(i => i.POItemId == poItem.Id && i.LocaleId == poItem.LocaleId);
                        POItemSize.CreateRange(poItemSize);
                    }

                    // 再這裡生成PurBatchItem是要正確的POItemId
                    var purBatchItem = new PurBatchItem
                    {
                        //Id = i.Id,
                        LocaleId = poItem.LocaleId,
                        BatchId = 0,
                        OrdersId = poItem.OrdersId,
                        MaterialId = poItem.MaterialId,
                        PlanUnitCodeId = poItem.UnitCodeId,
                        PlanQty = planQty ?? 0,  // poitem經過更新後，以沒有暫存的PlanQty
                        RefQuotId = 0,
                        VendorId = poItem.VendorId,
                        PurUnitPrice = (decimal)poItem.UnitPrice,
                        DollarCodeId = poItem.DollarCodeId,
                        PayCodeId = poItem.PayCodeId,
                        PurUnitCodeId = poItem.UnitCodeId,
                        PurQty = poItem.Qty,
                        PurLocaleId = poItem.PurLocaleId,
                        ReceivingLocaleId = poItem.ReceivingLocaleId,
                        PaymentLocaleId = poItem.PaymentLocaleId,
                        POItemId = poItem.Id,
                        OnHandQty = 0,
                        ParentMaterialId = poItem.ParentMaterialId,
                        ModifyUserName = poItem.ModifyUserName,
                        LastUpdateTime = poItem.LastUpdateTime,
                        PayDollarCodeId = poItem.PayDollarCodeId,
                        RefLocaleId = poItem.LocaleId,
                        RefItemId = 0,
                        AlternateType = poItem.AlternateType,
                    };
                    // 另外補流程 PurBatch 一律刪掉新增(因為如果有重複的，可以做除裡)

                    PurBatchItem.RemoveRange(i => i.LocaleId == poItem.LocaleId && i.POItemId == poItem.Id);
                    purBatchItem = PurBatchItem.Create(purBatchItem);

                    if (purBatchItem != null && purBatchItem.Id > 0)
                    {
                        // 回補poItem
                        poItem.PurBatchItemId = purBatchItem.Id;
                        POItem.Update(poItem);
                        // POItem.UpdatePart(poItem); // 只更新PurbatchItem
                    }

                }
                UnitOfWork.Commit();
                return this.GetPOItemGroup((int)poItem.Id, (int)poItem.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        // 其他採購的批量採購
        public IQueryable<ERP.Models.Views.POItem> SaveBatchPOItem(List<Models.Views.POItem> items)
        {

            if (items.Count() > 0)
            {
                UnitOfWork.BeginTransaction();
                try
                {

                    var remark = "※交貨之品質不符或數量不足交貨期延誤，而影響本公司生產時，售方應負擔一切損失。\n※交貨時請於簽單及材料包裝上註明採購單號及管制編號，請款時務必列明並附上採購單。\n※送貨時間請於上班時間內(週一到週五，上午8點~12點與下午1點~5點。\n※禁含致癌AZO偶氮染料.鉛等八大重金屬致癌物質。\n※提前交貨應事先徵得我方之書面同意，否則將拒絕收貨。";

                    var localeId = items[0].LocaleId;
                    var poItems = items.Select(i => new Models.Views.POItem
                    {
                        Id = (decimal)i.Id,
                        LocaleId = i.LocaleId,
                        POId = (decimal)i.POId,
                        OrdersId = i.OrdersId,
                        OrderNo = i.OrderNo,
                        StyleNo = i.StyleNo,
                        POType = i.POType,
                        MaterialId = i.MaterialId,
                        UnitPrice = i.UnitPrice, // 更改每次儲存都更新最新的單價。i.UnitPrice,
                        DollarCodeId = i.PayDollarCodeId, // 更改每次儲存都更新最新的單價幣別 i.DollarCodeId,
                        UnitCodeId = i.UnitCodeId,   // 改每次儲存都更新最的單價幣別i.UnistCodeId,
                        Qty = (decimal)i.Qty,
                        PayCodeId = (int)i.PayCodeId,
                        PurLocaleId = i.PurLocaleId,
                        ReceivingLocaleId = i.ReceivingLocaleId,
                        PaymentLocaleId = i.PaymentLocaleId,
                        PaymentCodeId = i.PaymentCodeId,
                        PaymentPoint = i.PaymentPoint,
                        ParentMaterialId = i.ParentMaterialId,
                        IsOverQty = i.IsOverQty == null ? 0 : (int)i.IsOverQty,
                        SamplingMethod = (int)i.SamplingMethod,
                        ModifyUserName = i.ModifyUserName,
                        PayDollarCodeId = i.PayDollarCodeId,
                        Status = i.Status == null ? 0 : (int)i.Status,
                        FactoryETD = i.FactoryETD == null ? DateTime.Now.Date : (DateTime)i.FactoryETD,
                        Remark = i.Remark,
                        CompanyId = i.CompanyId == 0 ? i.LocaleId : i.CompanyId,
                        AlternateType = i.IsShowSizeRun == 1 ? 0 : i.AlternateType,
                        IsShowSizeRun = i.IsShowSizeRun,

                        VendorId = i.VendorId,
                        IsAllowPartial = 0,
                        PurBatchItemId = i.PurBatchItemId,
                        PONo = i.PONo,
                        POTeam = i.POTeam,
                        PlanQty = i.PlanQty,
                    }).ToList();
                    var purBatchItems = new List<Models.Views.PurBatchItem>();

                    // have poId can update only, not have POId add
                    var addItems = poItems.Where(i => i.Id == 0 || i.Id == null).ToList();
                    var updateItems = poItems.Where(i => i.Id > 0).ToList();

                    // 1==================================================================================================================================
                    // 已經有採購單的，所以把Item裡有POItemId的取出 >> updateItems start,
                    // update exsit items
                    // step1, update POItem
                    // step2, group by PO Id and update by poid
                    if (updateItems.Count() > 0)
                    {
                        POItem.UpdateRange(updateItems);
                        updateItems.ForEach(i =>
                        {
                            var purBatchItem = new Models.Views.PurBatchItem
                            {
                                //Id = i.Id,
                                LocaleId = i.LocaleId,
                                BatchId = 0,
                                OrdersId = i.OrdersId,
                                MaterialId = i.MaterialId,
                                PlanUnitCodeId = i.UnitCodeId,
                                PlanQty = i.PlanQty ?? 0,
                                RefQuotId = 0,
                                VendorId = i.VendorId,
                                PurUnitPrice = (decimal)i.UnitPrice,
                                DollarCodeId = i.DollarCodeId,
                                PayCodeId = i.PayCodeId,
                                PurUnitCodeId = i.UnitCodeId,
                                PurQty = i.Qty,
                                PurLocaleId = i.PurLocaleId,
                                ReceivingLocaleId = i.ReceivingLocaleId,
                                PaymentLocaleId = i.PaymentLocaleId,
                                POItemId = i.Id,
                                OnHandQty = 0,
                                ParentMaterialId = i.ParentMaterialId,
                                ModifyUserName = i.ModifyUserName,
                                LastUpdateTime = i.LastUpdateTime,
                                PayDollarCodeId = i.PayDollarCodeId,
                                RefLocaleId = i.LocaleId,
                                RefItemId = 0,
                                AlternateType = i.AlternateType,
                                PONo = i.PONo,
                                Status = i.Status,
                                OrderNo = i.OrderNo,
                            };
                            purBatchItems.Add(purBatchItem);
                        });

                    }
                    // 已經有採購單的(沒收貨前可以更新數量等資料)，所以把Item裡有POItemId的取出 >> updateItems end,

                    // 2==================================================================================================================================
                    // 沒有採購單的就可以新增，把沒有POItemId的分開取出 >> addItems start,
                    // add new items
                    // step 1: get max seqId from Batch No, 取得該批次最大的序號
                    // step 2: distinct Materail, ParentMaterialId, Vendor to Seq List, 相同的材料、廠商都為同一個序號
                    // step 3: add PO
                    // step 4: 新增後PO後，把POId更新到表身，POItem裡
                    // step 5: 新增POItem,
                    if (addItems.Count() > 0)
                    {
                        var pos = new List<Models.Views.PO>();

                        // step 1: 取得該批次最大的序號
                        // addItems[0].POType, 同一批只能作業同一類型採購
                        var poTypeStr = addItems[0].POType == 5 ? "A" : addItems[0].POType == 2 ? "OP" : addItems[0].POType == 6 ? "AOP" : "S";
                        var _batch = poTypeStr + localeId + DateTime.Now.ToString("yyyyMMdd");

                        // step 1: 取得該批次最大的序號
                        var _seq = PO.Get().Where(i => i.BatchNo == _batch && i.LocaleId == localeId).Select(i => i.SeqId).ToList();
                        var seqId = _seq.Count() == 0 ? 0 : _seq.Max();


                        // step 2: 生成所有的PO, 從item中把相同材料跟廠商的取出當表頭，序號為自動編號，從批號中最大的加1，
                        // var poGroups = addItems.Select(p => new { p.MaterialId, p.ParentMaterialId, p.VendorId, p.LocaleId, p.ModifyUserName, p.POTeam, p.IsShowSizeRun }).Distinct().ToList();

                        addItems.ForEach(p =>
                        {
                            seqId += 1;

                            var po = new Models.Views.PO
                            {
                                LocaleId = p.LocaleId,
                                PODate = DateTime.Now.Date,
                                BatchNo = _batch,
                                SeqId = seqId,
                                VendorId = p.VendorId,
                                Remark = remark,
                                ModifyUserName = p.ModifyUserName,
                                POTeam = p.POTeam,
                                IsAllowPartial = 1,
                                IsShowSizeRun = (int)p.IsShowSizeRun,
                            };
                            pos.Add(po);

                            //更新POItem的PONo
                            p.SeqId = seqId;
                            p.PONo = _batch + "-" + seqId.ToString();

                            //新增PurBatchItem的PONo
                            var purBatchItem = new Models.Views.PurBatchItem
                            {
                                //Id = i.Id,
                                LocaleId = p.LocaleId,
                                BatchId = 0,
                                OrdersId = p.OrdersId,
                                MaterialId = p.MaterialId,
                                PlanUnitCodeId = p.UnitCodeId,
                                PlanQty = p.PlanQty ?? 0,
                                RefQuotId = 0,
                                VendorId = p.VendorId,
                                PurUnitPrice = (decimal)p.UnitPrice,
                                DollarCodeId = p.DollarCodeId,
                                PayCodeId = p.PayCodeId,
                                PurUnitCodeId = p.UnitCodeId,
                                PurQty = p.Qty,
                                PurLocaleId = p.PurLocaleId,
                                ReceivingLocaleId = p.ReceivingLocaleId,
                                PaymentLocaleId = p.PaymentLocaleId,
                                POItemId = p.Id,
                                OnHandQty = 0,
                                ParentMaterialId = p.ParentMaterialId,
                                ModifyUserName = p.ModifyUserName,
                                LastUpdateTime = p.LastUpdateTime,
                                PayDollarCodeId = p.PayDollarCodeId,
                                RefLocaleId = p.LocaleId,
                                RefItemId = 0,
                                AlternateType = p.AlternateType,
                                PONo = _batch + "-" + seqId.ToString(),
                                Status = p.Status,
                                OrderNo = p.OrderNo,
                            };
                            purBatchItems.Add(purBatchItem);
                        });

                        // step 3:新增PO
                        PO.CreateRange(pos);

                        // step 4: 新增後PO後，把POId更新到表身，POItem裡
                        var poNos = PO.Get().Where(p => p.BatchNo == _batch && p.LocaleId == localeId).Select(p => new { p.Id, p.LocaleId, PONo = p.BatchNo + "-" + p.SeqId.ToString() }).ToList();

                        addItems.ForEach(i =>
                        {
                            var po = poNos.Where(p => p.PONo == i.PONo).FirstOrDefault();
                            if (po != null)
                            {
                                i.POId = po.Id;
                            }
                        });
                        // step 5: add POItem
                        POItem.CreateRange(addItems);
                    }
                    // 沒有採購單的就可以新增，把沒有POItemId的分開取出 >> addItems end,

                    // 3==================================================================================================================================
                    // 把更新跟新增的POItem 整合在一起 start
                    var updatePONos = updateItems.Select(i => i.PONo).Distinct();
                    var addPONos = addItems.Select(i => i.PONo).Distinct();
                    var allPONos = updatePONos.Union(addPONos).Distinct().ToArray();
                    var allPOItems = POItem.Get().Where(p => allPONos.Contains(p.PONo) && p.LocaleId == localeId).ToList();

                    var poItemShowSizes = allPOItems.Where(i => i.IsShowSizeRun == 1).ToList();
                    var poItemNoSizes = allPOItems.Where(i => i.IsShowSizeRun == 0).ToList();
                    // 把更新跟新增的POItem 整合在一起 end。
                    // 4==================================================================================================================================
                    // 刪除所有的 POItemSizeRun後，再新增有SizeRun的訂單，同時更新PO的IsShowSizeRun Start
                    // 把所有要新增的POItemSize的訂單SizeRun都找出來
                    var orderNos = poItems.Where(i => i.IsShowSizeRun == 1).Select(i => i.OrderNo).Distinct();
                    var ordersItems = OrdersItemSizeRun.Get().Where(i => orderNos.Contains(i.OrderNo)).ToList();

                    // 新增，size run參考管制表的用量
                    var ordersIds = poItems.Where(i => i.IsShowSizeRun == 1).Select(i => i.OrdersId).Distinct().ToList();
                    var materialIds = poItems.Where(i => i.IsShowSizeRun == 1).Select(i => i.MaterialId).Distinct().ToList();
                    var mItems = MRPItemUsage.Get().Where(i => i.LocaleId == localeId && ordersIds.Contains(i.OrdersId) && materialIds.Contains(i.MaterialId))
                                  .Select(i => new
                                  {
                                      i.OrdersId,
                                      i.LocaleId,
                                      i.MaterialId,
                                      i.ParentMaterialId,
                                      i.SizeUsage
                                  }).ToList();
                    var mrpUsages = new List<Models.Views.POItemSizeUsage>();
                    mItems.ForEach(s =>
                    {
                        var items = JsonConvert.DeserializeObject<List<Models.Entities.MRPItemSizeUsage>>(s.SizeUsage);
                        items.ForEach(i =>
                        {
                            mrpUsages.Add(new Models.Views.POItemSizeUsage
                            {
                                OrdersId = s.OrdersId,
                                LocaleId = s.LocaleId,
                                MaterialId = s.MaterialId,
                                ParentMaterialId = s.ParentMaterialId,
                                ArticleSize = i.B,
                                ArticleSizeSuffix = i.C,
                                ArticleInnerSize = i.D,
                                // Usage = i.F > 0 ? 1 : 0,
                                Usage = i.F,
                                Qty = i.G,
                            });
                        });
                    });

                    // 5==================================================================================================================================
                    // 刪除所有POItemId 的size run
                    var poItemIds = poItems.Select(i => i.Id).Distinct();
                    POItemSize.RemoveRange(i => poItemIds.Contains(i.POItemId) && i.LocaleId == localeId);

                    // 生成POItemSize
                    var poItemSizes = (
                        from s in poItemShowSizes
                        join oi in ordersItems on new { OrderNo = s.OrderNo } equals new { OrderNo = oi.OrderNo }
                        select new Models.Views.POItemSize
                        {
                            POItemId = s.Id,
                            LocaleId = s.LocaleId,
                            DisplaySize = s.AlternateType == 1 ? oi.KnifeDisplaySize : s.AlternateType == 2 ? oi.OutsoleDisplaySize : s.AlternateType == 3 ? oi.LastDisplaySize : s.AlternateType == 4 ? oi.ShellDisplaySize :
                                            s.AlternateType == 5 ? oi.Other1Desc : s.AlternateType == 6 ? oi.Other2SizeDesc : oi.DisplaySize,
                            Qty = oi.Qty,
                            SeqNo = oi.ArticleInnerSize,
                            ModifyUserName = s.ModifyUserName,
                            LastUpdateTime = s.LastUpdateTime,
                            PreQty = oi.Qty,

                            ArticleInnerSize = oi.ArticleInnerSize,
                            OrdersId = oi.OrdersId,
                            MaterialId = s.MaterialId,
                            ParentMaterialId = s.ParentMaterialId,
                        }
                    )
                    .ToList();

                    //排除沒有用量的Size
                    var withOutZero = (
                        from p in poItemSizes.AsQueryable()
                        join ms in mrpUsages.Where(i => i.Usage > 0) on new { OrdersId = p.OrdersId, LocaleId = p.LocaleId, MateriaId = p.MaterialId, ParentMaterialId = p.ParentMaterialId, ArticleInnerSize = p.ArticleInnerSize } equals
                                                                        new { OrdersId = ms.OrdersId, LocaleId = ms.LocaleId, MateriaId = (decimal?)ms.MaterialId, ParentMaterialId = ms.ParentMaterialId, ArticleInnerSize = ms.ArticleInnerSize }
                        select new Models.Views.POItemSize
                        {
                            POItemId = p.POItemId,
                            LocaleId = p.LocaleId,
                            DisplaySize = p.DisplaySize,

                            // Qty = p.Qty,
                            Qty = (decimal)(p.Qty * ms.Usage),
                            SeqNo = p.ArticleInnerSize,
                            ModifyUserName = p.ModifyUserName,
                            LastUpdateTime = p.LastUpdateTime,
                            // PreQty = p.Qty,
                            PreQty = (decimal)(p.Qty * ms.Usage),

                            ArticleInnerSize = p.ArticleInnerSize,
                            OrdersId = p.OrdersId,
                            MaterialId = p.MaterialId,
                            ParentMaterialId = p.ParentMaterialId,
                        }
                    ).ToList();
                    // Inster Into POItemSzie
                    POItemSize.CreateRange(withOutZero);
                    // 5==================================================================================================================================

                    // 刪除所有的 POItemSizeRun後，再新增有SizeRun的訂單，同時更新PO的IsShowSizeRun Start
                    // 6==================================================================================================================================
                    // 更新PO的IsShowSizeRun start
                    // update PO, get POIds, update POTeam, ModifyUserName, Remark, IsShowSizeRun = 1; Purchase POItem by SizeRun; have POItemSize
                    if (poItemShowSizes.Any())
                    {
                        var poIds = poItemShowSizes.Select(i => i.POId).Distinct();
                        PO.UpdateRange((int)localeId, poIds, new Models.Views.PO { POTeam = allPOItems[0].POTeam, ModifyUserName = allPOItems[0].ModifyUserName, IsShowSizeRun = 1 });
                    }
                    // update PO, get POIds, update POTeam, ModifyUserName, Remark, IsShowSizeRun = 0; Purchase POItem without by SizeRun; not have POItemSize
                    if (poItemNoSizes.Any())
                    {
                        var poIdsNoSizes = poItemNoSizes.Select(i => i.POId).Distinct();
                        PO.UpdateRange((int)localeId, poIdsNoSizes, new Models.Views.PO { POTeam = allPOItems[0].POTeam, ModifyUserName = allPOItems[0].ModifyUserName, IsShowSizeRun = 0 });
                    }
                    // 更新PO的IsShowSizeRun end
                    // ==================================================================================================================================

                    // PurBatchItems ========start

                    var allPOItemIds = allPOItems.Select(i => (decimal?)i.Id).Distinct().ToList();
                    purBatchItems.Where(i => i.POItemId == 0).ToList().ForEach(i =>
                    {
                        i.POItemId = allPOItems.Where(pi => pi.PONo == i.PONo && pi.LocaleId == i.LocaleId).Select(i => i.Id).FirstOrDefault();
                    });
                    PurBatchItem.RemoveRange(i => i.LocaleId == localeId && allPOItemIds.Contains(i.POItemId));
                    PurBatchItem.CreateRange(purBatchItems);

                    var _purBatchItem = PurBatchItem.Get().Where(i => i.LocaleId == localeId && allPOItemIds.Contains(i.POItemId)).Select(i => new { i.Id, i.POItemId, i.LocaleId }).ToList();

                    // var _allPOItems = POItem.GetEntity().Where(p => addPONos.Contains(p.PONo) && p.LocaleId == localeId).ToList();
                    allPOItems.ForEach(i =>
                    {
                        i.PurBatchItemId = _purBatchItem.Where(p => p.POItemId == i.Id && p.LocaleId == i.LocaleId).Max(i => i.Id);
                        POItem.Update(i);   // 更新POItem 的PurBatchItemId, 用逐筆Update的才能避免POItem 
                    });

                    // POItemUpdate.UpdateRange(allPOItems);
                    // PurBatchItems ========end
                    UnitOfWork.Commit();

                    items = POItem.Get().Where(i => i.LocaleId == localeId && allPOItemIds.Contains(i.Id)).ToList();
                }
                catch (Exception e)
                {
                    UnitOfWork.Rollback();
                    throw e;
                }
            }

            return items.AsQueryable();
        }
        // 整個採購單
        public ERP.Models.Views.POGroup SavePOGroup(POGroup group)
        {
            try
            {
                var po = group.PO;
                var poItem = group.POItem.ToList();

                UnitOfWork.BeginTransaction();

                if (po != null)
                {
                    var _po = PO.Get().Where(i => i.LocaleId == po.LocaleId && i.Id == po.Id).FirstOrDefault();
                    if (_po != null)
                    {
                        po.Id = _po.Id;
                        po.LocaleId = _po.LocaleId;
                        po = PO.Update(po);

                        poItem.ForEach(i =>
                        {
                            i.PONo = po.BatchNo + "-" + po.SeqId.ToString();
                        });
                        POItem.UpdateRange(poItem);
                    }
                    else
                    {
                        po = PO.Create(po);
                        POItem.CreateRange(poItem);
                    }
                }
                UnitOfWork.Commit();
                return this.GetPOGroup((int)po.Id, (int)po.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        // 採購單異動，新增、儲存 ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝

        // 採購單刪除
        public ERP.Models.Views.POGroup RemovePOItemGroup(POItemGroup group)
        {
            try
            {
                var po = group.PO;
                var poItem = group.POItem;
                var poItemSize = group.POItemSize;

                UnitOfWork.BeginTransaction();
                if (po != null && poItem != null && (poItem.ReceivedLogId == null || poItem.ReceivedLogId == 0))
                {
                    POItemSize.RemoveRange(i => i.POItemId == poItem.Id && i.LocaleId == poItem.LocaleId);
                    PurBatchItem.RemoveRange(i => i.POItemId == poItem.Id && i.LocaleId == poItem.LocaleId);
                    POItem.Remove(poItem);

                    var hasItems = POItem.Get().Where(i => i.POId == po.Id && i.LocaleId == po.LocaleId).Any();
                    if (!hasItems)
                    {
                        PO.Remove(po);
                    }

                }
                UnitOfWork.Commit();
                return this.GetPOGroup((int)po.Id, (int)po.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        // public IQueryable<ERP.Models.Views.MaterialForPO> GetMRPMaterialForPO_(string predicate)
        // {
        //     var orderNos = ShipmentLog.Get().Select(i => i.OrderNo).Distinct();
        //     var ordersEx = Orders.Get().Where(o => !orderNos.Contains(o.OrderNo));

        //     // 1) 兩個來源各自先在 DB 端聚合（避免後面多次子查詢）
        //     var mrpAgg1 =
        //         from mrp in MRPItem.Get()
        //         group mrp by new { mrp.MaterialId, mrp.OrdersId, mrp.LocaleId, mrp.ParentMaterialId, mrp.SizeDivision, mrp.UnitCodeId, mrp.UnitNameTw }
        //         into g
        //         select new { g.Key.MaterialId, g.Key.OrdersId, g.Key.LocaleId, g.Key.ParentMaterialId, g.Key.SizeDivision, g.Key.UnitCodeId, g.Key.UnitNameTw, Usage = g.Sum(x => x.Total) };

        //     var mrpAgg2 =
        //         from mrp in MRPItemOrders.Get()
        //         group mrp by new { mrp.MaterialId, mrp.OrdersId, mrp.LocaleId, mrp.ParentMaterialId, mrp.SizeDivision, mrp.UnitCodeId, mrp.UnitNameTw }
        //         into g
        //         select new { g.Key.MaterialId, g.Key.OrdersId, g.Key.LocaleId, g.Key.ParentMaterialId, g.Key.SizeDivision, g.Key.UnitCodeId, g.Key.UnitNameTw, Usage = g.Sum(x => x.Total) };

        //     // 2) 連 Orders + 左連 Material；UNION 兩個來源；套 predicate（Dynamic LINQ）
        //     var q1 =
        //         from o in ordersEx
        //         join mrp in mrpAgg1 on new { OrdersId = o.Id, o.LocaleId } equals new { mrp.OrdersId, mrp.LocaleId }
        //         join m0 in Material.Get() on new { mrp.MaterialId, mrp.LocaleId } equals new { MaterialId = m0.Id, m0.LocaleId } into mGrp
        //         from m in mGrp.DefaultIfEmpty()
        //         select new
        //         {
        //             Id = m.Id,
        //             LocaleId = m.LocaleId,
        //             Material = m.MaterialName,
        //             MaterialEng = m.MaterialNameEng,
        //             SamplingMethod = m.SamplingMethod,
        //             CategoryCodeId = m.CategoryCodeId,
        //             SemiGoods = m.SemiGoods,
        //             TextureCodeId = m.TextureCodeId,
        //             OrderNo = o.OrderNo,
        //             OrdersId = o.Id,
        //             ParentMaterialId = mrp.ParentMaterialId,
        //             Usage = mrp.Usage,
        //             AlternateType = mrp.SizeDivision,
        //             UnitCodeId = mrp.UnitCodeId,
        //             UnitNameTw = mrp.UnitNameTw,
        //             StyleNo = o.StyleNo
        //         };

        //     var q2 =
        //         from o in ordersEx
        //         join mrp in mrpAgg2 on new { OrdersId = o.Id, o.LocaleId } equals new { mrp.OrdersId, mrp.LocaleId }
        //         join m0 in Material.Get() on new { mrp.MaterialId, mrp.LocaleId } equals new { MaterialId = m0.Id, m0.LocaleId } into mGrp
        //         from m in mGrp.DefaultIfEmpty()
        //         select new
        //         {
        //             Id = m.Id,
        //             LocaleId = m.LocaleId,
        //             Material = m.MaterialName,
        //             MaterialEng = m.MaterialNameEng,
        //             SamplingMethod = m.SamplingMethod,
        //             CategoryCodeId = m.CategoryCodeId,
        //             SemiGoods = m.SemiGoods,
        //             TextureCodeId = m.TextureCodeId,
        //             OrderNo = o.OrderNo,
        //             OrdersId = o.Id,
        //             ParentMaterialId = mrp.ParentMaterialId,
        //             Usage = mrp.Usage,
        //             AlternateType = mrp.SizeDivision,
        //             UnitCodeId = mrp.UnitCodeId,
        //             UnitNameTw = mrp.UnitNameTw,
        //             StyleNo = o.StyleNo
        //         };

        //     var baseSet = q1.Concat(q2).Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate).Distinct();

        //     // 3) 彙總成每筆要顯示的 MRP 物料需求（DB 端 GroupBy）
        //     var bomItems =
        //         from i in baseSet
        //         group i by new
        //         {
        //             i.LocaleId,
        //             i.OrdersId,
        //             i.OrderNo,
        //             MaterialId = i.Id,
        //             i.ParentMaterialId,
        //             i.Material,
        //             i.MaterialEng,
        //             i.UnitCodeId,
        //             i.CategoryCodeId,
        //             i.SemiGoods,
        //             i.SamplingMethod,
        //             i.TextureCodeId,
        //             AlternateType = i.AlternateType,
        //             i.StyleNo
        //         }
        //         into g
        //         select new
        //         {
        //             g.Key.LocaleId,
        //             g.Key.OrdersId,
        //             g.Key.OrderNo,
        //             g.Key.MaterialId,
        //             g.Key.Material,
        //             g.Key.MaterialEng,
        //             g.Key.ParentMaterialId,
        //             g.Key.UnitCodeId,
        //             g.Key.CategoryCodeId,
        //             g.Key.SemiGoods,
        //             g.Key.SamplingMethod,
        //             g.Key.TextureCodeId,
        //             g.Key.AlternateType,
        //             g.Key.StyleNo,
        //             TotalUsage = g.Sum(x => x.Usage)
        //         };

        //     // 4) 該訂單該料已規劃採購量（PurBatchItem）與已下 PO 量（POItem）→ 都在 DB 端 GroupBy
        //     var purSums =
        //         from p in PurBatchItem.Get()
        //         where p.Status != 2
        //         group p by new { p.OrdersId, p.LocaleId, p.MaterialId, p.ParentMaterialId }
        //         into g
        //         select new { g.Key.OrdersId, g.Key.LocaleId, g.Key.MaterialId, g.Key.ParentMaterialId, PurQty = g.Sum(x => (decimal?)x.PurQty) ?? 0m };

        //     var poSums =
        //         from p in POItem.Get()
        //         where p.Status != 2
        //         group p by new { p.OrdersId, p.LocaleId, p.MaterialId, p.ParentMaterialId }
        //         into g
        //         select new { g.Key.OrdersId, g.Key.LocaleId, g.Key.MaterialId, g.Key.ParentMaterialId, POQty = g.Sum(x => (decimal?)x.Qty) ?? 0m };

        //     // 5) 最新 PO 單（每鍵：MaterialId+ParentMaterialId+LocaleId 取 PODate DESC, Id DESC 的第一筆）
        //     var lastPO =
        //         from p in POItem.Get()
        //         where p.Status != 2
        //         group p by new { p.MaterialId, p.ParentMaterialId, p.LocaleId }
        //         into g
        //         select g.OrderByDescending(x => x.PODate).ThenByDescending(x => x.Id).Select(x => new
        //         {
        //             x.MaterialId,
        //             x.ParentMaterialId,
        //             x.LocaleId,
        //             x.UnitCodeId,
        //             x.UnitPrice,
        //             x.DollarCodeId,
        //             x.PayDollarCodeId,
        //             x.PaymentCodeId,
        //             x.PaymentPoint,
        //             x.SamplingMethod,
        //             x.VendorId,
        //             x.VendorNameTw,
        //             x.PODate,
        //             x.PayCodeId
        //         }).FirstOrDefault();

        //     // 6) 一次性左連所有輔助資料 → 直接投影到 ViewModel（大部分欄位都在 DB 端完成）
        //     var query =
        //         from b in bomItems
        //         join pur in purSums on new { b.OrdersId, b.LocaleId, MaterialId = (decimal?)b.MaterialId, ParentMaterialId = (decimal?)b.ParentMaterialId }
        //             equals new { pur.OrdersId, pur.LocaleId, MaterialId = (decimal?)pur.MaterialId, pur.ParentMaterialId } into purJoin
        //         from pj in purJoin.DefaultIfEmpty()
        //         join po in poSums on new { b.OrdersId, b.LocaleId, MaterialId = (decimal?)b.MaterialId, ParentMaterialId = (decimal?)b.ParentMaterialId }
        //             equals new { po.OrdersId, po.LocaleId, MaterialId = (decimal?)po.MaterialId, po.ParentMaterialId } into poJoin
        //         from poj in poJoin.DefaultIfEmpty()
        //         join lp in lastPO on new { MaterialId = (decimal?)b.MaterialId, ParentMaterialId = (decimal?)b.ParentMaterialId, b.LocaleId }
        //             equals new { MaterialId = (decimal?)lp.MaterialId, lp.ParentMaterialId, lp.LocaleId } into lpJoin
        //         from l in lpJoin.DefaultIfEmpty()
        //         select new ERP.Models.Views.MaterialForPO
        //         {
        //             Id = 0, // 先給 0，最後 ToList() 後再補流水號
        //             MaterialId = b.MaterialId,
        //             LocaleId = b.LocaleId,
        //             Material = b.Material,
        //             MaterialEng = b.MaterialEng,
        //             CategoryCodeId = b.CategoryCodeId,
        //             SemiGoods = b.SemiGoods,
        //             TextureCodeId = b.TextureCodeId,
        //             UnitCodeId = b.UnitCodeId,
        //             OrdersId = b.OrdersId,
        //             OrderNo = b.OrderNo,
        //             StyleNo = b.StyleNo,
        //             ParentMaterialId = b.ParentMaterialId,
        //             Usage = b.TotalUsage,
        //             // 數量：你原本最後覆蓋成 CEILING(Usage)，直接在 SQL 端完成
        //             PurQty = Math.Ceiling(b.TotalUsage),
        //             POQty = poj.POQty,
        //             AlternateType = b.AlternateType,
        //             // 「最新 PO 單」對應欄位（可能為 null）
        //             HasQuot = l == null ? 0 : 1,
        //             DollarCodeId = l == null ? null : l.DollarCodeId,
        //             PayCodeId = l == null ? (int?)null : l.PayCodeId,
        //             PaymentCodeId = l == null ? (decimal?)null : l.PaymentCodeId,
        //             PaymentPoint = l == null ? (int?)null : l.PaymentPoint,
        //             SamplingMethod = l == null ? b.SamplingMethod : l.SamplingMethod,
        //             VendorId = l == null ? (decimal?)null : l.VendorId,
        //             VendorShortNameTw = l == null ? null : l.VendorNameTw,
        //             PurUnitCodeId = l == null ? (decimal?)null : l.UnitCodeId,
        //             PurUnitPrice = l == null ? (decimal?)null : l.UnitPrice,
        //             PurDollarCodeId = l == null ? (decimal?)null : l.DollarCodeId,
        //             PurPayDollarCodeId = l == null ? (decimal?)null : l.PayDollarCodeId,
        //             PurPaymentCodeId = l == null ? (decimal?)null : l.PaymentCodeId,
        //             PurPaymentPoint = l == null ? (int?)null : l.PaymentPoint,
        //             PurSamplingMethod = l == null ? null : l.SamplingMethod,
        //             PurVendorId = l == null ? (decimal?)null : l.VendorId,
        //             PurVendorNameTw = l == null ? null : l.VendorNameTw,
        //             LastPODate = l == null ? (DateTime?)null : l.PODate
        //         };

        //     // 7) 到這裡整條都在 DB 端可翻譯；最後一次性取回並補上流水號 Id
        //     var list = query.OrderBy(x => x.Material).ToList();
        //     for (int i = 0; i < list.Count; i++) list[i].Id = i + 1;
        //     return list.AsQueryable();
        // }
        // 取得PO用的材料黨，包含報價資訊
        public IQueryable<ERP.Models.Views.MaterialForPO> GetMaterialForPO(string predicate)
        {
            var orderNos = ShipmentLog.Get().Select(i => i.OrderNo);
            var mrpItem = (
                from m in Material.Get()
                join mrp in MRPItem.Get().Select(i => new { i.MaterialId, i.OrdersId, i.LocaleId, i.ParentMaterialId }).Distinct()
                            on new { MaterialId = m.Id, LocaleId = m.LocaleId } equals new { MaterialId = mrp.MaterialId, LocaleId = mrp.LocaleId } into mrpGrp
                from mrp in mrpGrp.DefaultIfEmpty()
                join o in Orders.Get() on new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId } equals new { OrdersId = o.Id, LocaleId = o.LocaleId } into oGRP
                from o in oGRP.DefaultIfEmpty()
                where !orderNos.Contains(o.OrderNo)
                select new
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    Material = m.MaterialName,
                    MaterialEng = m.MaterialNameEng,
                    SamplingMethod = m.SamplingMethod,
                    CategoryCodeId = m.CategoryCodeId,
                    SemiGoods = m.SemiGoods,
                    TextureCodeId = m.TextureCodeId,
                    OrderNo = o.OrderNo,
                    OrdersId = o.Id,
                    ParentMaterialId = mrp.ParentMaterialId,
                    // POItemId = POItem.GetSimple().Where(i => i.Status != 2 && i.MaterialId == m.Id && i.LocaleId == m.LocaleId).OrderByDescending(i => i.PODate).Select(i => i.Id).FirstOrDefault(),
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                Material = i.Material,
                MaterialEng = i.MaterialEng,
                SamplingMethod = i.SamplingMethod,
                CategoryCodeId = i.CategoryCodeId,
                SemiGoods = i.SemiGoods,
                TextureCodeId = i.TextureCodeId,
                // ParentMaterialId = i.ParentMaterialId,
                ParentMaterialId = 0,
                // POItemId = i.POItemId,
            })
            .Distinct()
            .ToList();

            var mrpItemOrders = (
                from m in Material.Get()
                join mrp in MRPItemOrders.Get().Select(i => new { i.MaterialId, i.OrdersId, i.LocaleId, i.ParentMaterialId }).Distinct()
                            on new { MaterialId = m.Id, LocaleId = m.LocaleId } equals new { MaterialId = mrp.MaterialId, LocaleId = mrp.LocaleId } into mrpGrp
                from mrp in mrpGrp.DefaultIfEmpty()
                join o in Orders.Get() on new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId } equals new { OrdersId = o.Id, LocaleId = o.LocaleId } into oGRP
                from o in oGRP.DefaultIfEmpty()
                where !orderNos.Contains(o.OrderNo)
                select new
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    Material = m.MaterialName,
                    MaterialEng = m.MaterialNameEng,
                    SamplingMethod = m.SamplingMethod,
                    CategoryCodeId = m.CategoryCodeId,
                    SemiGoods = m.SemiGoods,
                    TextureCodeId = m.TextureCodeId,
                    OrderNo = o.OrderNo,
                    OrdersId = o.Id,
                    // ParentMaterialId = mrp.ParentMaterialId,
                    ParentMaterialId = 0,
                    // POItemId = POItem.GetSimple().Where(i => i.Status != 2 && i.MaterialId == m.Id && i.LocaleId == m.LocaleId).OrderByDescending(i => i.PODate).Select(i => i.Id).FirstOrDefault(),
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                Material = i.Material,
                MaterialEng = i.MaterialEng,
                SamplingMethod = i.SamplingMethod,
                CategoryCodeId = i.CategoryCodeId,
                SemiGoods = i.SemiGoods,
                TextureCodeId = i.TextureCodeId,
                ParentMaterialId = i.ParentMaterialId,
                // POItemId = i.POItemId,
            })
            .Distinct()
            .ToList();

            var items = mrpItem.Union(mrpItemOrders);
            var bomItems = items
                .GroupBy(i => new { i.LocaleId, i.Id, i.ParentMaterialId, i.Material, i.MaterialEng, i.CategoryCodeId, i.SemiGoods, i.SamplingMethod, i.TextureCodeId })
                .Select(i => new
                {
                    LocaleId = i.Key.LocaleId,
                    MaterialId = i.Key.Id,
                    Material = i.Key.Material,
                    MaterialEng = i.Key.MaterialEng,
                    ParentMaterialId = i.Key.ParentMaterialId,
                    CategoryCodeId = i.Key.CategoryCodeId,
                    // POItemId = i.Key.POItemId,
                    SamplingMethod = i.Key.SamplingMethod,
                    TextureCodeId = i.Key.TextureCodeId,
                    SemiGoods = i.Key.SemiGoods,
                })
                .OrderBy(i => i.Material)
                .Distinct()
                .ToList();

            if (bomItems.Count() == 0)
            {
                return (new List<ERP.Models.Views.MaterialForPO> { }).AsQueryable();
            }
            var localeId = bomItems[0].LocaleId;
            var mIds = bomItems.Select(i => i.MaterialId).Distinct().ToList();

            var maxPODates = POItem.Get().Where(i => mIds.Contains(i.MaterialId) && i.LocaleId == localeId && i.Status != 2)
                .GroupBy(i => new { i.MaterialId, i.LocaleId })
                .Select(g => new
                {
                    g.Key.MaterialId,
                    g.Key.LocaleId,
                    LatestPODate = g.Max(i => i.PODate) // 查找每個分組的最大 PODate
                })
                .Distinct();

            // var lastPOItems = (
            //     from p in POItem.Get().Where(i => mIds.Contains(i.MaterialId) && i.LocaleId == localeId && i.Status != 2)
            //     join mp in maxPODates on new { LocaleId = p.LocaleId, MaterialId = p.MaterialId, PODate = p.PODate } equals new { LocaleId = mp.LocaleId, MaterialId = mp.MaterialId, PODate = mp.LatestPODate } into mpGRP
            //     from mp in mpGRP.DefaultIfEmpty()
            //     select new
            //     {
            //         p.MaterialId,
            //         p.ParentMaterialId,
            //         p.LocaleId,
            //         p.UnitCodeId,
            //         p.UnitPrice,
            //         p.DollarCodeId,
            //         p.PayDollarCodeId,
            //         p.PaymentCodeId,
            //         p.PaymentPoint,
            //         p.SamplingMethod,
            //         p.VendorId,
            //         p.VendorNameTw,
            //         p.PODate,
            //         p.PayCodeId
            //     })
            //     .ToList();

            var lastPOItems = (
                from p in POItem.Get().Where(i => mIds.Contains(i.MaterialId) && i.LocaleId == localeId && i.Status != 2)
                join mp in maxPODates on new { LocaleId = p.LocaleId, MaterialId = p.MaterialId, PODate = p.PODate } equals new { LocaleId = mp.LocaleId, MaterialId = mp.MaterialId, PODate = mp.LatestPODate }
                select new
                {
                    p.MaterialId,
                    p.ParentMaterialId,
                    p.LocaleId,
                    p.UnitCodeId,
                    p.UnitPrice,
                    p.DollarCodeId,
                    p.PayDollarCodeId,
                    p.PaymentCodeId,
                    p.PaymentPoint,
                    p.SamplingMethod,
                    p.VendorId,
                    p.VendorNameTw,
                    p.PODate,
                    p.PayCodeId
                })
                .Distinct()
                .ToList();

            var materials = bomItems.AsQueryable().Select((i, index) => new Models.Views.MaterialForPO
            {
                Id = index + 1, // 分配序列号，从1开始
                MaterialId = i.MaterialId,
                LocaleId = i.LocaleId,
                Material = i.Material,
                MaterialEng = i.MaterialEng,
                CategoryCodeId = i.CategoryCodeId,
                SemiGoods = i.SemiGoods,
                TextureCodeId = i.TextureCodeId,
            })
            .ToList();

            materials.ForEach(i =>
            {
                i.Usage = 0;
                i.ParentMaterialId = 0;
                i.OrdersId = 0;

                var _poItem = lastPOItems.Where(p => p.MaterialId == i.MaterialId && p.LocaleId == i.LocaleId).FirstOrDefault();
                if (_poItem != null)
                {
                    i.HasQuot = 1;
                    i.UnitCodeId = _poItem.UnitCodeId;
                    i.UnitPrice = _poItem.UnitPrice;
                    i.DollarCodeId = _poItem.DollarCodeId;
                    i.PayCodeId = _poItem.PayCodeId;
                    i.PaymentCodeId = _poItem.PaymentCodeId;
                    i.PaymentPoint = _poItem.PaymentPoint;
                    i.SamplingMethod = _poItem.SamplingMethod;
                    i.VendorId = _poItem.VendorId;
                    i.VendorShortNameTw = _poItem.VendorNameTw;

                    i.PurUnitCodeId = _poItem.UnitCodeId;
                    i.PurUnitPrice = _poItem.UnitPrice;
                    i.PurDollarCodeId = _poItem.DollarCodeId;
                    i.PurPayDollarCodeId = _poItem.PayDollarCodeId;
                    i.PurPaymentCodeId = _poItem.PaymentCodeId;
                    i.PurPaymentPoint = _poItem.PaymentPoint;
                    i.PurSamplingMethod = _poItem.SamplingMethod;
                    i.PurVendorId = _poItem.VendorId;
                    i.PurVendorNameTw = _poItem.VendorNameTw;
                    i.PurPayCodeId = _poItem.PayCodeId;

                    i.LastPODate = _poItem.PODate;
                }
            });
            return materials.AsQueryable();
        }
        public IQueryable<ERP.Models.Views.MaterialForPO> GetMRPMaterialForPO(string predicate)
        {
            var orderNos = ShipmentLog.Get().Select(i => i.OrderNo);
            var mrpItem = (
                from o in Orders.Get()
                join mrp in MRPItem.Get().GroupBy(i => new { i.MaterialId, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw }).Select(i => new { i.Key.MaterialId, i.Key.OrdersId, i.Key.LocaleId, i.Key.ParentMaterialId, i.Key.SizeDivision, i.Key.UnitCodeId, i.Key.UnitNameTw, Usage = i.Sum(g => g.Total) }) on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId }
                join m in Material.Get() on new { MaterialId = mrp.MaterialId, LocaleId = mrp.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGrp
                from m in mGrp.DefaultIfEmpty()
                    // where !orderNos.Contains(o.OrderNo)
                select new
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    Material = m.MaterialName,
                    MaterialEng = m.MaterialNameEng,
                    SamplingMethod = m.SamplingMethod,
                    CategoryCodeId = m.CategoryCodeId,
                    SemiGoods = m.SemiGoods,
                    TextureCodeId = m.TextureCodeId,
                    OrderNo = o.OrderNo,
                    OrdersId = o.Id,
                    ParentMaterialId = mrp.ParentMaterialId,
                    Usage = mrp.Usage,
                    // POItemId = POItem.GetSimple().Where(i => i.Status != 2 && i.MaterialId == mrp.MaterialId && i.ParentMaterialId == mrp.ParentMaterialId && i.LocaleId == mrp.LocaleId).OrderByDescending(i => i.PODate).Select(i => i.Id).FirstOrDefault(),
                    // POItemId = POItem.GetSimple().Where(i => i.MaterialId == m.Id && i.LocaleId == m.LocaleId).OrderByDescending(i => i.PODate).Select(i => i.Id).FirstOrDefault(),
                    AlternateType = mrp.SizeDivision,
                    UnitCodeId = mrp.UnitCodeId,
                    UnitNameTw = mrp.UnitNameTw,
                    StyleNo = o.StyleNo,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Distinct()
            .ToList();

            var mrpItemOrders = (
                from o in Orders.Get()
                join mrp in MRPItemOrders.Get().GroupBy(i => new { i.MaterialId, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw }).Select(i => new { i.Key.MaterialId, i.Key.OrdersId, i.Key.LocaleId, i.Key.ParentMaterialId, i.Key.SizeDivision, i.Key.UnitCodeId, i.Key.UnitNameTw, Usage = i.Sum(g => g.Total) }) on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId }
                join m in Material.Get() on new { MaterialId = mrp.MaterialId, LocaleId = mrp.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGrp
                from m in mGrp.DefaultIfEmpty()
                    // where !orderNos.Contains(o.OrderNo)
                select new
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    Material = m.MaterialName,
                    MaterialEng = m.MaterialNameEng,
                    SamplingMethod = m.SamplingMethod,
                    CategoryCodeId = m.CategoryCodeId,
                    SemiGoods = m.SemiGoods,
                    TextureCodeId = m.TextureCodeId,
                    OrderNo = o.OrderNo,
                    OrdersId = o.Id,
                    ParentMaterialId = mrp.ParentMaterialId,
                    Usage = mrp.Usage,
                    // POItemId = POItem.GetSimple().Where(i => i.Status != 2 && i.MaterialId == mrp.MaterialId && i.ParentMaterialId == mrp.ParentMaterialId && i.LocaleId == mrp.LocaleId).OrderByDescending(i => i.PODate).Select(i => i.Id).FirstOrDefault(),
                    // POItemId = POItem.GetSimple().Where(i => i.MaterialId == m.Id && i.LocaleId == m.LocaleId).OrderByDescending(i => i.PODate).Select(i => i.Id).FirstOrDefault(),
                    AlternateType = mrp.SizeDivision,
                    UnitCodeId = mrp.UnitCodeId,
                    UnitNameTw = mrp.UnitNameTw,
                    StyleNo = o.StyleNo,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Distinct()
            .ToList();

            var items = mrpItem.Union(mrpItemOrders);
            var bomItems = items
                .GroupBy(i => new { i.LocaleId, i.OrdersId, i.OrderNo, i.Id, i.ParentMaterialId, i.Material, i.MaterialEng, i.UnitCodeId, i.CategoryCodeId, i.SemiGoods, i.SamplingMethod, i.TextureCodeId, i.AlternateType, i.StyleNo })
                .Select(i => new
                {
                    LocaleId = i.Key.LocaleId,
                    OrdersId = i.Key.OrdersId,
                    OrderNo = i.Key.OrderNo,
                    MaterialId = i.Key.Id,
                    Material = i.Key.Material,
                    MaterialEng = i.Key.MaterialEng,
                    ParentMaterialId = i.Key.ParentMaterialId,
                    UnitCodeId = i.Key.UnitCodeId,
                    CategoryCodeId = i.Key.CategoryCodeId,
                    SizeDivision = i.Key.AlternateType,
                    TotalUsage = i.Sum(g => g.Usage),
                    StyleNo = i.Key.StyleNo,
                    // POItemId = i.Key.POItemId,
                    SamplingMethod = i.Key.SamplingMethod,
                    TextureCodeId = i.Key.TextureCodeId,
                    SemiGoods = i.Key.SemiGoods,
                    AlternateType = i.Key.AlternateType,
                })
                .OrderBy(i => i.Material)
                .ToList();

            if (bomItems.Count() == 0)
            {
                return (new List<ERP.Models.Views.MaterialForPO> { }).AsQueryable();
            }

            var mIds = bomItems.Select(i => i.MaterialId).Distinct().ToList();
            var oIds = bomItems.Select(i => i.OrdersId).Distinct().ToList();
            var localeId = bomItems[0].LocaleId;

            var purBatchItems = PurBatchItem.Get().Where(i => i.Status != 2 && i.LocaleId == localeId && oIds.Contains(i.OrdersId) && mIds.Contains(i.MaterialId)).Select(i => new { i.OrdersId, i.LocaleId, i.MaterialId, i.ParentMaterialId, i.PurQty }).ToList();
            // 該訂單買過的採料採購單
            var poItems = POItem.Get().Where(i => i.Status != 2 && i.LocaleId == localeId && oIds.Contains(i.OrdersId) && mIds.Contains(i.MaterialId)).Select(i => new { i.OrdersId, i.LocaleId, i.MaterialId, i.ParentMaterialId, i.Qty }).ToList();

            // 該材料的最新採購單
            var maxPODates = POItem.Get().Where(i => mIds.Contains(i.MaterialId) && i.LocaleId == localeId && i.Status != 2)
                            .GroupBy(i => new { i.MaterialId, i.LocaleId })
                            .Select(g => new
                            {
                                g.Key.MaterialId,
                                g.Key.LocaleId,
                                LatestPODate = g.Max(i => i.PODate) // 查找每個分組的最大 PODate
                            })
                            .Distinct();
            var lastPOItems = (
                from p in POItem.Get().Where(i => mIds.Contains(i.MaterialId) && i.LocaleId == localeId && i.Status != 2)
                join mp in maxPODates on new { LocaleId = p.LocaleId, MaterialId = p.MaterialId, PODate = p.PODate } equals new { LocaleId = mp.LocaleId, MaterialId = mp.MaterialId, PODate = mp.LatestPODate } into mpGRP
                from mp in mpGRP.DefaultIfEmpty()
                select new
                {
                    p.MaterialId,
                    p.ParentMaterialId,
                    p.LocaleId,
                    p.UnitCodeId,
                    p.UnitPrice,
                    p.DollarCodeId,
                    p.PayDollarCodeId,
                    p.PaymentCodeId,
                    p.PaymentPoint,
                    p.SamplingMethod,
                    p.VendorId,
                    p.VendorNameTw,
                    p.PODate,
                    p.PayCodeId
                })
                .ToList();
            var materials = bomItems.AsQueryable().Select((i, index) => new Models.Views.MaterialForPO
            {
                Id = index + 1, // 分配序列号，从1开始
                MaterialId = i.MaterialId,
                LocaleId = i.LocaleId,
                Material = i.Material,
                MaterialEng = i.MaterialEng,
                CategoryCodeId = i.CategoryCodeId,
                SemiGoods = i.SemiGoods,
                TextureCodeId = i.TextureCodeId,
                UnitCodeId = i.UnitCodeId,
                OrdersId = i.OrdersId,
                OrderNo = i.OrderNo,
                StyleNo = i.StyleNo,
                ParentMaterialId = i.ParentMaterialId,
                Usage = i.TotalUsage,
                AlternateType = i.AlternateType,
            })
            .ToList();

            materials.ForEach(i =>
            {
                // 數量用該訂單的
                i.PurQty = purBatchItems.Where(p => p.OrdersId == i.OrdersId && p.LocaleId == i.LocaleId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId).Sum(p => p.PurQty);
                i.POQty = poItems.Where(p => p.OrdersId == i.OrdersId && p.LocaleId == i.LocaleId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId).Sum(p => p.Qty);

                // 單價廠商用最新的
                var _poItem = lastPOItems.Where(p => p.MaterialId == i.MaterialId && i.ParentMaterialId == p.ParentMaterialId && p.LocaleId == i.LocaleId).FirstOrDefault();
                if (_poItem != null)
                {
                    i.HasQuot = 1;
                    i.UnitCodeId = _poItem.UnitCodeId;
                    i.UnitPrice = _poItem.UnitPrice;
                    i.DollarCodeId = _poItem.DollarCodeId;
                    i.PayCodeId = _poItem.PayCodeId;
                    i.PaymentCodeId = _poItem.PaymentCodeId;
                    i.PaymentPoint = _poItem.PaymentPoint;
                    i.SamplingMethod = _poItem.SamplingMethod;
                    i.VendorId = _poItem.VendorId;
                    i.VendorShortNameTw = _poItem.VendorNameTw;

                    i.PurUnitCodeId = _poItem.UnitCodeId;
                    i.PurUnitPrice = _poItem.UnitPrice;
                    i.PurDollarCodeId = _poItem.DollarCodeId;
                    i.PurPayDollarCodeId = _poItem.PayDollarCodeId;
                    i.PurPaymentCodeId = _poItem.PaymentCodeId;
                    i.PurPaymentPoint = _poItem.PaymentPoint;
                    i.PurSamplingMethod = _poItem.SamplingMethod;
                    i.PurVendorId = _poItem.VendorId;
                    i.PurVendorNameTw = _poItem.VendorNameTw;
                    i.PurPayCodeId = _poItem.PayCodeId;

                    i.LastPODate = _poItem.PODate;
                }

                i.PurQty = Math.Ceiling((decimal)i.Usage);
            });

            return materials.AsQueryable();
        }

        // public IQueryable<ERP.Models.Views.MaterialForPO> GetMaterialForPO1(string predicate)
        // {
        //     var mrpItem = (
        //         from m in Material.Get()
        //         join mrp in MRPItem.Get().Select(i => new { i.MaterialId, i.OrdersId, i.LocaleId, i.ParentMaterialId }).Distinct()
        //                     on new { MaterialId = m.Id, LocaleId = m.LocaleId } equals new { MaterialId = mrp.MaterialId, LocaleId = mrp.LocaleId } into mrpGrp
        //         from mrp in mrpGrp.DefaultIfEmpty()
        //         join o in Orders.Get() on new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId } equals new { OrdersId = o.Id, LocaleId = o.LocaleId } into oGRP
        //         from o in oGRP.DefaultIfEmpty()
        //         select new
        //         {
        //             Id = m.Id,
        //             LocaleId = m.LocaleId,
        //             Material = m.MaterialName,
        //             MaterialEng = m.MaterialNameEng,
        //             SamplingMethod = m.SamplingMethod,
        //             CategoryCodeId = m.CategoryCodeId,
        //             SemiGoods = m.SemiGoods,
        //             TextureCodeId = m.TextureCodeId,
        //             OrderNo = o.OrderNo,
        //             OrdersId = o.Id,
        //             ParentMaterialId = mrp.ParentMaterialId,
        //             POItemId = POItem.GetSimple().Where(i => i.Status != 2 && i.MaterialId == m.Id && i.LocaleId == m.LocaleId).OrderByDescending(i => i.PODate).Select(i => i.Id).FirstOrDefault(),
        //         }
        //     )
        //     .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
        //     .Select(i => new
        //     {
        //         Id = i.Id,
        //         LocaleId = i.LocaleId,
        //         Material = i.Material,
        //         MaterialEng = i.MaterialEng,
        //         SamplingMethod = i.SamplingMethod,
        //         CategoryCodeId = i.CategoryCodeId,
        //         SemiGoods = i.SemiGoods,
        //         TextureCodeId = i.TextureCodeId,
        //         // ParentMaterialId = i.ParentMaterialId,
        //         ParentMaterialId = 0,
        //         POItemId = i.POItemId,
        //     })
        //     .Distinct()
        //     .ToList();

        //     var mrpItemOrders = (
        //         from m in Material.Get()
        //         join mrp in MRPItemOrders.Get().Select(i => new { i.MaterialId, i.OrdersId, i.LocaleId, i.ParentMaterialId }).Distinct()
        //                     on new { MaterialId = m.Id, LocaleId = m.LocaleId } equals new { MaterialId = mrp.MaterialId, LocaleId = mrp.LocaleId } into mrpGrp
        //         from mrp in mrpGrp.DefaultIfEmpty()
        //         join o in Orders.Get() on new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId } equals new { OrdersId = o.Id, LocaleId = o.LocaleId } into oGRP
        //         from o in oGRP.DefaultIfEmpty()
        //         select new
        //         {
        //             Id = m.Id,
        //             LocaleId = m.LocaleId,
        //             Material = m.MaterialName,
        //             MaterialEng = m.MaterialNameEng,
        //             SamplingMethod = m.SamplingMethod,
        //             CategoryCodeId = m.CategoryCodeId,
        //             SemiGoods = m.SemiGoods,
        //             TextureCodeId = m.TextureCodeId,
        //             OrderNo = o.OrderNo,
        //             OrdersId = o.Id,
        //             // ParentMaterialId = mrp.ParentMaterialId,
        //             ParentMaterialId = 0,
        //             POItemId = POItem.GetSimple().Where(i => i.Status != 2 && i.MaterialId == m.Id && i.LocaleId == m.LocaleId).OrderByDescending(i => i.PODate).Select(i => i.Id).FirstOrDefault(),
        //         }
        //     )
        //     .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
        //     .Select(i => new
        //     {
        //         Id = i.Id,
        //         LocaleId = i.LocaleId,
        //         Material = i.Material,
        //         MaterialEng = i.MaterialEng,
        //         SamplingMethod = i.SamplingMethod,
        //         CategoryCodeId = i.CategoryCodeId,
        //         SemiGoods = i.SemiGoods,
        //         TextureCodeId = i.TextureCodeId,
        //         ParentMaterialId = i.ParentMaterialId,
        //         POItemId = i.POItemId,
        //     })
        //     .Distinct()
        //     .ToList();

        //     var items = mrpItem.Union(mrpItemOrders);
        //     var bomItems = items
        //         .GroupBy(i => new { i.LocaleId, i.Id, i.ParentMaterialId, i.Material, i.MaterialEng, i.CategoryCodeId, i.SemiGoods, i.SamplingMethod, i.TextureCodeId, i.POItemId })
        //         .Select(i => new
        //         {
        //             LocaleId = i.Key.LocaleId,
        //             MaterialId = i.Key.Id,
        //             Material = i.Key.Material,
        //             MaterialEng = i.Key.MaterialEng,
        //             ParentMaterialId = i.Key.ParentMaterialId,
        //             CategoryCodeId = i.Key.CategoryCodeId,
        //             POItemId = i.Key.POItemId,
        //             SamplingMethod = i.Key.SamplingMethod,
        //             TextureCodeId = i.Key.TextureCodeId,
        //             SemiGoods = i.Key.SemiGoods,
        //         })
        //         .OrderBy(i => i.Material)
        //         .Distinct()
        //         .ToList();

        //     if (bomItems.Count() == 0)
        //     {
        //         return (new List<ERP.Models.Views.MaterialForPO> { }).AsQueryable();
        //     }
        //     var mIds = bomItems.Select(i => i.MaterialId).Distinct().ToList();
        //     var lPOIds = bomItems.Select(i => i.POItemId).Distinct().ToList();
        //     var localeId = bomItems[0].LocaleId;
        //     var lastPOItems = POItem.Get()
        //             .Where(i => lPOIds.Contains(i.Id) && i.LocaleId == localeId)
        //             .Select(i => new
        //             {
        //                 i.MaterialId,
        //                 i.ParentMaterialId,
        //                 i.LocaleId,
        //                 i.UnitCodeId,
        //                 i.UnitPrice,
        //                 i.DollarCodeId,
        //                 i.PayDollarCodeId,
        //                 i.PaymentCodeId,
        //                 i.PaymentPoint,
        //                 i.SamplingMethod,
        //                 i.VendorId,
        //                 i.VendorNameTw,
        //                 i.PODate,
        //                 i.PayCodeId,
        //             })
        //             .ToList()  // 在数据库中执行上述查询，并将结果加载到内存中
        //             .GroupBy(i => new { i.MaterialId, i.ParentMaterialId })
        //             .Select(g => g.OrderByDescending(i => i.PODate).FirstOrDefault())
        //             .ToList();

        //     var materials = bomItems.AsQueryable().Select((i, index) => new Models.Views.MaterialForPO
        //     {
        //         Id = index + 1, // 分配序列号，从1开始
        //         MaterialId = i.MaterialId,
        //         LocaleId = i.LocaleId,
        //         Material = i.Material,
        //         MaterialEng = i.MaterialEng,
        //         CategoryCodeId = i.CategoryCodeId,
        //         SemiGoods = i.SemiGoods,
        //         TextureCodeId = i.TextureCodeId,
        //     })
        //     .ToList();

        //     materials.ForEach(i =>
        //     {
        //         i.Usage = 0;
        //         i.ParentMaterialId = 0;
        //         i.OrdersId = 0;

        //         var _poItem = lastPOItems.Where(p => p.MaterialId == i.MaterialId && p.LocaleId == i.LocaleId).FirstOrDefault();
        //         if (_poItem != null)
        //         {
        //             i.HasQuot = 1;
        //             i.UnitCodeId = _poItem.UnitCodeId;
        //             i.UnitPrice = _poItem.UnitPrice;
        //             i.DollarCodeId = _poItem.DollarCodeId;
        //             i.PayCodeId = _poItem.PayCodeId;
        //             i.PaymentCodeId = _poItem.PaymentCodeId;
        //             i.PaymentPoint = _poItem.PaymentPoint;
        //             i.SamplingMethod = _poItem.SamplingMethod;
        //             i.VendorId = _poItem.VendorId;
        //             i.VendorShortNameTw = _poItem.VendorNameTw;

        //             i.PurUnitCodeId = _poItem.UnitCodeId;
        //             i.PurUnitPrice = _poItem.UnitPrice;
        //             i.PurDollarCodeId = _poItem.DollarCodeId;
        //             i.PurPayDollarCodeId = _poItem.PayDollarCodeId;
        //             i.PurPaymentCodeId = _poItem.PaymentCodeId;
        //             i.PurPaymentPoint = _poItem.PaymentPoint;
        //             i.PurSamplingMethod = _poItem.SamplingMethod;
        //             i.PurVendorId = _poItem.VendorId;
        //             i.PurVendorNameTw = _poItem.VendorNameTw;
        //             i.PurPayCodeId = _poItem.PayCodeId;

        //             i.LastPODate = _poItem.PODate;
        //         }
        //     });
        //     return materials.AsQueryable();
        // }
        // public IQueryable<ERP.Models.Views.MaterialForPO> GetMRPMaterialForPO1(string predicate)
        // {
        //     var mrpItem = (
        //         from o in Orders.Get()
        //         join mrp in MRPItem.Get().GroupBy(i => new { i.MaterialId, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw }).Select(i => new { i.Key.MaterialId, i.Key.OrdersId, i.Key.LocaleId, i.Key.ParentMaterialId, i.Key.SizeDivision, i.Key.UnitCodeId, i.Key.UnitNameTw, Usage = i.Sum(g => g.Total) }) on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId }
        //         join m in Material.Get() on new { MaterialId = mrp.MaterialId, LocaleId = mrp.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGrp
        //         from m in mGrp.DefaultIfEmpty()
        //         select new
        //         {
        //             Id = m.Id,
        //             LocaleId = m.LocaleId,
        //             Material = m.MaterialName,
        //             MaterialEng = m.MaterialNameEng,
        //             SamplingMethod = m.SamplingMethod,
        //             CategoryCodeId = m.CategoryCodeId,
        //             SemiGoods = m.SemiGoods,
        //             TextureCodeId = m.TextureCodeId,
        //             OrderNo = o.OrderNo,
        //             OrdersId = o.Id,
        //             ParentMaterialId = mrp.ParentMaterialId,
        //             Usage = mrp.Usage,
        //             POItemId = POItem.GetSimple().Where(i => i.Status != 2 && i.MaterialId == mrp.MaterialId && i.ParentMaterialId == mrp.ParentMaterialId && i.LocaleId == mrp.LocaleId).OrderByDescending(i => i.PODate).Select(i => i.Id).FirstOrDefault(),
        //             // POItemId = POItem.GetSimple().Where(i => i.MaterialId == m.Id && i.LocaleId == m.LocaleId).OrderByDescending(i => i.PODate).Select(i => i.Id).FirstOrDefault(),
        //             AlternateType = mrp.SizeDivision,
        //             UnitCodeId = mrp.UnitCodeId,
        //             UnitNameTw = mrp.UnitNameTw,
        //             StyleNo = o.StyleNo,
        //         }
        //     )
        //     .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
        //     .Distinct()
        //     .ToList();

        //     var mrpItemOrders = (
        //         from o in Orders.Get()
        //         join mrp in MRPItemOrders.Get().GroupBy(i => new { i.MaterialId, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw }).Select(i => new { i.Key.MaterialId, i.Key.OrdersId, i.Key.LocaleId, i.Key.ParentMaterialId, i.Key.SizeDivision, i.Key.UnitCodeId, i.Key.UnitNameTw, Usage = i.Sum(g => g.Total) }) on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId }
        //         join m in Material.Get() on new { MaterialId = mrp.MaterialId, LocaleId = mrp.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGrp
        //         from m in mGrp.DefaultIfEmpty()
        //         select new
        //         {
        //             Id = m.Id,
        //             LocaleId = m.LocaleId,
        //             Material = m.MaterialName,
        //             MaterialEng = m.MaterialNameEng,
        //             SamplingMethod = m.SamplingMethod,
        //             CategoryCodeId = m.CategoryCodeId,
        //             SemiGoods = m.SemiGoods,
        //             TextureCodeId = m.TextureCodeId,
        //             OrderNo = o.OrderNo,
        //             OrdersId = o.Id,
        //             ParentMaterialId = mrp.ParentMaterialId,
        //             Usage = mrp.Usage,
        //             POItemId = POItem.GetSimple().Where(i => i.Status != 2 && i.MaterialId == mrp.MaterialId && i.ParentMaterialId == mrp.ParentMaterialId && i.LocaleId == mrp.LocaleId).OrderByDescending(i => i.PODate).Select(i => i.Id).FirstOrDefault(),
        //             // POItemId = POItem.GetSimple().Where(i => i.MaterialId == m.Id && i.LocaleId == m.LocaleId).OrderByDescending(i => i.PODate).Select(i => i.Id).FirstOrDefault(),
        //             AlternateType = mrp.SizeDivision,
        //             UnitCodeId = mrp.UnitCodeId,
        //             UnitNameTw = mrp.UnitNameTw,
        //             StyleNo = o.StyleNo,
        //         }
        //     )
        //     .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
        //     .Distinct()
        //     .ToList();

        //     var items = mrpItem.Union(mrpItemOrders);
        //     var bomItems = items
        //         .GroupBy(i => new { i.LocaleId, i.OrdersId, i.OrderNo, i.Id, i.ParentMaterialId, i.Material, i.MaterialEng, i.UnitCodeId, i.CategoryCodeId, i.SemiGoods, i.SamplingMethod, i.TextureCodeId, i.AlternateType, i.StyleNo, i.POItemId })
        //         .Select(i => new
        //         {
        //             LocaleId = i.Key.LocaleId,
        //             OrdersId = i.Key.OrdersId,
        //             OrderNo = i.Key.OrderNo,
        //             MaterialId = i.Key.Id,
        //             Material = i.Key.Material,
        //             MaterialEng = i.Key.MaterialEng,
        //             ParentMaterialId = i.Key.ParentMaterialId,
        //             UnitCodeId = i.Key.UnitCodeId,
        //             CategoryCodeId = i.Key.CategoryCodeId,
        //             SizeDivision = i.Key.AlternateType,
        //             TotalUsage = i.Sum(g => g.Usage),
        //             StyleNo = i.Key.StyleNo,
        //             POItemId = i.Key.POItemId,
        //             SamplingMethod = i.Key.SamplingMethod,
        //             TextureCodeId = i.Key.TextureCodeId,
        //             SemiGoods = i.Key.SemiGoods,
        //             AlternateType = i.Key.AlternateType,
        //         })
        //         .OrderBy(i => i.Material)
        //         .ToList();

        //     if (bomItems.Count() == 0)
        //     {
        //         return (new List<ERP.Models.Views.MaterialForPO> { }).AsQueryable();
        //     }

        //     var mIds = bomItems.Select(i => i.MaterialId).Distinct().ToList();
        //     var oIds = bomItems.Select(i => i.OrdersId).Distinct().ToList();
        //     var lPOIds = bomItems.Select(i => i.POItemId).Distinct().ToList();
        //     var localeId = bomItems[0].LocaleId;

        //     var purBatchItems = PurBatchItem.Get().Where(i => i.Status != 2 && i.LocaleId == localeId && oIds.Contains(i.OrdersId)).Select(i => new { i.OrdersId, i.LocaleId, i.MaterialId, i.ParentMaterialId, i.PurQty }).ToList();
        //     var poItems = POItem.Get().Where(i => i.Status != 2 && i.LocaleId == localeId && oIds.Contains(i.OrdersId)).Select(i => new { i.OrdersId, i.LocaleId, i.MaterialId, i.ParentMaterialId, i.Qty }).ToList();

        //     var lastPOItems = POItem.Get()
        //             .Where(i => lPOIds.Contains(i.Id) && i.LocaleId == localeId)
        //             .Select(i => new
        //             {
        //                 i.MaterialId,
        //                 i.ParentMaterialId,
        //                 i.LocaleId,
        //                 i.UnitCodeId,
        //                 i.UnitPrice,
        //                 i.DollarCodeId,
        //                 i.PayDollarCodeId,
        //                 i.PaymentCodeId,
        //                 i.PaymentPoint,
        //                 i.SamplingMethod,
        //                 i.VendorId,
        //                 i.VendorNameTw,
        //                 i.PODate,
        //                 i.PayCodeId,
        //             })
        //             .ToList()  // 在数据库中执行上述查询，并将结果加载到内存中
        //             .GroupBy(i => new { i.MaterialId, i.ParentMaterialId })
        //             .Select(g => g.OrderByDescending(i => i.PODate).FirstOrDefault())
        //             .ToList();

        //     var materials = bomItems.AsQueryable().Select((i, index) => new Models.Views.MaterialForPO
        //     {
        //         Id = index + 1, // 分配序列号，从1开始
        //         MaterialId = i.MaterialId,
        //         LocaleId = i.LocaleId,
        //         Material = i.Material,
        //         MaterialEng = i.MaterialEng,
        //         CategoryCodeId = i.CategoryCodeId,
        //         SemiGoods = i.SemiGoods,
        //         TextureCodeId = i.TextureCodeId,
        //         UnitCodeId = i.UnitCodeId,
        //         OrdersId = i.OrdersId,
        //         OrderNo = i.OrderNo,
        //         StyleNo = i.StyleNo,
        //         ParentMaterialId = i.ParentMaterialId,
        //         Usage = i.TotalUsage,
        //         AlternateType = i.AlternateType,
        //     })
        //         .ToList();

        //     materials.ForEach(i =>
        //     {
        //         i.PurQty = purBatchItems.Where(p => p.OrdersId == i.OrdersId && p.LocaleId == i.LocaleId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId).Sum(p => p.PurQty);
        //         i.POQty = poItems.Where(p => p.OrdersId == i.OrdersId && p.LocaleId == i.LocaleId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId).Sum(p => p.Qty);

        //         var _poItem = lastPOItems.Where(p => p.MaterialId == i.MaterialId && i.ParentMaterialId == p.ParentMaterialId && p.LocaleId == i.LocaleId).FirstOrDefault();
        //         if (_poItem != null)
        //         {
        //             i.HasQuot = 1;
        //             i.UnitCodeId = _poItem.UnitCodeId;
        //             i.UnitPrice = _poItem.UnitPrice;
        //             i.DollarCodeId = _poItem.DollarCodeId;
        //             i.PayCodeId = _poItem.PayCodeId;
        //             i.PaymentCodeId = _poItem.PaymentCodeId;
        //             i.PaymentPoint = _poItem.PaymentPoint;
        //             i.SamplingMethod = _poItem.SamplingMethod;
        //             i.VendorId = _poItem.VendorId;
        //             i.VendorShortNameTw = _poItem.VendorNameTw;

        //             i.PurUnitCodeId = _poItem.UnitCodeId;
        //             i.PurUnitPrice = _poItem.UnitPrice;
        //             i.PurDollarCodeId = _poItem.DollarCodeId;
        //             i.PurPayDollarCodeId = _poItem.PayDollarCodeId;
        //             i.PurPaymentCodeId = _poItem.PaymentCodeId;
        //             i.PurPaymentPoint = _poItem.PaymentPoint;
        //             i.PurSamplingMethod = _poItem.SamplingMethod;
        //             i.PurVendorId = _poItem.VendorId;
        //             i.PurVendorNameTw = _poItem.VendorNameTw;
        //             i.PurPayCodeId = _poItem.PayCodeId;

        //             i.LastPODate = _poItem.PODate;
        //         }

        //         i.PurQty = Math.Ceiling((decimal)i.Usage);
        //     });

        //     return materials.AsQueryable();
        // }
        // public IQueryable<ERP.Models.Views.MaterialForPO> GetAllMaterialForPO(string predicate)
        // {
        //     var mrpItem = (
        //         from m in Material.Get()
        //         join mrp in MRPItem.Get().GroupBy(i => new { i.MaterialId, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw }).Select(i => new { i.Key.MaterialId, i.Key.OrdersId, i.Key.LocaleId, i.Key.ParentMaterialId, i.Key.SizeDivision, i.Key.UnitCodeId, i.Key.UnitNameTw, Usage = i.Sum(g => g.Total) }) 
        //                     on new { MaterialId = m.Id, LocaleId = m.LocaleId } equals new { MaterialId = mrp.MaterialId, LocaleId = mrp.LocaleId } into mrpGrp
        //         from mrp in mrpGrp.DefaultIfEmpty()
        //         join o in Orders.Get() on new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId } equals new { OrdersId = o.Id, LocaleId = o.LocaleId } into oGRP
        //         from o in oGRP.DefaultIfEmpty()
        //         select new
        //         {
        //             Id = m.Id,
        //             LocaleId = m.LocaleId,
        //             Material = m.MaterialName,
        //             MaterialEng = m.MaterialNameEng,
        //             SamplingMethod = m.SamplingMethod,
        //             CategoryCodeId = m.CategoryCodeId,
        //             SemiGoods = m.SemiGoods,
        //             TextureCodeId = m.TextureCodeId,
        //             OrderNo = o.OrderNo,
        //             OrdersId = o.Id,
        //             ParentMaterialId = mrp.ParentMaterialId,
        //             POItemId = POItem.GetSimple().Where(i => i.Status != 2 && i.MaterialId == m.Id && i.LocaleId == m.LocaleId).OrderByDescending(i => i.PODate).Select(i => i.Id).FirstOrDefault(),
        //             AlternateType = mrp.SizeDivision,
        //             UnitCodeId = mrp.UnitCodeId,
        //             UnitNameTw = mrp.UnitNameTw,
        //             StyleNo = o.StyleNo,
        //             Usage = mrp.Usage,
        //         }
        //     )
        //     .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
        //     .Distinct()
        //     .ToList();

        //     var mrpItemOrders = (
        //         from m in Material.Get()
        //         // join mrp in MRPItemOrders.Get().Select(i => new { i.MaterialId, i.OrdersId, i.LocaleId, i.ParentMaterialId }).Distinct()
        //         join mrp in MRPItemOrders.Get().GroupBy(i => new { i.MaterialId, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw }).Select(i => new { i.Key.MaterialId, i.Key.OrdersId, i.Key.LocaleId, i.Key.ParentMaterialId, i.Key.SizeDivision, i.Key.UnitCodeId, i.Key.UnitNameTw, Usage = i.Sum(g => g.Total) }) 
        //                     on new { MaterialId = m.Id, LocaleId = m.LocaleId } equals new { MaterialId = mrp.MaterialId, LocaleId = mrp.LocaleId } into mrpGrp
        //         from mrp in mrpGrp.DefaultIfEmpty()
        //         join o in Orders.Get() on new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId } equals new { OrdersId = o.Id, LocaleId = o.LocaleId } into oGRP
        //         from o in oGRP.DefaultIfEmpty()
        //         select new
        //         {
        //             Id = m.Id,
        //             LocaleId = m.LocaleId,
        //             Material = m.MaterialName,
        //             MaterialEng = m.MaterialNameEng,
        //             SamplingMethod = m.SamplingMethod,
        //             CategoryCodeId = m.CategoryCodeId,
        //             SemiGoods = m.SemiGoods,
        //             TextureCodeId = m.TextureCodeId,
        //             OrderNo = o.OrderNo,
        //             OrdersId = o.Id,
        //             ParentMaterialId = mrp.ParentMaterialId,
        //             POItemId = POItem.GetSimple().Where(i => i.Status != 2 && i.MaterialId == m.Id && i.LocaleId == m.LocaleId).OrderByDescending(i => i.PODate).Select(i => i.Id).FirstOrDefault(),
        //             AlternateType = mrp.SizeDivision,
        //             UnitCodeId = mrp.UnitCodeId,
        //             UnitNameTw = mrp.UnitNameTw,
        //             StyleNo = o.StyleNo,
        //             Usage = mrp.Usage,
        //         }
        //     )
        //     .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
        //     .Distinct()
        //     .ToList();

        //     var items = mrpItem.Union(mrpItemOrders);
        //     var bomItems = items
        //         .GroupBy(i => new { i.LocaleId, i.OrdersId, i.OrderNo, i.Id, i.ParentMaterialId, i.Material, i.MaterialEng, i.UnitCodeId, i.CategoryCodeId, i.SemiGoods, i.SamplingMethod, i.TextureCodeId, i.AlternateType, i.StyleNo, i.POItemId })
        //         .Select(i => new
        //         {
        //             LocaleId = i.Key.LocaleId,
        //             OrdersId = i.Key.OrdersId,
        //             OrderNo = i.Key.OrderNo,
        //             MaterialId = i.Key.Id,
        //             Material = i.Key.Material,
        //             MaterialEng = i.Key.MaterialEng,
        //             ParentMaterialId = i.Key.ParentMaterialId,
        //             UnitCodeId = i.Key.UnitCodeId,
        //             CategoryCodeId = i.Key.CategoryCodeId,
        //             SizeDivision = i.Key.AlternateType,
        //             TotalUsage = i.Sum(g => g.Usage),
        //             StyleNo = i.Key.StyleNo,
        //             POItemId = i.Key.POItemId,
        //             SamplingMethod = i.Key.SamplingMethod,
        //             TextureCodeId = i.Key.TextureCodeId,
        //             SemiGoods = i.Key.SemiGoods,
        //             AlternateType = i.Key.AlternateType,
        //         })
        //         .OrderBy(i => i.Material)
        //         .ToList();

        //     if (bomItems.Count() == 0)
        //     {
        //         return (new List<ERP.Models.Views.MaterialForPO> { }).AsQueryable();
        //     }
        //     var mIds = bomItems.Select(i => i.MaterialId).Distinct().ToList();
        //     var lPOIds = bomItems.Select(i => i.POItemId).Distinct().ToList();
        //     var localeId = bomItems[0].LocaleId;
        //     var lastPOItems = POItem.Get()
        //             .Where(i => lPOIds.Contains(i.Id) && i.LocaleId == localeId)
        //             .Select(i => new
        //             {
        //                 i.MaterialId,
        //                 i.ParentMaterialId,
        //                 i.LocaleId,
        //                 i.UnitCodeId,
        //                 i.UnitPrice,
        //                 i.DollarCodeId,
        //                 i.PayDollarCodeId,
        //                 i.PaymentCodeId,
        //                 i.PaymentPoint,
        //                 i.SamplingMethod,
        //                 i.VendorId,
        //                 i.VendorNameTw,
        //                 i.PODate,
        //                 i.PayCodeId,
        //             })
        //             .ToList()  // 在数据库中执行上述查询，并将结果加载到内存中
        //             .GroupBy(i => new { i.MaterialId, i.ParentMaterialId })
        //             .Select(g => g.OrderByDescending(i => i.PODate).FirstOrDefault())
        //             .ToList();

        //     var materials = bomItems.AsQueryable().Select((i, index) => new Models.Views.MaterialForPO
        //     {
        //         // Id = index + 1, // 分配序列号，从1开始
        //         // MaterialId = i.MaterialId,
        //         // LocaleId = i.LocaleId,
        //         // Material = i.Material,
        //         // MaterialEng = i.MaterialEng,
        //         // CategoryCodeId = i.CategoryCodeId,
        //         // SemiGoods = i.SemiGoods,
        //         // TextureCodeId = i.TextureCodeId,
        //         // OrderNo = i.OrderNo,
        //         // OrdersId = i.OrdersId,
        //         Id = index + 1, // 分配序列号，从1开始
        //         MaterialId = i.MaterialId,
        //         LocaleId = i.LocaleId,
        //         Material = i.Material,
        //         MaterialEng = i.MaterialEng,
        //         CategoryCodeId = i.CategoryCodeId,
        //         SemiGoods = i.SemiGoods,
        //         TextureCodeId = i.TextureCodeId,
        //         UnitCodeId = i.UnitCodeId,
        //         OrdersId = i.OrdersId,
        //         OrderNo = i.OrderNo,
        //         StyleNo = i.StyleNo,
        //         ParentMaterialId = i.ParentMaterialId,
        //         Usage = i.TotalUsage,
        //         AlternateType = i.AlternateType,
        //     })
        //     .ToList();

        //     materials.ForEach(i =>
        //     {
        //         var _poItem = lastPOItems.Where(p => p.MaterialId == i.MaterialId && p.LocaleId == i.LocaleId).FirstOrDefault();
        //         if (_poItem != null)
        //         {
        //             i.HasQuot = 1;
        //             i.UnitCodeId = _poItem.UnitCodeId;
        //             i.UnitPrice = _poItem.UnitPrice;
        //             i.DollarCodeId = _poItem.DollarCodeId;
        //             i.PayCodeId = _poItem.PayCodeId;
        //             i.PaymentCodeId = _poItem.PaymentCodeId;
        //             i.PaymentPoint = _poItem.PaymentPoint;
        //             i.SamplingMethod = _poItem.SamplingMethod;
        //             i.VendorId = _poItem.VendorId;
        //             i.VendorShortNameTw = _poItem.VendorNameTw;

        //             i.PurUnitCodeId = _poItem.UnitCodeId;
        //             i.PurUnitPrice = _poItem.UnitPrice;
        //             i.PurDollarCodeId = _poItem.DollarCodeId;
        //             i.PurPayDollarCodeId = _poItem.PayDollarCodeId;
        //             i.PurPaymentCodeId = _poItem.PaymentCodeId;
        //             i.PurPaymentPoint = _poItem.PaymentPoint;
        //             i.PurSamplingMethod = _poItem.SamplingMethod;
        //             i.PurVendorId = _poItem.VendorId;
        //             i.PurVendorNameTw = _poItem.VendorNameTw;
        //             i.PurPayCodeId = _poItem.PayCodeId;

        //             i.LastPODate = _poItem.PODate;
        //         }
        //     });
        //     return materials.AsQueryable();
        // }
        // public IQueryable<ERP.Models.Views.MaterialForPO> GetAllMaterialForImport(string predicate)
        // {
        //     var mrpItem = (
        //         from m in Material.Get()
        //         select new
        //         {
        //             Id = m.Id,
        //             LocaleId = m.LocaleId,
        //             Material = m.MaterialName,
        //             MaterialEng = m.MaterialNameEng,
        //             SamplingMethod = m.SamplingMethod,
        //             CategoryCodeId = m.CategoryCodeId,
        //             SemiGoods = m.SemiGoods,
        //             TextureCodeId = m.TextureCodeId,
        //             POItemId = POItem.GetSimple().Where(i => i.Status != 2 && i.MaterialId == m.Id && i.LocaleId == m.LocaleId).OrderByDescending(i => i.PODate).Select(i => i.Id).FirstOrDefault(),
        //         }
        //     )
        //     .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
        //     .Distinct()
        //     .ToList();

        //     var bomItems = mrpItem
        //         .GroupBy(i => new { i.LocaleId, i.Id, i.Material, i.MaterialEng, i.CategoryCodeId, i.SemiGoods, i.SamplingMethod, i.TextureCodeId, i.POItemId })
        //         .Select(i => new
        //         {
        //             LocaleId = i.Key.LocaleId,
        //             MaterialId = i.Key.Id,
        //             Material = i.Key.Material,
        //             MaterialEng = i.Key.MaterialEng,
        //             CategoryCodeId = i.Key.CategoryCodeId,
        //             POItemId = i.Key.POItemId,
        //             SamplingMethod = i.Key.SamplingMethod,
        //             TextureCodeId = i.Key.TextureCodeId,
        //             SemiGoods = i.Key.SemiGoods,
        //         })
        //         .OrderBy(i => i.Material)
        //         .ToList();

        //     if (bomItems.Count() == 0)
        //     {
        //         return (new List<ERP.Models.Views.MaterialForPO> { }).AsQueryable();
        //     }
        //     var mIds = bomItems.Select(i => i.MaterialId).Distinct().ToList();
        //     var lPOIds = bomItems.Select(i => i.POItemId).Distinct().ToList();
        //     var localeId = bomItems[0].LocaleId;
        //     var lastPOItems = POItem.Get()
        //             .Where(i => lPOIds.Contains(i.Id) && i.LocaleId == localeId)
        //             .Select(i => new
        //             {
        //                 i.MaterialId,
        //                 i.ParentMaterialId,
        //                 i.LocaleId,
        //                 i.UnitCodeId,
        //                 i.UnitPrice,
        //                 i.DollarCodeId,
        //                 i.PayDollarCodeId,
        //                 i.PaymentCodeId,
        //                 i.PaymentPoint,
        //                 i.SamplingMethod,
        //                 i.VendorId,
        //                 i.VendorNameTw,
        //                 i.PODate,
        //                 i.PayCodeId,
        //             })
        //             .ToList()  // 在数据库中执行上述查询，并将结果加载到内存中
        //             .GroupBy(i => new { i.MaterialId, i.ParentMaterialId })
        //             .Select(g => g.OrderByDescending(i => i.PODate).FirstOrDefault())
        //             .ToList();

        //     var materials = bomItems.AsQueryable().Select((i, index) => new Models.Views.MaterialForPO
        //     {
        //         // Id = index + 1, // 分配序列号，从1开始
        //         // MaterialId = i.MaterialId,
        //         // LocaleId = i.LocaleId,
        //         // Material = i.Material,
        //         // MaterialEng = i.MaterialEng,
        //         // CategoryCodeId = i.CategoryCodeId,
        //         // SemiGoods = i.SemiGoods,
        //         // TextureCodeId = i.TextureCodeId,
        //         // OrderNo = i.OrderNo,
        //         // OrdersId = i.OrdersId,
        //         Id = index + 1, // 分配序列号，从1开始
        //         MaterialId = i.MaterialId,
        //         LocaleId = i.LocaleId,
        //         Material = i.Material,
        //         MaterialEng = i.MaterialEng,
        //         CategoryCodeId = i.CategoryCodeId,
        //         SemiGoods = i.SemiGoods,
        //         TextureCodeId = i.TextureCodeId,
        //         UnitCodeId = 0,
        //         OrdersId = 0,
        //         OrderNo = "",
        //         StyleNo = "",
        //         ParentMaterialId = 0,
        //         Usage = 0,
        //         // AlternateType = i.AlternateType,
        //     })
        //     .ToList();

        //     materials.ForEach(i =>
        //     {
        //         var _poItem = lastPOItems.Where(p => p.MaterialId == i.MaterialId && p.LocaleId == i.LocaleId).FirstOrDefault();
        //         if (_poItem != null)
        //         {
        //             i.HasQuot = 1;
        //             i.UnitCodeId = _poItem.UnitCodeId;
        //             i.UnitPrice = _poItem.UnitPrice;
        //             i.DollarCodeId = _poItem.DollarCodeId;
        //             i.PayCodeId = _poItem.PayCodeId;
        //             i.PaymentCodeId = _poItem.PaymentCodeId;
        //             i.PaymentPoint = _poItem.PaymentPoint;
        //             i.SamplingMethod = _poItem.SamplingMethod;
        //             i.VendorId = _poItem.VendorId;
        //             i.VendorShortNameTw = _poItem.VendorNameTw;

        //             i.PurUnitCodeId = _poItem.UnitCodeId;
        //             i.PurUnitPrice = _poItem.UnitPrice;
        //             i.PurDollarCodeId = _poItem.DollarCodeId;
        //             i.PurPayDollarCodeId = _poItem.PayDollarCodeId;
        //             i.PurPaymentCodeId = _poItem.PaymentCodeId;
        //             i.PurPaymentPoint = _poItem.PaymentPoint;
        //             i.PurSamplingMethod = _poItem.SamplingMethod;
        //             i.PurVendorId = _poItem.VendorId;
        //             i.PurVendorNameTw = _poItem.VendorNameTw;
        //             i.PurPayCodeId = _poItem.PayCodeId;

        //             i.LastPODate = _poItem.PODate;
        //         }
        //     });
        //     return materials.AsQueryable();
        // }

        // 取得報價單
        public ERP.Models.Views.MaterialQuot GetMaterialQuotation(int materialId, int vendorId, int localeId)
        {
            var quotation = new ERP.Models.Views.MaterialQuot();

            var _quotation = MaterialQuot.Get().Where(i => i.Enable == 1 && i.MaterialId == materialId && i.VendorId == vendorId && i.LocaleId == localeId).OrderByDescending(i => i.EffectiveDate).FirstOrDefault();

            quotation = _quotation == null ? quotation : _quotation;

            return quotation;
        }
    }
}
