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
    public class PurPlanService : BusinessService
    {
        private ERP.Services.Business.Entities.PurBatchService PurBatch { get; set; }
        private ERP.Services.Business.Entities.PurOrdersItemService PurOrdersItem { get; set; }
        private ERP.Services.Business.Entities.PurBatchItemService PurBatchItem { get; set; }
        private ERP.Services.Business.Entities.MRPItemService MRPItem { get; set; }
        private ERP.Services.Business.Entities.MRPItemOrdersService MRPItemOrders { get; set; }
        private ERP.Services.Business.Entities.POItemService POItem { get; set; }
        private ERP.Services.Business.Entities.MaterialQuotService Quotation { get; set; }

        private ERP.Services.Entities.OrdersService Orders { get; set; }
        public PurPlanService(
            ERP.Services.Business.Entities.PurBatchService purBatchService,
            ERP.Services.Business.Entities.PurOrdersItemService purOrdersItemService,
            ERP.Services.Business.Entities.PurBatchItemService purBatchItemService,
            ERP.Services.Business.Entities.MRPItemService mrpItemService,
            ERP.Services.Business.Entities.MRPItemOrdersService mrpItemOrdersService,
            ERP.Services.Business.Entities.POItemService poItemService,
            ERP.Services.Business.Entities.MaterialQuotService materialQuotService,
            ERP.Services.Entities.OrdersService ordersService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            PurBatch = purBatchService;
            PurOrdersItem = purOrdersItemService;
            PurBatchItem = purBatchItemService;
            MRPItem = mrpItemService;
            MRPItemOrders = mrpItemOrdersService;
            Orders = ordersService;
            POItem = poItemService;
            Quotation = materialQuotService;
        }
        public IQueryable<Models.Views.PurBatch> GetWithItem(string predicate)
        {
            var result = (
                from p in PurBatch.Get()
                join pi in PurOrdersItem.Get() on new { PurId = p.Id, LocaleId = p.LocaleId } equals new { PurId = pi.PurBatchId, LocaleId = pi.LocaleId } into apiGRP
                from pi in apiGRP.DefaultIfEmpty()
                select new
                {
                    Id = p.Id,
                    LocaleId = p.LocaleId,
                    OrderNo = pi.OrderNo,
                    CompanyId = pi.CompanyId,
                    BatchDate = p.BatchDate,
                    BatchNo = p.BatchNo,
                    RefLocaleId = p.RefLocaleId,
                    ModifyUserName = p.ModifyUserName,
                    LastUpdateTime = p.LastUpdateTime,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new ERP.Models.Views.PurBatch
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                BatchNo = i.BatchNo,
                BatchDate = i.BatchDate,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                RefLocaleId = i.RefLocaleId,
            })
            .Distinct();
            return result;
        }
        public ERP.Models.Views.PurPlanGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.PurPlanGroup { };
            var purBatch = PurBatch.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            var purOrdersItem = PurOrdersItem.Get().Where(i => i.PurBatchId == id && i.LocaleId == localeId).OrderBy(i => i.OrderNo).ToList();

            if (purBatch != null && purOrdersItem.Any())
            {
                group.PurBatch = purBatch;
                group.PurOrdersItem = purOrdersItem;
                // group.PurBatchItem = purPlanItem;
            }
            return group;
        }
        public ERP.Models.Views.PurPlanGroup Save(PurPlanGroup item)
        {
            var purBatch = item.PurBatch;
            var purOrdersItems = item.PurOrdersItem.ToList();
            var purBatchItems = item.PurBatchItem.ToList();

            if (purBatch != null)
            {
                try
                {
                    // delete from PurBatchItem  where OrdersId =22832 and MaterialId =15254 and ParentMaterialId =0 and (POItemId is null or LEN(POItemId)<=0) and (PurLocaleId >7 or PurLocaleId <7) and LocaleId =7
                    UnitOfWork.BeginTransaction();
                    purBatchItems.ForEach(i =>
                    {
                        PurBatchItem.RemoveRange(p => p.BatchId == purBatch.Id && p.LocaleId == purBatch.LocaleId && p.OrdersId == i.OrdersId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId && (p.POItemId == 0 || p.POItemId == null));
                        i.POItemId = null;
                        i.PurQty = (decimal)i.NewPurQty;
                    });
                    PurBatchItem.CreateRange(purBatchItems);

                    UnitOfWork.Commit();
                }
                catch (Exception e)
                {
                    UnitOfWork.Rollback();
                    throw e;
                }
            }
            return Get((int)purBatch.Id, (int)purBatch.LocaleId);
        }
        public void Remove(PurPlanGroup item)
        {
            var purBatch = item.PurBatch;
            var purOrdersItems = item.PurOrdersItem.ToList();
            var purBatchItems = item.PurBatchItem.ToList();

            if (purBatch != null)
            {
                try
                {
                    // delete from PurBatchItem  where OrdersId =22832 and MaterialId =15254 and ParentMaterialId =0 and (POItemId is null or LEN(POItemId)<=0) and (PurLocaleId >7 or PurLocaleId <7) and LocaleId =7
                    UnitOfWork.BeginTransaction();
                    var items = purBatchItems.Select(i => new { i.BatchId, i.LocaleId, i.OrdersId, i.MaterialId, i.ParentMaterialId }).Distinct().ToList();
                    items.ForEach(i =>
                    {
                        PurBatchItem.RemoveRange(p => p.BatchId == purBatch.Id && p.LocaleId == purBatch.LocaleId && p.OrdersId == i.OrdersId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId && (p.POItemId == 0 || p.POItemId == null));
                    });
                    // purBatchItems.ForEach(i =>
                    // {
                    //     PurBatchItem.RemoveRange(p => p.BatchId == purBatch.Id && p.LocaleId == purBatch.LocaleId && p.OrdersId == i.OrdersId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId && (p.POItemId == 0 || p.POItemId == null));
                    // });

                    UnitOfWork.Commit();
                }
                catch (Exception e)
                {
                    UnitOfWork.Rollback();
                    throw e;
                }
            }
        }

        public Models.Views.PurBatchItemGroup BuildPurPlanItemGroup(string predicate, string[] filters)
        {

            // step1: get all Item from BuildPurPlanItem
            var purBatchItem = BuildPurPlanItem(predicate, filters).ToList();
            if (purBatchItem.Any())
            {
                var localeId = purBatchItem.First().LocaleId;
                var materialId = purBatchItem.Select(i => i.MaterialId).Distinct().ToList();

                // step2: get Quotation from  Step1'a Item
                var quotations = Quotation.Get()
                    .Where(i => materialId.Contains(i.MaterialId) && i.LocaleId == localeId && i.Enable == 1 && i.QuotType == 1)
                    .Select(i => new MaterialSimpleQuot
                    {
                        Id = i.Id,
                        LocaleId = i.LocaleId,
                        MaterialId = i.MaterialId,
                        VendorId = i.VendorId,
                        UnitCodeId = i.UnitCodeId,
                        UnitPrice = i.UnitPrice,
                        DollarCodeId = i.DollarCodeId,
                        PayCodeId = i.PayCodeId,
                        EffectiveDate = i.EffectiveDate,
                        QuotType = i.QuotType,
                        Enable = i.Enable
                    })
                    .ToList();

                var quotaGroup = quotations
                    .GroupBy(i => new { i.MaterialId, i.VendorId })
                    .Select(g => g.OrderByDescending(i => i.EffectiveDate).FirstOrDefault())
                    .ToList();

                // step2: get Quotation from  Step1'a Item
                purBatchItem.ForEach(i =>
                {
                    var quotation = quotaGroup.Where(q => q.MaterialId == i.MaterialId && q.VendorId == i.VendorId).OrderByDescending(i => i.EffectiveDate).FirstOrDefault();

                    //20250417 取消這段，這裡會造成不是這個廠商，但單價是領外一個廠商的問題
                    // if (quotation == null)
                    // {
                    //     quotation = quotaGroup.Where(q => q.MaterialId == i.MaterialId).OrderByDescending(i => i.EffectiveDate).FirstOrDefault();
                    //     i.PurUnitPrice = 0;
                    //     i.RefQuotId = quotation.Id;
                    // }

                    if (quotation != null)
                    {
                        i.DollarCodeId = quotation.DollarCodeId;
                        i.PayCodeId = quotation.PayCodeId;
                        i.PurUnitPrice = quotation.UnitPrice;
                        // i.PurUnitCodeId = quotation.UnitCodeId;
                        i.PayDollarCodeId = quotation.DollarCodeId;
                        i.RefQuotId = quotation.Id;
                        i.VendorId = quotation.VendorId;
                    }
                    else
                    {
                        i.PurUnitPrice = 0;
                    }

                    i.NewPurQty = i.PurQty == 0 ? Math.Ceiling(i.PlanQty) : Math.Ceiling((decimal)i.NewPurQty);
                });

                return new Models.Views.PurBatchItemGroup
                {
                    PurBatchItem = purBatchItem,
                    MaterialQuot = quotaGroup,
                };
            }

            return new Models.Views.PurBatchItemGroup { };
        }
        public IQueryable<Models.Views.PurBatchItem> BuildPurPlanItem(string predicate, string[] filters)
        {
            var batchId = 0;
            var localeId = 0;
            List<decimal> orderIds = new List<decimal> { };

            bool withoutPO = false;
            bool withoutZero = true;
            // extend condition, obj by ExtentionItem
            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                batchId = (int)extenFilters.Field1;
                localeId = (int)extenFilters.Field2;
                withoutPO = extenFilters.Field9;
                withoutZero = extenFilters.Field10;
                orderIds = Array.ConvertAll(extenFilters.Field3, i => (decimal)i).ToList();
            }

            var purOrdersItem = PurOrdersItem.Get().Where(i => i.PurBatchId == batchId && i.LocaleId == localeId).OrderBy(i => i.OrderNo).ToList();

            // setp1: get MRPItem
            // setp2: get MRPItemOrders
            // setp3: combind MPRItem+MRPItemOrders
            // setp4: get POItem, 4.1 get all marterilaId, 4.2 get latesstPOItem by material, 4.3 get purbatchItem, 4.4 get poitem by Batch 

            // setp1: MRPItem
            var mrpItem = MRPItem.Get()
                .Where(i => orderIds.Contains(i.OrdersId) && i.LocaleId == localeId)
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .GroupBy(i => new { i.MaterialId, i.MaterialNameEn, i.MaterialNameTw, i.CategoryCodeId, i.SemiGoods, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw, i.MRPVersion })
                .Select(i => new
                {
                    LocaleId = i.Key.LocaleId,
                    // PartId = i.PartId,
                    ParentMaterialId = i.Key.ParentMaterialId,
                    OrdersId = i.Key.OrdersId,
                    UnitCodeId = i.Key.UnitCodeId,
                    MaterialId = i.Key.MaterialId,
                    MaterialNameTw = i.Key.MaterialNameTw,
                    MaterialNameEn = i.Key.MaterialNameEn,
                    Total = i.Sum(g => g.Total), //i.Usage,
                    Type = 1,
                    CategoryCodeId = i.Key.CategoryCodeId,
                    SizeDivision = i.Key.SizeDivision,
                    SemiGoods = i.Key.SemiGoods,
                    // POItemId = POItem.GetSimple().Where(p => p.Status != 2 && p.MaterialId == i.Key.MaterialId && p.LocaleId == i.Key.LocaleId).OrderByDescending(p => p.PODate).ThenByDescending(p => p.Id).FirstOrDefault().Id,
                    MRPVersion = i.Key.MRPVersion,
                })
                .ToList();

            // setp2: MRPOrderItem
            var mrpItemOrders = MRPItemOrders.Get()
                .Where(i => orderIds.Contains(i.OrdersId) && i.LocaleId == localeId)
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .GroupBy(i => new { i.MaterialId, i.MaterialNameEn, i.MaterialNameTw, i.CategoryCodeId, i.SemiGoods, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw, i.MRPVersion })
                .Select(i => new
                {
                    LocaleId = i.Key.LocaleId,
                    // PartId = i.PartId,
                    ParentMaterialId = i.Key.ParentMaterialId,
                    OrdersId = i.Key.OrdersId,
                    UnitCodeId = i.Key.UnitCodeId,
                    MaterialId = i.Key.MaterialId,
                    MaterialNameTw = i.Key.MaterialNameTw,
                    MaterialNameEn = i.Key.MaterialNameEn,
                    Total = i.Sum(g => g.Total), //i.Usage,
                    Type = 2,
                    CategoryCodeId = i.Key.CategoryCodeId,
                    SizeDivision = i.Key.SizeDivision,
                    SemiGoods = i.Key.SemiGoods,
                    // POItemId = POItem.GetSimple().Where(p => p.Status != 2 && p.MaterialId == i.Key.MaterialId && p.LocaleId == i.Key.LocaleId).OrderByDescending(p => p.PODate).ThenByDescending(p => p.Id).FirstOrDefault().Id,
                    MRPVersion = i.Key.MRPVersion,
                }).ToList();

            // setp3: combind MPRItem+MRPItemOrders
            var items = mrpItem.Union(mrpItemOrders);
            var bomItems = items.GroupBy(i => new { i.LocaleId, i.OrdersId, i.MaterialId, i.MaterialNameTw, i.MaterialNameEn, i.ParentMaterialId, i.UnitCodeId, i.CategoryCodeId, i.SizeDivision, i.SemiGoods, i.MRPVersion })
                                .Select(i => new
                                {
                                    LocaleId = i.Key.LocaleId,
                                    OrdersId = i.Key.OrdersId,
                                    MaterialId = i.Key.MaterialId,
                                    MaterialNameTw = i.Key.MaterialNameTw,
                                    MaterialNameEn = i.Key.MaterialNameEn,
                                    ParentMaterialId = i.Key.ParentMaterialId,
                                    UnitCodeId = i.Key.UnitCodeId,
                                    CategoryCodeId = i.Key.CategoryCodeId,
                                    SizeDivision = i.Key.SizeDivision,
                                    SemiGoods = i.Key.SemiGoods,
                                    // POItemId = i.Key.POItemId,
                                    MRPVersion = i.Key.MRPVersion,
                                    TotalUsage = i.Sum(g => g.Total),
                                }).ToList();

            // step 4: 透過MRPItemt產生計畫，同時依照這些材料找到過去的採購記錄

            // 4.1 取得所有材料Id
            var mIds = bomItems.Select(i => i.MaterialId).Distinct();
            // 4.2 取得這些材料最後一張採購單
            var lastPOItems = POItem.GetLatest().Where(i => i.Status != 2 && i.LocaleId == localeId && mIds.Contains(i.MaterialId)).ToList();

            // 4.3 這個批次裡所有買過的計畫、整張訂單有買過的採購單，所以採購單的數量可能會大過計畫，排除掉計畫、採購單作廢的
            var purBatchItems = PurBatchItem.Get().Where(i => i.Status != 2 && i.LocaleId == localeId && orderIds.Contains(i.OrdersId) && i.BatchId == batchId && mIds.Contains(i.MaterialId)).ToList();
            // 4.4 取得這個批次裡所有買過的該材料的採購單
            var poItems = POItem.GetSimple().Where(i => i.Status != 2 && i.LocaleId == localeId && orderIds.Contains(i.OrdersId) && mIds.Contains(i.MaterialId)).ToList();

            
            // 從管制表採料轉寄劃
            var purPlanItem = (
                    from m in bomItems
                    select new ERP.Models.Views.PurBatchItem
                    {
                        Id = 0,
                        LocaleId = m.LocaleId,
                        BatchId = batchId,
                        OrdersId = m.OrdersId,
                        MaterialId = m.MaterialId,
                        ParentMaterialId = m.ParentMaterialId,
                        PlanUnitCodeId = m.UnitCodeId,
                        PurUnitCodeId = m.UnitCodeId,

                        PlanQty = m.TotalUsage,
                        SemiGoods = m.SemiGoods,
                        RefQuotId = 0,
                        VendorId = 0,
                        Vendor = "",
                        PurUnitPrice = 0,
                        DollarCodeId = 0,
                        PayCodeId = 0,
                        PurLocaleId = m.LocaleId,
                        ReceivingLocaleId = m.LocaleId,
                        PaymentLocaleId = m.LocaleId,
                        POItemId = 0,
                        PayDollarCodeId = 0,
                        RefLocaleId = m.LocaleId,
                        OnHandQty = m.ParentMaterialId > 0 ? 1 : 0,
                        RefItemId = 0,
                        MaterialNameTw = m.MaterialNameTw,
                        MaterialNameEn = m.MaterialNameEn,
                        OrderNo = purOrdersItem.Where(o => o.OrdersId == m.OrdersId).Max(i => i.OrderNo),
                        LCSD = purOrdersItem.Where(o => o.OrdersId == m.OrdersId).Max(i => i.LCSD),
                        Seq = m.ParentMaterialId > 0 ? m.ParentMaterialId.ToString() + m.OrdersId.ToString() + m.MaterialId.ToString() :
                              m.MaterialId.ToString() + m.OrdersId.ToString(),
                        PurQty = 0,
                        POQty = 0,
                        NewPurQty = 0,
                        CategoryCodeId = m.CategoryCodeId,
                        AlternateType = m.SizeDivision,
                        MRPVersion = m.MRPVersion,
                    }
                )
                .OrderBy(i => i.Seq)
                .ToList();
            // update lastPOItem, Vendor, Price, Unit,Dollar
            purPlanItem.ForEach(i =>
            {
                // 把有採購計劃的最後新一筆更新在系統裡
                i.Id = 0;

                var _purBatchItem = purBatchItems.Where(p => p.OrdersId == i.OrdersId && p.LocaleId == i.LocaleId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId && i.Status != 2).OrderByDescending(i => i.LastUpdateTime).FirstOrDefault();
                if (_purBatchItem != null)
                {
                    i.Id = _purBatchItem.Id;
                    i.ReceivingLocaleId = _purBatchItem.ReceivingLocaleId;
                    i.PaymentLocaleId = _purBatchItem.PaymentLocaleId;
                    i.PayCodeId = _purBatchItem.PayCodeId;

                    i.PurUnitPrice = (decimal)_purBatchItem.PurUnitPrice;
                    i.DollarCodeId = _purBatchItem.DollarCodeId;
                    i.VendorId = _purBatchItem.VendorId;
                    i.PayDollarCodeId = _purBatchItem.DollarCodeId;
                }

                // 抓最後一個採購單，沒有就用採購計劃來放入，如果都沒就是空的全新的資料
                var _poItem = lastPOItems.Where(p => p.MaterialId == i.MaterialId && p.LocaleId == i.LocaleId).OrderByDescending(i => i.PODate).ThenByDescending(i => i.Id).FirstOrDefault();
                if (_poItem != null)
                {
                    i.PurUnitPrice = (decimal)_poItem.UnitPrice;
                    i.DollarCodeId = _poItem.DollarCodeId;
                    i.VendorId = _poItem.VendorId;
                    i.PayDollarCodeId = _poItem.DollarCodeId;
                }

                // 更新清單量、已採購量
                i.PurQty = purBatchItems.Where(p => p.OrdersId == i.OrdersId && p.LocaleId == i.LocaleId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId && i.Status != 2).Sum(p => p.PurQty);
                i.POQty = poItems.Where(p => p.OrdersId == i.OrdersId && p.LocaleId == i.LocaleId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId && p.Status != 2).Sum(p => p.Qty);
                i.NewPurQty = i.PurQty >= i.PlanQty ? 0 : i.PlanQty - i.PurQty;
                i.POItemId = poItems.Where(p => p.OrdersId == i.OrdersId && p.LocaleId == i.LocaleId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId && p.POType == 1 && p.Status != 2).OrderByDescending(p => p.PODate).Select(p => p.Id).FirstOrDefault();
            });

            // display item without po
            if (withoutPO)
            {
                purPlanItem = purPlanItem.Where(i => !(i.POItemId > 0)).OrderBy(i => i.Seq).ToList();
            }

            if (withoutZero)
            {
                purPlanItem = purPlanItem.Where(i => i.PlanQty > 0).OrderBy(i => i.Seq).ToList();
            }
            return purPlanItem.AsQueryable();
        }


        public IQueryable<Models.Views.PurBatchItem> BuildPurPlanItem710(string predicate, string[] filters)
        {
            var batchId = 0;
            var localeId = 0;
            List<decimal> orderIds = new List<decimal> { };

            bool withoutPO = false;
            bool withoutZero = true;
            // extend condition, obj by ExtentionItem
            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                batchId = (int)extenFilters.Field1;
                localeId = (int)extenFilters.Field2;
                withoutPO = extenFilters.Field9;
                withoutZero = extenFilters.Field10;
                orderIds = Array.ConvertAll(extenFilters.Field3, i => (decimal)i).ToList();
            }

            var purOrdersItem = PurOrdersItem.Get().Where(i => i.PurBatchId == batchId && i.LocaleId == localeId).OrderBy(i => i.OrderNo).ToList();

            // setp1: get MRPItem
            // setp2: get MRPItemOrders
            // setp3: combind MPRItem+MRPItemOrders
            // setp4: get POItem

            // MRPItem
            var mrpItem = MRPItem.Get()
                .Where(i => orderIds.Contains(i.OrdersId) && i.LocaleId == localeId)
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .GroupBy(i => new { i.MaterialId, i.MaterialNameEn, i.MaterialNameTw, i.CategoryCodeId, i.SemiGoods, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw, i.MRPVersion })
                .Select(i => new
                {
                    LocaleId = i.Key.LocaleId,
                    // PartId = i.PartId,
                    ParentMaterialId = i.Key.ParentMaterialId,
                    OrdersId = i.Key.OrdersId,
                    UnitCodeId = i.Key.UnitCodeId,
                    MaterialId = i.Key.MaterialId,
                    MaterialNameTw = i.Key.MaterialNameTw,
                    MaterialNameEn = i.Key.MaterialNameEn,
                    Total = i.Sum(g => g.Total), //i.Usage,
                    Type = 1,
                    CategoryCodeId = i.Key.CategoryCodeId,
                    SizeDivision = i.Key.SizeDivision,
                    SemiGoods = i.Key.SemiGoods,
                    POItemId = POItem.GetSimple().Where(p => p.Status != 2 && p.MaterialId == i.Key.MaterialId && p.LocaleId == i.Key.LocaleId).OrderByDescending(p => p.PODate).ThenByDescending(p => p.Id).FirstOrDefault().Id,
                    MRPVersion = i.Key.MRPVersion,
                })
                .ToList();

            // MRPOrderItem
            var mrpItemOrders = MRPItemOrders.Get()
                .Where(i => orderIds.Contains(i.OrdersId) && i.LocaleId == localeId)
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .GroupBy(i => new { i.MaterialId, i.MaterialNameEn, i.MaterialNameTw, i.CategoryCodeId, i.SemiGoods, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw, i.MRPVersion })
                .Select(i => new
                {
                    LocaleId = i.Key.LocaleId,
                    // PartId = i.PartId,
                    ParentMaterialId = i.Key.ParentMaterialId,
                    OrdersId = i.Key.OrdersId,
                    UnitCodeId = i.Key.UnitCodeId,
                    MaterialId = i.Key.MaterialId,
                    MaterialNameTw = i.Key.MaterialNameTw,
                    MaterialNameEn = i.Key.MaterialNameEn,
                    Total = i.Sum(g => g.Total), //i.Usage,
                    Type = 2,
                    CategoryCodeId = i.Key.CategoryCodeId,
                    SizeDivision = i.Key.SizeDivision,
                    SemiGoods = i.Key.SemiGoods,
                    POItemId = POItem.GetSimple().Where(p => p.Status != 2 && p.MaterialId == i.Key.MaterialId && p.LocaleId == i.Key.LocaleId).OrderByDescending(p => p.PODate).ThenByDescending(p => p.Id).FirstOrDefault().Id,
                    MRPVersion = i.Key.MRPVersion,
                }).ToList();

            var items = mrpItem.Union(mrpItemOrders);
            var bomItems = items.GroupBy(i => new { i.LocaleId, i.OrdersId, i.MaterialId, i.MaterialNameTw, i.MaterialNameEn, i.ParentMaterialId, i.UnitCodeId, i.CategoryCodeId, i.SizeDivision, i.SemiGoods, i.POItemId, i.MRPVersion })
                                .Select(i => new
                                {
                                    LocaleId = i.Key.LocaleId,
                                    OrdersId = i.Key.OrdersId,
                                    MaterialId = i.Key.MaterialId,
                                    MaterialNameTw = i.Key.MaterialNameTw,
                                    MaterialNameEn = i.Key.MaterialNameEn,
                                    ParentMaterialId = i.Key.ParentMaterialId,
                                    UnitCodeId = i.Key.UnitCodeId,
                                    CategoryCodeId = i.Key.CategoryCodeId,
                                    SizeDivision = i.Key.SizeDivision,
                                    SemiGoods = i.Key.SemiGoods,
                                    POItemId = i.Key.POItemId,
                                    MRPVersion = i.Key.MRPVersion,
                                    TotalUsage = i.Sum(g => g.Total),
                                }).ToList();

            // Step3: 透過MRPItemt產生計畫，同時依照這些材料找到過去的採購記錄
            var mIds = bomItems.Select(i => i.MaterialId).Distinct();
            // 取該材料最後買的採購單
            var poItemIds = bomItems.Select(i => i.POItemId).Distinct().ToList();
            var lastPOItems = POItem.Get()
                .Where(i => i.LocaleId == localeId && poItemIds.Contains(i.Id) && i.Status != 2)
                .Select(i => new
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    MaterialId = i.MaterialId,
                    ParentMaterialId = i.ParentMaterialId,
                    PODate = i.PODate,
                    VendorId = i.VendorId,
                    Vendor = i.VendorNameTw,
                    UnitCodeId = i.UnitCodeId,
                    UnitPrice = i.UnitPrice,
                    DollarCodeId = i.DollarCodeId,
                })
                .Distinct()
                .ToList();

            //這個批次裡所有買過的計畫、整張訂單有買過的採購單，所以採購單的數量可能會大過計畫，排除掉計畫、採購單作廢的
            // 取得這個批次裡所有買過的該材料的採購單
            var purBatchItems = PurBatchItem.Get().Where(i => i.Status != 2 && i.LocaleId == localeId && orderIds.Contains(i.OrdersId) && i.BatchId == batchId && mIds.Contains(i.MaterialId)).ToList();
            var poItems = POItem.Get().Where(i => i.Status != 2 && i.LocaleId == localeId && orderIds.Contains(i.OrdersId) && mIds.Contains(i.MaterialId)).ToList();

            // 從管制表採料轉寄劃
            var purPlanItem = (
                    from m in bomItems
                    select new ERP.Models.Views.PurBatchItem
                    {
                        Id = 0,
                        LocaleId = m.LocaleId,
                        BatchId = batchId,
                        OrdersId = m.OrdersId,
                        MaterialId = m.MaterialId,
                        ParentMaterialId = m.ParentMaterialId,
                        PlanUnitCodeId = m.UnitCodeId,
                        PurUnitCodeId = m.UnitCodeId,

                        PlanQty = m.TotalUsage,
                        SemiGoods = m.SemiGoods,
                        RefQuotId = 0,
                        VendorId = 0,
                        Vendor = "",
                        PurUnitPrice = 0,
                        DollarCodeId = 0,
                        PayCodeId = 0,
                        PurLocaleId = m.LocaleId,
                        ReceivingLocaleId = m.LocaleId,
                        PaymentLocaleId = m.LocaleId,
                        POItemId = 0,
                        PayDollarCodeId = 0,
                        RefLocaleId = m.LocaleId,
                        OnHandQty = m.ParentMaterialId > 0 ? 1 : 0,
                        RefItemId = 0,
                        MaterialNameTw = m.MaterialNameTw,
                        MaterialNameEn = m.MaterialNameEn,
                        OrderNo = purOrdersItem.Where(o => o.OrdersId == m.OrdersId).Max(i => i.OrderNo),
                        LCSD = purOrdersItem.Where(o => o.OrdersId == m.OrdersId).Max(i => i.LCSD),
                        Seq = m.ParentMaterialId > 0 ? m.ParentMaterialId.ToString() + m.OrdersId.ToString() + m.MaterialId.ToString() :
                              m.MaterialId.ToString() + m.OrdersId.ToString(),
                        PurQty = 0,
                        POQty = 0,
                        NewPurQty = 0,
                        CategoryCodeId = m.CategoryCodeId,
                        AlternateType = m.SizeDivision,
                        MRPVersion = m.MRPVersion,
                    }
                )
                .OrderBy(i => i.Seq)
                .ToList();
            // update lastPOItem, Vendor, Price, Unit,Dollar
            purPlanItem.ForEach(i =>
            {
                // 把有採購計劃的最後新一筆更新在系統裡
                i.Id = 0;

                var _purBatchItem = purBatchItems.Where(p => p.OrdersId == i.OrdersId && p.LocaleId == i.LocaleId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId && i.Status != 2).OrderByDescending(i => i.LastUpdateTime).FirstOrDefault();
                if (_purBatchItem != null)
                {
                    i.Id = _purBatchItem.Id;
                    i.ReceivingLocaleId = _purBatchItem.ReceivingLocaleId;
                    i.PaymentLocaleId = _purBatchItem.PaymentLocaleId;
                    i.PayCodeId = _purBatchItem.PayCodeId;

                    i.PurUnitPrice = (decimal)_purBatchItem.PurUnitPrice;
                    i.DollarCodeId = _purBatchItem.DollarCodeId;
                    i.VendorId = _purBatchItem.VendorId;
                    i.PayDollarCodeId = _purBatchItem.DollarCodeId;
                }

                // 抓最後一個採購單，沒有就用採購計劃來放入，如果都沒就是空的全新的資料
                var _poItem = lastPOItems.Where(p => p.MaterialId == i.MaterialId && p.LocaleId == i.LocaleId).OrderByDescending(i => i.PODate).ThenByDescending(i => i.Id).FirstOrDefault();
                if (_poItem != null)
                {
                    i.PurUnitPrice = (decimal)_poItem.UnitPrice;
                    i.DollarCodeId = _poItem.DollarCodeId;
                    i.VendorId = _poItem.VendorId;
                    i.PayDollarCodeId = _poItem.DollarCodeId;
                }

                // 更新清單量、已採購量
                i.PurQty = purBatchItems.Where(p => p.OrdersId == i.OrdersId && p.LocaleId == i.LocaleId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId && i.Status != 2).Sum(p => p.PurQty);
                i.POQty = poItems.Where(p => p.OrdersId == i.OrdersId && p.LocaleId == i.LocaleId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId && p.Status != 2).Sum(p => p.Qty);
                i.NewPurQty = i.PurQty >= i.PlanQty ? 0 : i.PlanQty - i.PurQty;
                i.POItemId = poItems.Where(p => p.OrdersId == i.OrdersId && p.LocaleId == i.LocaleId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId && p.POType == 1).OrderByDescending(p => p.PODate).Select(p => p.Id).FirstOrDefault();
            });

            // display item without po
            if (withoutPO)
            {
                purPlanItem = purPlanItem.Where(i => !(i.POItemId > 0)).OrderBy(i => i.Seq).ToList();
            }

            if (withoutZero)
            {
                purPlanItem = purPlanItem.Where(i => i.PlanQty > 0).OrderBy(i => i.Seq).ToList();
            }
            return purPlanItem.AsQueryable();
        }

        public IQueryable<Models.Views.PurBatchItem> BuildPurPlanItem1(string predicate, string[] filters)
        {
            var batchId = 0;
            var localeId = 0;
            List<decimal> orderIds = new List<decimal> { };

            bool withoutPO = false;
            bool withoutZero = true;
            // extend condition, obj by ExtentionItem
            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                batchId = (int)extenFilters.Field1;
                localeId = (int)extenFilters.Field2;
                withoutPO = extenFilters.Field9;
                withoutZero = extenFilters.Field10;
                orderIds = Array.ConvertAll(extenFilters.Field3, i => (decimal)i).ToList();
            }

            var purOrdersItem = PurOrdersItem.Get().Where(i => i.PurBatchId == batchId && i.LocaleId == localeId).OrderBy(i => i.OrderNo).ToList();

            // setp1: get MRPItem
            // setp2: get MRPItemOrders
            // setp3: combind MPRItem+MRPItemOrders
            // setp4: get POItem

            // MRPItem
            var mrpItem = MRPItem.Get()
                .GroupBy(i => new { i.MaterialId, i.MaterialNameEn, i.MaterialNameTw, i.CategoryCodeId, i.SemiGoods, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw, i.MRPVersion })
                .Select(i => new { i.Key.MaterialId, i.Key.MaterialNameEn, i.Key.MaterialNameTw, i.Key.CategoryCodeId, i.Key.SemiGoods, i.Key.OrdersId, i.Key.LocaleId, i.Key.ParentMaterialId, i.Key.SizeDivision, i.Key.UnitCodeId, i.Key.UnitNameTw, i.Key.MRPVersion, Usage = i.Sum(g => g.Total) })
                .Where(i => orderIds.Contains(i.OrdersId))
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Select(i => new
                {
                    LocaleId = i.LocaleId,
                    // PartId = i.PartId,
                    ParentMaterialId = i.ParentMaterialId,
                    OrdersId = i.OrdersId,
                    UnitCodeId = i.UnitCodeId,
                    MaterialId = i.MaterialId,
                    MaterialNameTw = i.MaterialNameTw,
                    MaterialNameEn = i.MaterialNameEn,
                    Total = i.Usage,
                    Type = 1,
                    CategoryCodeId = i.CategoryCodeId,
                    SizeDivision = i.SizeDivision,
                    SemiGoods = i.SemiGoods,
                    POItemId = POItem.Get().Where(p => p.Status != 2 && p.MaterialId == i.MaterialId && p.LocaleId == i.LocaleId).OrderByDescending(p => p.PODate).ThenByDescending(p => p.Id).FirstOrDefault().Id,
                    MRPVersion = i.MRPVersion,
                })
                .ToList();

            // MRPOrderItem
            var mrpItemOrders = MRPItemOrders.Get()
                .GroupBy(i => new { i.MaterialId, i.MaterialNameEn, i.MaterialNameTw, i.CategoryCodeId, i.SemiGoods, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw, i.MRPVersion })
                .Select(i => new { i.Key.MaterialId, i.Key.MaterialNameEn, i.Key.MaterialNameTw, i.Key.CategoryCodeId, i.Key.SemiGoods, i.Key.OrdersId, i.Key.LocaleId, i.Key.ParentMaterialId, i.Key.SizeDivision, i.Key.UnitCodeId, i.Key.UnitNameTw, i.Key.MRPVersion, Usage = i.Sum(g => g.Total) })
                .Where(i => orderIds.Contains(i.OrdersId))
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Select(i => new
                {
                    LocaleId = i.LocaleId,
                    // PartId = i.PartId,
                    ParentMaterialId = i.ParentMaterialId,
                    OrdersId = i.OrdersId,
                    UnitCodeId = i.UnitCodeId,
                    MaterialId = i.MaterialId,
                    MaterialNameTw = i.MaterialNameTw,
                    MaterialNameEn = i.MaterialNameEn,
                    Total = i.Usage,
                    Type = 2,
                    CategoryCodeId = i.CategoryCodeId,
                    SizeDivision = i.SizeDivision,
                    SemiGoods = i.SemiGoods,
                    POItemId = POItem.Get().Where(p => p.Status != 2 && p.MaterialId == i.MaterialId && p.LocaleId == i.LocaleId).OrderByDescending(p => p.PODate).ThenByDescending(p => p.Id).FirstOrDefault().Id,
                    MRPVersion = i.MRPVersion,
                }).ToList();

            var items = mrpItem.Union(mrpItemOrders);
            var bomItems = items.GroupBy(i => new { i.LocaleId, i.OrdersId, i.MaterialId, i.MaterialNameTw, i.MaterialNameEn, i.ParentMaterialId, i.UnitCodeId, i.CategoryCodeId, i.SizeDivision, i.SemiGoods, i.POItemId, i.MRPVersion })
                                .Select(i => new
                                {
                                    LocaleId = i.Key.LocaleId,
                                    OrdersId = i.Key.OrdersId,
                                    MaterialId = i.Key.MaterialId,
                                    MaterialNameTw = i.Key.MaterialNameTw,
                                    MaterialNameEn = i.Key.MaterialNameEn,
                                    ParentMaterialId = i.Key.ParentMaterialId,
                                    UnitCodeId = i.Key.UnitCodeId,
                                    CategoryCodeId = i.Key.CategoryCodeId,
                                    SizeDivision = i.Key.SizeDivision,
                                    SemiGoods = i.Key.SemiGoods,
                                    POItemId = i.Key.POItemId,
                                    MRPVersion = i.Key.MRPVersion,
                                    TotalUsage = i.Sum(g => g.Total),
                                }).ToList();

            var mIds = bomItems.Select(i => i.MaterialId).Distinct();
            //這個批次裡所有買過的計畫、整張訂單有買過的採購單，所以採購單的數量可能會大過計畫，排除掉計畫、採購單作廢的
            // Step3: 透過MRPItem找到該訂單所有計畫、採購單
            var purBatchItems = PurBatchItem.Get().Where(i => i.Status != 2 && i.LocaleId == localeId && orderIds.Contains(i.OrdersId) && i.BatchId == batchId && mIds.Contains(i.MaterialId)).ToList();
            var poItems = POItem.Get().Where(i => i.Status != 2 && i.LocaleId == localeId && orderIds.Contains(i.OrdersId) && mIds.Contains(i.MaterialId)).ToList();
            // 取該材料最後買的採購單
            var poItemIds = bomItems.Select(i => i.POItemId).Distinct().ToList();
            var lastPOItems = POItem.Get()
                .Where(i => i.LocaleId == localeId && poItemIds.Contains(i.Id))
                .Select(i => new
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    MaterialId = i.MaterialId,
                    ParentMaterialId = i.ParentMaterialId,
                    PODate = i.PODate,
                    VendorId = i.VendorId,
                    Vendor = i.VendorNameTw,
                    UnitCodeId = i.UnitCodeId,
                    UnitPrice = i.UnitPrice,
                    DollarCodeId = i.DollarCodeId,
                })
                .Distinct()
                .ToList();

            // 從管制表採料轉寄劃
            var purPlanItem = (
                    from m in bomItems
                    select new ERP.Models.Views.PurBatchItem
                    {
                        Id = 0,
                        LocaleId = m.LocaleId,
                        BatchId = batchId,
                        OrdersId = m.OrdersId,
                        MaterialId = m.MaterialId,
                        ParentMaterialId = m.ParentMaterialId,
                        PlanUnitCodeId = m.UnitCodeId,
                        PurUnitCodeId = m.UnitCodeId,

                        PlanQty = m.TotalUsage,
                        SemiGoods = m.SemiGoods,
                        RefQuotId = 0,
                        VendorId = 0,
                        Vendor = "",
                        PurUnitPrice = 0,
                        DollarCodeId = 0,
                        PayCodeId = 0,
                        PurLocaleId = m.LocaleId,
                        ReceivingLocaleId = m.LocaleId,
                        PaymentLocaleId = m.LocaleId,
                        POItemId = 0,
                        PayDollarCodeId = 0,
                        RefLocaleId = m.LocaleId,
                        OnHandQty = m.ParentMaterialId > 0 ? 1 : 0,
                        RefItemId = 0,
                        MaterialNameTw = m.MaterialNameTw,
                        MaterialNameEn = m.MaterialNameEn,
                        OrderNo = purOrdersItem.Where(o => o.OrdersId == m.OrdersId).Max(i => i.OrderNo),
                        LCSD = purOrdersItem.Where(o => o.OrdersId == m.OrdersId).Max(i => i.LCSD),
                        Seq = m.ParentMaterialId > 0 ? m.ParentMaterialId.ToString() + m.OrdersId.ToString() + m.MaterialId.ToString() :
                              m.MaterialId.ToString() + m.OrdersId.ToString(),
                        PurQty = 0,
                        POQty = 0,
                        NewPurQty = 0,
                        CategoryCodeId = m.CategoryCodeId,
                        AlternateType = m.SizeDivision,
                        MRPVersion = m.MRPVersion,
                    }
                )
                .OrderBy(i => i.Seq)
                .ToList();
            // update lastPOItem, Vendor, Price, Unit,Dollar
            purPlanItem.ForEach(i =>
            {
                // 把有採購計劃的最後新一筆更新在系統裡
                i.Id = 0;

                var _purBatchItem = purBatchItems.Where(p => p.OrdersId == i.OrdersId && p.LocaleId == i.LocaleId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId && i.Status != 2).OrderByDescending(i => i.LastUpdateTime).FirstOrDefault();
                if (_purBatchItem != null)
                {
                    i.Id = _purBatchItem.Id;
                    i.ReceivingLocaleId = _purBatchItem.ReceivingLocaleId;
                    i.PaymentLocaleId = _purBatchItem.PaymentLocaleId;
                    i.PayCodeId = _purBatchItem.PayCodeId;

                    i.PurUnitPrice = (decimal)_purBatchItem.PurUnitPrice;
                    i.DollarCodeId = _purBatchItem.DollarCodeId;
                    i.VendorId = _purBatchItem.VendorId;
                    i.PayDollarCodeId = _purBatchItem.DollarCodeId;
                }

                // 抓最後一個採購單，沒有就用採購計劃來放入，如果都沒就是空的全新的資料
                var _poItem = lastPOItems.Where(p => p.MaterialId == i.MaterialId && p.LocaleId == i.LocaleId).OrderByDescending(i => i.PODate).ThenByDescending(i => i.Id).FirstOrDefault();
                if (_poItem != null)
                {
                    i.PurUnitPrice = (decimal)_poItem.UnitPrice;
                    i.DollarCodeId = _poItem.DollarCodeId;
                    i.VendorId = _poItem.VendorId;
                    i.PayDollarCodeId = _poItem.DollarCodeId;
                }

                // 更新清單量、已採購量
                i.PurQty = purBatchItems.Where(p => p.OrdersId == i.OrdersId && p.LocaleId == i.LocaleId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId && i.Status != 2).Sum(p => p.PurQty);
                i.POQty = poItems.Where(p => p.OrdersId == i.OrdersId && p.LocaleId == i.LocaleId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId && p.Status != 2).Sum(p => p.Qty);
                i.NewPurQty = i.PurQty >= i.PlanQty ? 0 : i.PlanQty - i.PurQty;
                i.POItemId = poItems.Where(p => p.OrdersId == i.OrdersId && p.LocaleId == i.LocaleId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId && p.POType == 1).OrderByDescending(p => p.PODate).Select(p => p.Id).FirstOrDefault();
            });

            // display item without po
            if (withoutPO)
            {
                purPlanItem = purPlanItem.Where(i => !(i.POItemId > 0)).OrderBy(i => i.Seq).ToList();
            }

            if (withoutZero)
            {
                purPlanItem = purPlanItem.Where(i => i.PlanQty > 0).OrderBy(i => i.Seq).ToList();
            }
            return purPlanItem.AsQueryable();
        }

        public IQueryable<Models.Views.PurBatchItem> GetPurPlanItem(string predicate)
        {
            var purPOItems = (
                from pbi in PurBatchItem.Get().Where(i => i.Status != 2)
                join poi in POItem.Get().Where(i => i.Status != 2) on new { POItemId = pbi.POItemId, LocaleId = pbi.LocaleId } equals new { POItemId = (decimal?)poi.Id, LocaleId = poi.LocaleId } into poiGRP
                from poi in poiGRP.DefaultIfEmpty()
                select new Models.Views.PurBatchItem
                {
                    Id = pbi.Id,
                    BatchId = pbi.BatchId,
                    OrdersId = pbi.OrdersId,
                    RefLocaleId = pbi.RefLocaleId,
                    MaterialId = pbi.MaterialId,
                    ParentMaterialId = pbi.ParentMaterialId,
                    PlanUnitCodeId = pbi.PlanUnitCodeId,
                    LocaleId = pbi.LocaleId,
                    VendorId = pbi.VendorId,
                    PurLocaleId = pbi.PurLocaleId,
                    PaymentLocaleId = pbi.PaymentLocaleId,
                    ReceivingLocaleId = pbi.ReceivingLocaleId,
                    PayCodeId = pbi.PayCodeId,
                    PayDollarCodeId = pbi.PayDollarCodeId,
                    POItemId = poi.Id,
                    DollarCodeId = pbi.DollarCodeId,
                    AlternateType = pbi.AlternateType,

                    PurQty = pbi.PurQty,
                    PlanQty = pbi.PlanQty,
                    POQty = poi.Qty,
                    PurUnitPrice = pbi.PurUnitPrice,

                    MRPVersion = pbi.MRPVersion,
                })
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .ToList()
                .AsQueryable();
            return purPOItems;
        }
    }
}
