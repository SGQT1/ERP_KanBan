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
    public class PurPOService : BusinessService
    {
        private ERP.Services.Business.Entities.PurBatchService PurBatch { get; set; }
        private ERP.Services.Business.Entities.PurOrdersItemService PurOrdersItem { get; set; }
        private ERP.Services.Business.Entities.PurBatchItemService PurBatchItem { get; set; }
        private ERP.Services.Business.Entities.MRPItemService MRPItem { get; set; }
        private ERP.Services.Business.Entities.MRPItemOrdersService MRPItemOrders { get; set; }
        private ERP.Services.Business.Entities.POItemService POItem { get; set; }
        private ERP.Services.Business.Entities.POService PO { get; set; }
        private ERP.Services.Business.Entities.POItemSizeService POItemSize { get; set; }
        private ERP.Services.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Business.Entities.OrdersItemSizeRunService OrdersItemSizeRun { get; set; }
        private ERP.Services.Entities.MaterialService Material { get; set; }
        private ERP.Services.Entities.ReceivedLogService ReceivedLog { get; set; }
        private ERP.Services.Entities.VendorService Vendor { get; set; }
        private ERP.Services.Entities.CompanyService Company { get; set; }
        private ERP.Services.Business.Entities.MaterialQuotService Quotation { get; set; }
        private ERP.Services.Entities.MRPItemUsageService MRPItemUsage { get; set; }

        private ERP.Services.Entities.MaterialSamplingMethodService MaterialSamplingMethod { get; set; }
        private ERP.Services.Business.Entities.CodeItemService CodeItem { get; set; }

        public PurPOService
        (
            ERP.Services.Business.Entities.PurBatchService purBatchService,
            ERP.Services.Business.Entities.PurOrdersItemService purOrdersItemService,
            ERP.Services.Business.Entities.PurBatchItemService purBatchItemService,
            ERP.Services.Business.Entities.MRPItemService mrpItemService,
            ERP.Services.Business.Entities.MRPItemOrdersService mrpItemOrdersService,
            ERP.Services.Business.Entities.POItemService poItemService,
            ERP.Services.Business.Entities.POService poService,
            ERP.Services.Business.Entities.POItemSizeService poItemSizeService,
            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Business.Entities.OrdersItemSizeRunService ordersItemSizeRunService,
            ERP.Services.Entities.MaterialService materialService,
            ERP.Services.Entities.ReceivedLogService receivedLogService,
            ERP.Services.Entities.VendorService vendorService,
            ERP.Services.Entities.CompanyService companyService,
            ERP.Services.Business.Entities.MaterialQuotService materialQuotService,
            ERP.Services.Entities.MRPItemUsageService mrpItemUsageService,
            ERP.Services.Entities.MaterialSamplingMethodService materialSamplingMethodService,
            ERP.Services.Business.Entities.CodeItemService codeItemService,
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
            Material = materialService;
            ReceivedLog = receivedLogService;
            Vendor = vendorService;
            Company = companyService;
            Quotation = materialQuotService;
            MRPItemUsage = mrpItemUsageService;
            MaterialSamplingMethod = materialSamplingMethodService;
            CodeItem = codeItemService;
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
        public ERP.Models.Views.PurPOGroup Get(int id, int localeId, int type)
        {
            var group = new ERP.Models.Views.PurPOGroup { };
            var purBatch = PurBatch.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            var purOrdersItem = PurOrdersItem.Get().Where(i => i.PurBatchId == id && i.LocaleId == localeId).OrderBy(i => i.OrderNo).ToList();
            // var purPOItem = GetPurPOItem((int)purBatch.Id, (int)purBatch.LocaleId, type);

            if (purBatch != null && purOrdersItem.Any())
            {
                group.PurBatch = purBatch;
                group.PurOrdersItem = purOrdersItem;
                // group.PurPOItem = purPOItem;
            }
            return group;
        }
        public ERP.Models.Views.PurPOGroup Save(PurPOGroup item)
        {
            var purBatch = item.PurBatch;
            var purOrdersItems = item.PurOrdersItem.ToList();
            var localeId = item.PurBatch.LocaleId;

            if (purBatch != null)
            {
                try
                {
                    // 轉採購清單的動作如下
                    // 已經有採購單的(沒收貨前可以更新數量等資料)，所以把Item裡有POItemId的取出 >> updateItems,
                    // 沒有採購單的就可以新增，把沒有POItemId的分開取出 >> addItems,
                    // 把更新跟新增的POItem 整合在一起。
                    // 刪除所有的 POItemSizeRun後，再新增有SizeRun的訂單
                    // 更新PO的IsShowSizeRun
                    // 更新回去PurBatchItem，也另外取出要更新的數據 >> purBatchItme
                    UnitOfWork.BeginTransaction();

                    var companies = Company.Get().ToList();
                    var remark = "※交貨之品質不符或數量不足交貨期延誤，而影響本公司生產時，售方應負擔一切損失。\n※交貨時請於簽單及材料包裝上註明採購單號及管制編號，請款時務必列明並附上採購單。\n※送貨時間請於上班時間內(週一到週五，上午8點~12點與下午1點~5點。\n※禁含致癌AZO偶氮染料.鉛等八大重金屬致癌物質。\n※提前交貨應事先徵得我方之書面同意，否則將拒絕收貨。";

                    var purPOItems = item.PurPOItem.Select(i => new Models.Views.POItem
                    {
                        Id = i.POItemId == null ? 0 : (decimal)i.POItemId,
                        LocaleId = i.LocaleId,
                        POId = i.POId == null ? 0 : (decimal)i.POId,
                        OrdersId = i.OrdersId,
                        OrderNo = i.OrderNo,
                        StyleNo = i.StyleNo,
                        POType = i.POType,
                        MaterialId = i.MaterialId,
                        UnitPrice = i.QuotUnitPrice, //i.PurUnitPrice, // 更改每次儲存都更新最新的單價。i.UnitPrice, i.PurUnitPrice = i.QuotUnitPrice
                        DollarCodeId = i.PayDollarCodeId, // 更改每次儲存都更新最新的單價幣別 i.DollarCodeId,
                        UnitCodeId = i.PlanUnitCodeId,   // 改每次儲存都更新最的單價幣別i.,
                        Qty = (decimal)i.POQty,
                        PayCodeId = (int)i.PayCodeId,
                        PurLocaleId = i.PurLocaleId ?? 0,
                        ReceivingLocaleId = i.ReceivingLocaleId ?? 0,
                        PaymentLocaleId = i.PaymentLocaleId ?? 0,
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
                        AlternateType = i.AlternateType,
                        IsShowSizeRun = i.IsShowSizeRun,

                        VendorId = i.VendorId ?? 0,
                        IsAllowPartial = 0,
                        PurBatchItemId = i.Id,
                        PONo = i.PONo,
                        POTeam = i.POTeam,
                        MRPVersion = i.MRPVersion,
                    }).ToList();

                    var purBatchItems = item.PurPOItem.Select(i => new Models.Views.PurBatchItem
                    {
                        Id = i.Id,
                        LocaleId = i.LocaleId,
                        BatchId = i.BatchId,
                        OrdersId = i.OrdersId,
                        MaterialId = i.MaterialId,
                        PlanUnitCodeId = i.PlanUnitCodeId,
                        PlanQty = i.PlanQty ?? 0,
                        RefQuotId = i.RefQuotId,
                        VendorId = i.VendorId ?? 0,
                        PurUnitPrice = (decimal)i.QuotUnitPrice, //i.PurUnitPrice,
                        DollarCodeId = i.DollarCodeId ?? 0,
                        PayCodeId = i.PayCodeId ?? 0,
                        PurUnitCodeId = i.PlanUnitCodeId,
                        PurQty = i.PurQty,
                        PurLocaleId = i.PurLocaleId ?? 0,
                        ReceivingLocaleId = i.ReceivingLocaleId ?? 0,
                        PaymentLocaleId = i.PaymentLocaleId ?? 0,
                        POItemId = i.POItemId,
                        OnHandQty = i.OnHandQty,
                        ParentMaterialId = i.ParentMaterialId,
                        ModifyUserName = i.ModifyUserName,
                        LastUpdateTime = i.LastUpdateTime,
                        PayDollarCodeId = i.PayDollarCodeId,
                        RefLocaleId = i.RefLocaleId,
                        RefItemId = i.RefItemId,
                        AlternateType = i.AlternateType,
                        Status = i.Status,
                        MRPVersion = i.MRPVersion,
                    })
                    .ToList();

                    // have poId can update only, not have POId add
                    var addItems = purPOItems.Where(i => i.Id == 0 || i.Id == null).ToList();
                    var updateItems = purPOItems.Where(i => i.Id > 0).ToList();
                    // 1==================================================================================================================================
                    // 已經有採購單的(沒收貨前可以更新數量等資料)，所以把Item裡有POItemId的取出 >> updateItems start,
                    // update exsit items
                    // step1, update POItem
                    // step2, group by PO Id and update by poid
                    if (updateItems.Count() > 0)
                    {
                        POItem.UpdateRange(updateItems);
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
                        var items = PO.GetWithOutVendor().Where(i => i.BatchNo == purBatch.BatchNo && i.LocaleId == purBatch.LocaleId).Select(i => i.SeqId).ToList();
                        var seqId = items.Count() == 0 ? 0 : items.Max();

                        // step 2: 生成所有的PO, 從item中把相同材料跟廠商的取出當表頭，序號為自動編號，從批號中最大的加1，
                        var poGroups = addItems.Select(p => new { p.MaterialId, p.VendorId, p.LocaleId, p.ModifyUserName, p.POTeam }).Distinct().ToList();
                        poGroups.ForEach(p =>
                        {
                            seqId += 1;

                            var po = new Models.Views.PO
                            {
                                LocaleId = p.LocaleId,
                                PODate = DateTime.Now.Date,
                                BatchNo = purBatch.BatchNo,
                                SeqId = seqId,
                                VendorId = p.VendorId,
                                Remark = remark,
                                ModifyUserName = p.ModifyUserName,
                                POTeam = p.POTeam,
                                IsAllowPartial = 1,
                                IsShowSizeRun = addItems.Where(i => i.IsShowSizeRun == 1).Any() ? 1 : 0  // 是否為有Size訂單，批次採購因為PO下有很多POItem，只要有一個POItem By Size, 整個PO都是
                            };
                            pos.Add(po);
                            addItems.Where(m => m.MaterialId == p.MaterialId && m.VendorId == p.VendorId).ToList().ForEach(p =>
                            {
                                p.SeqId = seqId;
                                p.PONo = purBatch.BatchNo + "-" + seqId.ToString();
                            });
                        });

                        // step 3:新增PO
                        PO.CreateRange(pos);

                        // step 4: 新增後PO後，把POId更新到表身，POItem裡
                        var poNos = PO.GetWithOutVendor().Where(p => p.BatchNo == purBatch.BatchNo && p.LocaleId == purBatch.LocaleId).Select(p => new { p.Id, p.LocaleId, PONo = p.BatchNo + "-" + p.SeqId.ToString() }).ToList();
                        addItems.ForEach(i =>
                        {
                            var po = poNos.Where(p => p.PONo == i.PONo).FirstOrDefault();
                            if (po != null)
                            {
                                i.POId = po.Id;
                            }
                        });

                        // var testAdd = addItems.Select(i => new { i.MaterialId, i.VendorId}).Distinct();
                        // step 5: add POItem
                        POItem.CreateRange(addItems);
                    }
                    // 沒有採購單的就可以新增，把沒有POItemId的分開取出 >> addItems end,

                    // 3==================================================================================================================================
                    // 把更新跟新增的POItem 整合在一起 start
                    var addPONos = addItems.Select(i => i.PONo).Distinct().ToList();
                    var newAddItems = POItem.Get().Where(p => addPONos.Contains(p.PONo) && p.LocaleId == localeId).ToList();
                    var allPOItems = updateItems.Union(newAddItems).ToList();
                    var poItemShowSizes = allPOItems.Where(i => i.IsShowSizeRun == 1).ToList();
                    var poItemNoSizes = allPOItems.Where(i => i.IsShowSizeRun == 0).ToList();
                    // 把更新跟新增的POItem 整合在一起 end。
                    // 4==================================================================================================================================
                    // 刪除所有的 POItemSizeRun後，再新增有SizeRun的訂單，同時更新PO的IsShowSizeRun Start
                    // 把所有要新增的POItemSize的訂單SizeRun都找出來
                    var orderNos = purPOItems.Where(i => i.IsShowSizeRun == 1).Select(i => i.OrderNo).Distinct();
                    var ordersItems = OrdersItemSizeRun.Get().Where(i => orderNos.Contains(i.OrderNo)).ToList();

                    // 新增，size run參考管制表的用量
                    var ordersIds = purPOItems.Where(i => i.IsShowSizeRun == 1).Select(i => i.OrdersId).Distinct().ToList();
                    var materialIds = purPOItems.Where(i => i.IsShowSizeRun == 1).Select(i => i.MaterialId).Distinct().ToList();
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
                                Usage = i.F > 0 ? 1 : 0,
                                Qty = i.G,
                            });
                        });
                    });

                    // 5==================================================================================================================================
                    // 刪除所有POItemId 的size run
                    var poItemIds = purPOItems.Select(i => i.Id).Distinct();
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

                            Qty = p.Qty,
                            SeqNo = p.ArticleInnerSize,
                            ModifyUserName = p.ModifyUserName,
                            LastUpdateTime = p.LastUpdateTime,
                            PreQty = p.Qty,

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
                    // 更新回去PurBatchItem，也另外取出要更新的數據 >> purBatchItme start，批次的話，是先有PurBatchItemId, 所以直接從POItem資料上取得
                    if (purBatchItems.Count() > 0)
                    {
                        // var poItems = POItem.Get().Where(p => p.PONo.Contains(purBatch.BatchNo + "-") && p.LocaleId == purBatch.LocaleId).Select(p => new { p.Id, p.LocaleId, p.PurBatchItemId }).Distinct().ToList();
                        purBatchItems.ForEach(p =>
                        {
                            var _poitem = allPOItems.Where(pi => pi.PurBatchItemId == p.Id && pi.LocaleId == p.LocaleId).FirstOrDefault();
                            if (_poitem != null)
                            {
                                p.POItemId = _poitem.Id;
                                p.PurQty = _poitem.Qty;
                                p.Status = _poitem.Status;
                                p.PlanUnitCodeId = p.PlanUnitCodeId == 0 ? _poitem.UnitCodeId : p.PlanUnitCodeId;
                                p.PayDollarCodeId = p.PayDollarCodeId;
                                p.PurUnitPrice = (decimal)p.PurUnitPrice;
                            }
                        });
                        PurBatchItem.UpdateRange(purBatchItems);
                    }// 更新回去PurBatchItem，也另外取出要更新的數據 >> purBatchItme end

                    UnitOfWork.Commit();
                }
                catch (Exception e)
                {
                    UnitOfWork.Rollback();
                    throw e;
                }
            }
            return Get((int)purBatch.Id, (int)purBatch.LocaleId, 0);
        }
        public void Remove(PurPOGroup item)
        {
            var purBatch = item.PurBatch;
            var purOrdersItems = item.PurOrdersItem.ToList();
            var putPOItems = item.PurPOItem.ToList();

            if (purBatch != null)
            {
                try
                {
                    // delete from PurBatchItem  where OrdersId =22832 and MaterialId =15254 and ParentMaterialId =0 and (POItemId is null or LEN(POItemId)<=0) and (PurLocaleId >7 or PurLocaleId <7) and LocaleId =7
                    UnitOfWork.BeginTransaction();
                    // var items = purBatchItems.Select(i => new { i.BatchId, i.LocaleId, i.OrdersId, i.MaterialId, i.ParentMaterialId }).Distinct().ToList();
                    // items.ForEach(i =>
                    // {
                    //     PurBatchItem.RemoveRange(p => p.BatchId == purBatch.Id && p.LocaleId == purBatch.LocaleId && p.OrdersId == i.OrdersId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId && (p.POItemId == 0 || p.POItemId == null));
                    // });
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
        public IQueryable<Models.Views.PurPOItem> BuildPurPOItem(string predicate, string[] filters)
        {
            var status1 = new List<int> { 0, 1 };
            var localeId = 0;
            var type = 1;

            // extend condition, obj by ExtentionItem
            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                localeId = (int)extenFilters.Field1;
                type = (int)extenFilters.Field2;
            }

            try
            {
                var purPOItems = (
                    from pbi in PurBatchItem.Get().Where(i => i.Status != 2)
                    join m in Material.Get() on new { MaterialId = pbi.MaterialId, LocaleId = pbi.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGrp
                    from m in mGrp.DefaultIfEmpty()
                    join o in Orders.Get() on new { OrdersId = pbi.OrdersId, LocaleId = pbi.LocaleId } equals new { OrdersId = o.Id, LocaleId = o.LocaleId } into oGRP
                    from o in oGRP.DefaultIfEmpty()
                    join v in Vendor.Get() on new { VendorId = pbi.VendorId, LocaleId = pbi.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                    from v in vGRP.DefaultIfEmpty()
                    join poi in POItem.Get().Where(i => i.Status != 2) on new { POItemId = pbi.POItemId, LocaleId = pbi.LocaleId } equals new { POItemId = (decimal?)poi.Id, LocaleId = poi.LocaleId } into poiGRP
                    from poi in poiGRP.DefaultIfEmpty()
                    join v1 in Vendor.Get() on new { VendorId = poi.VendorId, LocaleId = poi.LocaleId } equals new { VendorId = v1.Id, LocaleId = v1.LocaleId } into v1GRP
                    from v1 in v1GRP.DefaultIfEmpty()
                    select new Models.Views.PurPOItem
                    {
                        OrdersId = pbi.OrdersId,
                        MaterialId = pbi.MaterialId,
                        ParentMaterialId = pbi.ParentMaterialId,
                        MRPVersion = pbi.MRPVersion,
                        PlanUnitCodeId = pbi.PlanUnitCodeId,
                        OnHandQty = pbi.OnHandQty,
                        PurQty = pbi.PurQty,
                        PurVendorId = pbi.VendorId,
                        PurAlternateType = pbi.AlternateType,
                        PurPurLocaleId = pbi.PurLocaleId,
                        PurReceivingLocaleId = pbi.ReceivingLocaleId,
                        PurPaymentLocaleId = pbi.PaymentLocaleId,
                        PurPayCodeId = pbi.PayCodeId,
                        PurPlanQty = pbi.PlanQty,
                        Id = pbi.Id,
                        BatchId = pbi.BatchId,
                        RefLocaleId = pbi.RefLocaleId,
                        LocaleId = pbi.LocaleId,
                        ModifyUserName = pbi.ModifyUserName,
                        PurUnitCodeId = pbi.PurUnitCodeId,
                        PurUnitPrice = pbi.PurUnitPrice,
                        PayDollarCodeId = pbi.DollarCodeId,
                        RefQuotId = 0,

                        PurSamplingMethod = MaterialSamplingMethod.Get().Where(i => i.CategoryNameTw == CodeItem.Get().Where(c => c.Id == m.CategoryCodeId && c.LocaleId == m.LocaleId).Max(c => c.NameTW)).Max(i => i.SamplingMethod),
                        CategoryCodeId = (decimal)m.CategoryCodeId,
                        MaterialNameTw = m.MaterialName,
                        MaterialNameEn = m.MaterialNameEng,

                        OrderNo = o.OrderNo,
                        PurRemark = o.ShoeName + " " + o.Customer + " " + o.CustomerOrderNo,
                        StyleNo = o.StyleNo,
                        LCSD = o.LCSD,
                        CSD = o.CSD,

                        Vendor = v1.NameTw,
                        PurPaymentCodeId = v.PaymentCodeId,
                        PurPaymentPoint = v.PaymentPoint,
                        PurVendor = v.NameTw,

                        IsShowSizeRun = poi.IsShowSizeRun,
                        AlternateType = poi.AlternateType,
                        PONo = poi.RefPONo,
                        ReceivedBarcode = poi.ReceivedBarcode,
                        PurLocaleId = poi.PurLocaleId,
                        ReceivingLocaleId = poi.ReceivingLocaleId,
                        PaymentLocaleId = poi.PaymentLocaleId,
                        PayCodeId = poi.PayCodeId,
                        PlanQty = poi.PlanQty,
                        POQty = poi.Qty,
                        UnitCodeId = poi.UnitCodeId,
                        DollarCodeId = poi.DollarCodeId,
                        UnitPrice = poi.UnitPrice,
                        VendorId = poi.VendorId,
                        IsOverQty = poi.IsOverQty,
                        POId = poi.POId,
                        POItemId = poi.Id,
                        Status = poi.Status,
                        POType = poi.POType,
                        SamplingMethod = poi.SamplingMethod,
                        FactoryETD = poi.FactoryETD,
                        Remark = poi.Remark,
                        CompanyId = (decimal)poi.CompanyId,
                        Seq = poi.SeqId.ToString(),
                        PaymentCodeId = poi.PaymentCodeId,
                        PaymentPoint = poi.PaymentPoint,
                        ReceivedLogId = ReceivedLog.Get().Where(r => r.POItemId == poi.Id && r.RefLocaleId == poi.LocaleId).Max(r => r.Id),
                        QuotUnitPrice = poi.QuotUnitPrice,
                        PODate = poi.PODate,
                    })
                    .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                    .ToList()
                    .AsQueryable();

                // 取得材料報價單
                var vendorIds = purPOItems.Select(i => i.PurVendorId).Distinct().ToList(); // 用S2的廠商
                var mIds = purPOItems.Select(i => i.MaterialId).Distinct().ToList();
                var qutos = Quotation.Get().Where(i => vendorIds.Contains(i.VendorId) && mIds.Contains(i.MaterialId) && i.LocaleId == localeId && i.Enable == 1).ToList();

                var items = type == 1 ? purPOItems.Where(i => i.POItemId == 0 || i.POItemId == null).ToList() :
                            type == 2 ? purPOItems.Where(i => i.POItemId > 0 && status1.Contains((int)i.Status)).ToList() :
                            // type == 3 ? purPOItems.Where(i => i.PurLocaleId != localeId ) :
                            purPOItems.ToList();

                items.ForEach(i =>
                {
                    //報價單用S2的廠商取，因為S3的PO可能還沒生成，S2與S3的廠商應該是同步的
                    var _quto = i.PODate == null ? qutos.Where(q => q.VendorId == i.PurVendorId && q.LocaleId == i.LocaleId && q.MaterialId == i.MaterialId && q.Enable == 1 && q.EffectiveDate <= DateTime.Today).OrderByDescending(q => q.EffectiveDate).FirstOrDefault()
                                                : qutos.Where(q => q.VendorId == i.PurVendorId && q.LocaleId == i.LocaleId && q.MaterialId == i.MaterialId && q.Enable == 1 && q.EffectiveDate <= i.PODate).OrderByDescending(q => q.EffectiveDate).FirstOrDefault();

                    i.QuotUnitPrice = _quto != null ? _quto.UnitPrice : 0;

                    if (i.ReceivedLogId > 0)
                    {
                        i.PurSamplingMethod = i.PurSamplingMethod;
                        i.POType = i.POType;
                        i.IsShowSizeRun = i.IsShowSizeRun;
                        i.AlternateType = i.AlternateType;

                        i.PurLocaleId = i.PurLocaleId;
                        i.ReceivingLocaleId = i.ReceivingLocaleId;
                        i.PaymentLocaleId = i.PaymentLocaleId;
                        i.PayCodeId = i.PayCodeId;
                        i.PlanQty = i.PlanQty;
                        i.OnHandQty = i.OnHandQty;
                        i.POQty = i.POQty;
                        i.PlanUnitCodeId = i.PlanUnitCodeId;
                        i.IsOverQty = i.IsOverQty;
                        i.Remark = i.Remark;
                        i.SamplingMethod = i.SamplingMethod;

                        i.UnitCodeId = i.UnitCodeId;
                        i.DollarCodeId = i.DollarCodeId;
                        i.UnitPrice = i.UnitPrice;
                        i.VendorId = i.VendorId;
                        i.Vendor = i.Vendor;
                        i.Diff = i.PurVendorId == i.VendorId ? 0 : 1;

                    }
                    else
                    {
                        if (_quto != null)
                        {
                            i.PurUnitCodeId = _quto.UnitCodeId;
                            i.PurUnitPrice = _quto.UnitPrice;
                            i.RefQuotId = _quto.Id;
                            i.PayDollarCodeId = _quto.DollarCodeId;
                        }
                        
                        i.DollarCodeId = i.PayDollarCodeId; // 未收貨時，每次都根據S2的結果更新幣別
                        i.PlanUnitCodeId = i.PlanUnitCodeId; // 未收貨時，每次都根據S2的結果更新單位
                        i.PurSamplingMethod = i.PurSamplingMethod ?? 2;
                        i.POType = i.POType ?? 1;

                        i.IsShowSizeRun = i.POItemId == null ? 0 : i.IsShowSizeRun;
                        i.AlternateType = i.POItemId == null ? i.PurAlternateType : i.AlternateType;
                        i.PONo = i.POItemId == null ? "" : i.PONo;
                        i.ReceivedBarcode = i.POItemId == null ? "" : i.ReceivedBarcode;
                        i.PurLocaleId = i.POItemId == null ? i.PurPurLocaleId : i.PurLocaleId;
                        i.ReceivingLocaleId = i.POItemId == null ? i.PurReceivingLocaleId : i.ReceivingLocaleId;
                        i.PaymentLocaleId = i.POItemId == null ? i.PurPaymentLocaleId : i.PaymentLocaleId;
                        i.PayCodeId = i.POItemId == null ? i.PurPayCodeId : i.PayCodeId;
                        i.PlanQty = i.POItemId == null ? i.PurPlanQty : i.PlanQty;
                        i.OnHandQty = i.POItemId == null ? i.OnHandQty : i.OnHandQty;
                        i.POQty = i.POItemId == null ? i.PurQty : i.POQty;
                        i.IsOverQty = i.POItemId == null ? 0 : i.IsOverQty;
                        i.Remark = i.POItemId == null ? i.PurRemark : i.Remark;
                        i.SamplingMethod = i.POItemId == null ? i.PurSamplingMethod : i.SamplingMethod;
                        i.UnitCodeId = i.POItemId == null ? i.PlanUnitCodeId : i.UnitCodeId;
                        i.UnitPrice = i.POItemId == null ? i.PurUnitPrice : i.UnitPrice;    // 還沒有PO就等於用PurBatchItem單價＝最新報價單單價
                        i.VendorId = i.POItemId == null ? i.PurVendorId : i.VendorId;
                        i.Vendor = i.POItemId == null ? i.PurVendor : i.Vendor;
                        i.Diff = i.PurVendorId == i.VendorId ? 0 : 1;

                        // i.PurSamplingMethod = i.PurSamplingMethod == null ? 2 : i.PurSamplingMethod;
                        // i.POType = i.POType == null ? 1 : i.POType;
                        // i.IsShowSizeRun = i.POItemId == null ? 0 : i.IsShowSizeRun;
                        // i.AlternateType = i.POItemId == null ? i.PurAlternateType : i.AlternateType;
                        // i.PONo = i.POItemId == null ? "" : i.PONo;
                        // i.ReceivedBarcode = i.POItemId == null ? "" : i.ReceivedBarcode;

                        // i.PurLocaleId = i.POItemId == null ? i.PurPurLocaleId : i.PurLocaleId;
                        // i.ReceivingLocaleId = i.POItemId == null ? i.PurReceivingLocaleId : i.ReceivingLocaleId;
                        // i.PaymentLocaleId = i.POItemId == null ? i.PurPaymentLocaleId : i.PaymentLocaleId;
                        // i.PayCodeId = i.POItemId == null ? i.PurPayCodeId : i.PayCodeId;
                        // i.PlanQty = i.POItemId == null ? i.PurPlanQty : i.PlanQty;
                        // i.OnHandQty = i.POItemId == null ? i.OnHandQty : i.OnHandQty;
                        // i.POQty = i.POItemId == null ? i.PurQty : i.POQty;
                        // i.PlanUnitCodeId = i.PlanUnitCodeId;
                        // i.IsOverQty = i.POItemId == null ? 0 : i.IsOverQty;
                        // i.Remark = i.POItemId == null ? i.PurRemark : i.Remark;
                        // i.SamplingMethod = i.POItemId == null ? i.PurSamplingMethod : i.SamplingMethod;

                        // i.UnitCodeId = i.POItemId == null ? i.PlanUnitCodeId : i.UnitCodeId;
                        // i.DollarCodeId = i.PayDollarCodeId;
                        // i.UnitPrice = i.POItemId == null ? i.PurUnitPrice : i.UnitPrice;    // 還沒有PO就等於用PurBatchItem單價＝最新報價單單價
                        // i.VendorId = i.POItemId == null ? i.PurVendorId : i.VendorId;
                        // i.Vendor = i.POItemId == null ? i.PurVendor : i.Vendor;
                        // i.Diff = i.PurVendorId == i.VendorId ? 0 : 1;
                    }
                });
                return items.AsQueryable();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IQueryable<Models.Views.PurPOItem> BuildPurPOItem1(string predicate, string[] filters)
        {
            var status1 = new List<int> { 0, 1 };
            var localeId = 0;
            var type = 1;

            // extend condition, obj by ExtentionItem
            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                localeId = (int)extenFilters.Field1;
                type = (int)extenFilters.Field2;
            }

            var purPOItems = (
                from pbi in PurBatchItem.Get().Where(i => i.Status != 2)
                join m in Material.Get() on new { MaterialId = pbi.MaterialId, LocaleId = pbi.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGrp
                from m in mGrp.DefaultIfEmpty()
                join o in Orders.Get() on new { OrdersId = pbi.OrdersId, LocaleId = pbi.LocaleId } equals new { OrdersId = o.Id, LocaleId = o.LocaleId } into oGRP
                from o in oGRP.DefaultIfEmpty()
                join v in Vendor.Get() on new { VendorId = pbi.VendorId, LocaleId = pbi.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                join poi in POItem.Get().Where(i => i.Status != 2) on new { POItemId = pbi.POItemId, LocaleId = pbi.LocaleId } equals new { POItemId = (decimal?)poi.Id, LocaleId = poi.LocaleId } into poiGRP
                from poi in poiGRP.DefaultIfEmpty()
                join v1 in Vendor.Get() on new { VendorId = poi.VendorId, LocaleId = poi.LocaleId } equals new { VendorId = v1.Id, LocaleId = v1.LocaleId } into v1GRP
                from v1 in v1GRP.DefaultIfEmpty()
                select new Models.Views.PurPOItem
                {
                    OrdersId = pbi.OrdersId,
                    MaterialId = pbi.MaterialId,
                    ParentMaterialId = pbi.ParentMaterialId,
                    MRPVersion = pbi.MRPVersion,
                    PlanUnitCodeId = pbi.PlanUnitCodeId,
                    OnHandQty = pbi.OnHandQty,
                    PurQty = pbi.PurQty,
                    PurVendorId = pbi.VendorId,
                    PurAlternateType = pbi.AlternateType,
                    PurPurLocaleId = pbi.PurLocaleId,
                    PurReceivingLocaleId = pbi.ReceivingLocaleId,
                    PurPaymentLocaleId = pbi.PaymentLocaleId,
                    PurPayCodeId = pbi.PayCodeId,
                    PurPlanQty = pbi.PlanQty,
                    Id = pbi.Id,
                    BatchId = pbi.BatchId,
                    RefLocaleId = pbi.RefLocaleId,
                    LocaleId = pbi.LocaleId,
                    ModifyUserName = pbi.ModifyUserName,

                    PurUnitCodeId = pbi.PurUnitCodeId,
                    PurUnitPrice = pbi.PurUnitPrice,
                    RefQuotId = 0,
                    PayDollarCodeId = pbi.DollarCodeId,

                    PurSamplingMethod = MaterialSamplingMethod.Get().Where(i => i.CategoryNameTw == CodeItem.Get().Where(c => c.Id == m.CategoryCodeId && c.LocaleId == m.LocaleId).Max(c => c.NameTW)).Max(i => i.SamplingMethod),
                    CategoryCodeId = (decimal)m.CategoryCodeId,
                    MaterialNameTw = m.MaterialName,
                    MaterialNameEn = m.MaterialNameEng,
                    OrderNo = o.OrderNo,
                    Vendor = v1.NameTw,
                    PurPaymentCodeId = v.PaymentCodeId,
                    PurPaymentPoint = v.PaymentPoint,
                    PurRemark = o.ShoeName + " " + o.Customer + " " + o.CustomerOrderNo,
                    PurVendor = v.NameTw,
                    StyleNo = o.StyleNo,
                    LCSD = o.LCSD,
                    CSD = o.CSD,

                    IsShowSizeRun = poi.IsShowSizeRun,
                    AlternateType = poi.AlternateType,
                    PONo = poi.RefPONo,
                    ReceivedBarcode = poi.ReceivedBarcode,
                    PurLocaleId = poi.PurLocaleId,
                    ReceivingLocaleId = poi.ReceivingLocaleId,
                    PaymentLocaleId = poi.PaymentLocaleId,
                    PayCodeId = poi.PayCodeId,
                    PlanQty = poi.PlanQty,
                    POQty = poi.Qty,
                    UnitCodeId = poi.UnitCodeId,
                    DollarCodeId = poi.DollarCodeId,
                    UnitPrice = poi.UnitPrice,
                    VendorId = poi.VendorId,
                    IsOverQty = poi.IsOverQty,
                    POId = poi.POId,
                    POItemId = poi.Id,
                    Status = poi.Status,
                    POType = poi.POType,
                    SamplingMethod = poi.SamplingMethod,
                    FactoryETD = poi.FactoryETD,
                    Remark = poi.Remark,
                    CompanyId = (decimal)poi.CompanyId,
                    Seq = poi.SeqId.ToString(),
                    PaymentCodeId = poi.PaymentCodeId,
                    PaymentPoint = poi.PaymentPoint,
                    ReceivedLogId = ReceivedLog.Get().Where(r => r.POItemId == poi.Id && r.RefLocaleId == poi.LocaleId).Max(r => r.Id),
                    QuotUnitPrice = poi.QuotUnitPrice,
                    PODate = poi.PODate,
                })
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .ToList()
                .AsQueryable();

            // 取得材料報價單
            var vendorIds = purPOItems.Select(i => i.PurVendorId).Distinct().ToList(); // 用S2的廠商
            var mIds = purPOItems.Select(i => i.MaterialId).Distinct().ToList();
            var qutos = Quotation.Get().Where(i => vendorIds.Contains(i.VendorId) && mIds.Contains(i.MaterialId) && i.LocaleId == localeId && i.Enable == 1).ToList();

            var items = type == 1 ? purPOItems.Where(i => i.POItemId == 0 || i.POItemId == null).ToList() :
                        type == 2 ? purPOItems.Where(i => i.POItemId > 0 && status1.Contains((int)i.Status)).ToList() :
                        // type == 3 ? purPOItems.Where(i => i.PurLocaleId != localeId ) :
                        purPOItems.ToList();

            items.ForEach(i =>
            {
                //報價單用S2的廠商取，因為S3的PO可能還沒生成，S2與S3的廠商應該是同步的
                var _quto = i.PODate == null ? qutos.Where(q => q.VendorId == i.PurVendorId && q.LocaleId == i.LocaleId && q.MaterialId == i.MaterialId && q.Enable == 1 && q.EffectiveDate <= DateTime.Today).OrderByDescending(q => q.EffectiveDate).FirstOrDefault()
                                            : qutos.Where(q => q.VendorId == i.PurVendorId && q.LocaleId == i.LocaleId && q.MaterialId == i.MaterialId && q.Enable == 1 && q.EffectiveDate <= i.PODate).OrderByDescending(q => q.EffectiveDate).FirstOrDefault();

                i.QuotUnitPrice = _quto != null ? _quto.UnitPrice : 0;

                if (i.ReceivedLogId > 0)
                {
                    i.PurSamplingMethod = i.PurSamplingMethod;
                    i.POType = i.POType;
                    i.IsShowSizeRun = i.IsShowSizeRun;
                    i.AlternateType = i.AlternateType;

                    i.PurLocaleId = i.PurLocaleId;
                    i.ReceivingLocaleId = i.ReceivingLocaleId;
                    i.PaymentLocaleId = i.PaymentLocaleId;
                    i.PayCodeId = i.PayCodeId;
                    i.PlanQty = i.PlanQty;
                    i.OnHandQty = i.OnHandQty;
                    i.POQty = i.POQty;
                    i.PlanUnitCodeId = i.PlanUnitCodeId;
                    i.IsOverQty = i.IsOverQty;
                    i.Remark = i.Remark;
                    i.SamplingMethod = i.SamplingMethod;

                    i.UnitCodeId = i.UnitCodeId;
                    i.DollarCodeId = i.DollarCodeId;
                    i.UnitPrice = i.UnitPrice;
                    i.VendorId = i.VendorId;
                    i.Vendor = i.Vendor;
                    i.Diff = i.PurVendorId == i.VendorId ? 0 : 1;

                }
                else
                {
                    if (_quto != null)
                    {
                        i.PurUnitCodeId = _quto.UnitCodeId;
                        i.PurUnitPrice = _quto.UnitPrice;
                        i.RefQuotId = _quto.Id;
                        i.PayDollarCodeId = _quto.DollarCodeId;
                    }

                    i.PurSamplingMethod = i.PurSamplingMethod == null ? 2 : i.PurSamplingMethod;
                    i.POType = i.POType == null ? 1 : i.POType;
                    i.IsShowSizeRun = i.POItemId == null ? 0 : i.IsShowSizeRun;
                    i.AlternateType = i.POItemId == null ? i.PurAlternateType : i.AlternateType;

                    i.PurLocaleId = i.POItemId == null ? i.PurPurLocaleId : i.PurLocaleId;
                    i.ReceivingLocaleId = i.POItemId == null ? i.PurReceivingLocaleId : i.ReceivingLocaleId;
                    i.PaymentLocaleId = i.POItemId == null ? i.PurPaymentLocaleId : i.PaymentLocaleId;
                    i.PayCodeId = i.POItemId == null ? i.PurPayCodeId : i.PayCodeId;
                    i.PlanQty = i.POItemId == null ? i.PurPlanQty : i.PlanQty;
                    i.OnHandQty = i.POItemId == null ? i.OnHandQty : i.OnHandQty;
                    i.POQty = i.POItemId == null ? i.PurQty : i.POQty;
                    i.PlanUnitCodeId = i.PlanUnitCodeId;
                    i.IsOverQty = i.POItemId == null ? 0 : i.IsOverQty;
                    i.Remark = i.POItemId == null ? i.PurRemark : i.Remark;
                    i.SamplingMethod = i.POItemId == null ? i.PurSamplingMethod : i.SamplingMethod;

                    i.UnitCodeId = i.POItemId == null ? i.PlanUnitCodeId : i.UnitCodeId;
                    i.DollarCodeId = i.PayDollarCodeId;
                    i.UnitPrice = i.POItemId == null ? i.PurUnitPrice : i.UnitPrice;    // 還沒有PO就等於用PurBatchItem單價＝最新報價單單價
                    i.VendorId = i.POItemId == null ? i.PurVendorId : i.VendorId;
                    i.Vendor = i.POItemId == null ? i.PurVendor : i.Vendor;
                    i.Diff = i.PurVendorId == i.VendorId ? 0 : 1;
                }
            });
            return items.AsQueryable();
        }
        public void RemovePurBatchPlan(List<Models.Views.PurPOItem> items)
        {
            if (items != null && items.Count() > 0)
            {
                try
                {
                    // delete from PurBatchItem  where OrdersId =22832 and MaterialId =15254 and ParentMaterialId =0 and (POItemId is null or LEN(POItemId)<=0) and (PurLocaleId >7 or PurLocaleId <7) and LocaleId =7
                    UnitOfWork.BeginTransaction();

                    var localeId = items[0].LocaleId;
                    var ids = items.Select(i => i.Id).ToArray();

                    PurBatchItem.RemoveRange(i => i.LocaleId == localeId && ids.Contains(i.Id) && (i.POItemId == null || i.POItemId == 0));
                    // var item = PurBatchItem.Get().Where(i => i.LocaleId == localeId && ids.Contains(i.Id)).ToList();

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
}
