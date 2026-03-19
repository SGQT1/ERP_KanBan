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

namespace ERP.Services.Business
{
    public class BatchOrdersCostService : BusinessService
    {
        private ERP.Services.Business.Entities.OrdersService Orders;
        private ERP.Services.Business.Entities.BatchOrdersCostService BatchOrdersCost;
        private ERP.Services.Business.Entities.BatchOrdersCostItemService BatchOrdersCostItem;

        private ERP.Services.Business.Entities.MaterialStockBatchCostService MaterialStockBatchCost;
        private ERP.Services.Business.Entities.MaterialQuotService MaterialQuot;

        private ERP.Services.Business.Entities.POService PO { get; set; }
        private ERP.Services.Business.Entities.POItemService POItem { get; set; }
        private ERP.Services.Business.Entities.ExchangeRateService ExchangeRate { get; set; }
        private ERP.Services.Business.BOMService BOM { get; set; }

        private ERP.Services.Business.Entities.MPSProcedurePOService MPSProcedurePO;

        public BatchOrdersCostService(
            ERP.Services.Business.Entities.OrdersService ordersService,
            ERP.Services.Business.Entities.BatchOrdersCostService batchOrderCostService,
            ERP.Services.Business.Entities.BatchOrdersCostItemService batchOrdersCostItemService,

            ERP.Services.Business.Entities.MaterialStockBatchCostService materialStockBatchCostService,
            ERP.Services.Business.Entities.MaterialQuotService materialQuotService,
            ERP.Services.Business.Entities.POItemService poItemService,
            ERP.Services.Business.Entities.POService poService,
            ERP.Services.Business.Entities.ExchangeRateService exchangeRateService,
            ERP.Services.Business.BOMService bomService,
            ERP.Services.Business.Entities.MPSProcedurePOService mpsProcedurePOService,

            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Orders = ordersService;
            BatchOrdersCost = batchOrderCostService;
            BatchOrdersCostItem = batchOrdersCostItemService;

            MaterialStockBatchCost = materialStockBatchCostService;
            MaterialQuot = materialQuotService;
            PO = poService;
            POItem = poItemService;

            ExchangeRate = exchangeRateService;
            BOM = bomService;
            MPSProcedurePO = mpsProcedurePOService;
        }
        public ERP.Models.Views.BatchOrdersCostGroup GetBatchOrdersCostGroup(string orderNo, int localeId)
        {
            var orders = Orders.Get().Where(i => i.OrderNo == orderNo).FirstOrDefault();
            var cost = BatchOrdersCost.Get().Where(i => i.OrderNo == orderNo).FirstOrDefault();

            if (orders != null && cost != null)
            {
                var costItems = BatchOrdersCostItem.Get().Where(i => i.OrderNo == orderNo).ToList();
                var exchageDate = cost.ExchangeDate != null ? cost.ExchangeDate : orders.CSD;
                var exchangeRates = this.GetExchangeRatesForCost((DateTime)exchageDate);
                var outsource = MPSProcedurePO.Get().Where(i => i.OrderNo == orderNo);

                costItems.ForEach(i =>
                {
                    var sCost = ((cost.ShipmentUnitPrice - cost.ToolingCost - cost.ToolingFund) * cost.OrderQty * cost.ExchangeRate);
                    i.ActualCostRate = sCost == 0 ? 0 : i.ActualCostAmount / ((cost.ShipmentUnitPrice - cost.ToolingCost - cost.ToolingFund) * cost.OrderQty * cost.ExchangeRate);
                    i.StandardCostRate = sCost == 0 ? 0 : i.StandardCostAmount / ((cost.ShipmentUnitPrice - cost.ToolingCost - cost.ToolingFund) * cost.OrderQty * cost.ExchangeRate);
                });

                return new ERP.Models.Views.BatchOrdersCostGroup
                {
                    BatchOrdersCost = cost,
                    BatchOrdersCostItem = costItems,
                    ExchangeRate = exchangeRates,
                    MPSOutsource = outsource,
                };
            }
            return new ERP.Models.Views.BatchOrdersCostGroup { };
        }
        public ERP.Models.Views.BatchOrdersCostGroup BuildBatchOrdersCostGroup(string orderNo, int localeId)
        {
            /* 成本分析作業，匯入相關資料：管制表項目，金額以採購單為主，沒有的以報價單。加上另用量(如果管制表沒有，而領用有，也匯入)
             *
             */
            var cost = BatchOrdersCost.Build(orderNo, localeId);

            if (cost != null)
            {
                //step1: Get all of date from db, BOMItem, POItems, MaterialStockBatchCost
                // var ioTypes = new List<int?> {1, 6, 7, 9, 12};
                var ioTypes = new List<int?> { 1, 2, 3, 5, 6 };
                var bom = BOM.Get((int)cost.OrdersId, (int)cost.RefLocaleId);   // 管制表頭
                var bomItems = bom.BOMItems != null ? bom.BOMItems.ToList() : new List<Models.Views.View.BOMItem>();    //管制表身
                var materialStock = MaterialStockBatchCost.Get().Where(i => i.OrderNo == orderNo && ioTypes.Contains(i.IOType)).ToList();   // 結算後的材料成本
                var outsource = MPSProcedurePO.Get().Where(i => i.OrderNo == orderNo).ToList(); // 拖外

                var poItems = (         // 採購單
                    from pi in POItem.Get()
                    join p in PO.Get() on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = p.Id, LocaleId = p.LocaleId }
                    select new
                    {
                        Status = pi.Status,
                        OrderNo = pi.OrderNo,
                        Material = pi.Material,
                        Unit = pi.Unit,
                        Currency = pi.Currency,
                        UnitPrice = pi.UnitPrice,
                        Vendor = p.Vendor,
                        PurQty = pi.PurQty,
                    })
                    .Where(i => i.OrderNo == cost.OrderNo && i.Status != 2).ToList();

                var exchangeRates = this.GetExchangeRatesForCost((DateTime)cost.CSD);   // 匯率

                //step2: base on bom-item, then fill the price and usage
                var costItems = bomItems.GroupBy(i => new { i.MaterialNameTw, i.UnitNameTw, i.MaterialId, i.LocaleId, i.SemiGoods })
                    .Select(i => new ERP.Models.Views.BatchOrdersCostItem
                    {
                        OrderNo = cost.OrderNo,
                        SemiGoods = (decimal)i.Key.SemiGoods,
                        PartNo = i.Where(g => g.MaterialId == i.Key.MaterialId).FirstOrDefault().PartNo,
                        MaterialNameTw = i.Key.MaterialNameTw,
                        UnitNameTw = i.Key.UnitNameTw,
                        StandardUsage = i.Sum(g => g.Total),
                        ActualUsage = materialStock.Where(s => s.MaterialName == i.Key.MaterialNameTw && s.PCLUnitNameTw == i.Key.UnitNameTw).Any() ?
                                    -materialStock.Where(s => s.MaterialName == i.Key.MaterialNameTw && s.PCLUnitNameTw == i.Key.UnitNameTw).Sum(s => s.IOQty) : 0,
                        TransRate = 1,
                        ExchangeRate = 1,
                        CostDollarNameTw = "USD",
                        CostUnitPrice = 0,
                        StandardCostAmount = 0,
                        ActualCostAmount = 0,
                        IsNew = 0,
                        UsageType = 1,

                        PurUnitNameTw = poItems.Where(p => p.Material == i.Key.MaterialNameTw).Any() ?
                                        poItems.Where(p => p.Material == i.Key.MaterialNameTw).OrderByDescending(p => p.PurQty).First().Unit : "",
                        PurDollarNameTw = poItems.Where(p => p.Material == i.Key.MaterialNameTw).Any() ?
                                        poItems.Where(p => p.Material == i.Key.MaterialNameTw).OrderByDescending(p => p.PurQty).First().Currency : "",
                        PurUnitPrice = poItems.Where(p => p.Material == i.Key.MaterialNameTw).Any() ?
                                        (decimal)poItems.Where(p => p.Material == i.Key.MaterialNameTw).OrderByDescending(p => p.PurQty).First().UnitPrice : 0,
                        PriceType = poItems.Where(p => p.Material == i.Key.MaterialNameTw).Any() ? 1 : 0,
                        Vendor = poItems.Where(p => p.Material == i.Key.MaterialNameTw).Any() ?
                                 poItems.Where(p => p.Material == i.Key.MaterialNameTw).OrderByDescending(p => p.PurQty).First().Vendor : "",
                    })
                    .OrderBy(i => i.PartNo).ThenBy(i => i.MaterialNameTw);

                //step3: have usage not in bom-item
                var bomMaterial = costItems.Select(i => i.MaterialNameTw).Distinct();
                var extraCostItem = materialStock.Where(i => !bomMaterial.Contains(i.MaterialName))
                    .GroupBy(i => new { i.MaterialName, i.PCLUnitNameTw, i.LocaleId })
                    .Select(i => new ERP.Models.Views.BatchOrdersCostItem
                    {
                        OrderNo = cost.OrderNo,
                        SemiGoods = 0,
                        PartNo = "",
                        MaterialNameTw = i.Key.MaterialName,
                        UnitNameTw = i.Key.PCLUnitNameTw,
                        StandardUsage = 0,
                        ActualUsage = -(i.Sum(g => g.IOQty)),
                        PurUnitNameTw = poItems.Where(p => p.Material == i.Key.MaterialName).Any() ?
                                       poItems.Where(p => p.Material == i.Key.MaterialName).Max(p => p.Unit) : "",
                        PurDollarNameTw = poItems.Where(p => p.Material == i.Key.MaterialName).Any() ?
                                       poItems.Where(p => p.Material == i.Key.MaterialName).Max(p => p.Currency) : "",
                        PurUnitPrice = poItems.Where(p => p.Material == i.Key.MaterialName).Any() ?
                                       (decimal)poItems.Where(p => p.Material == i.Key.MaterialName).Max(p => p.UnitPrice) : 0,
                        TransRate = 1,
                        ExchangeRate = 1,
                        CostDollarNameTw = "USD",
                        CostUnitPrice = 0,
                        StandardCostAmount = 0,
                        ActualCostAmount = 0,
                        IsNew = 0,
                        UsageType = 2,
                        PriceType = poItems.Where(p => p.Material == i.Key.MaterialName).Any() ? 1 : 0,
                        Vendor = poItems.Where(p => p.Material == i.Key.MaterialName).Any() ?
                                 poItems.Where(p => p.Material == i.Key.MaterialName).Max(p => p.Vendor) : "",
                    });

                var allItems = costItems.Union(extraCostItem).OrderBy(i => i.MaterialNameTw).ToList();

                // allItems = allItems.Where(i => i.MaterialNameTw == "LJ-T36G-DDY-VEPM5 白色 36*44\" *隆昌").ToList();
                //step4: import ，取道所有材料後，一次性抓回報價單
                var noPriceItems = allItems.Where(i => i.PurUnitPrice == 0).Select(i => i.MaterialNameTw).ToArray();
                var quotations = MaterialQuot.Get().Where(i =>
                        i.LocaleId == cost.RefLocaleId &&
                        i.EffectiveDate < DateTime.Now &&
                        i.Enable == 1 &&
                        noPriceItems.Contains(i.MaterialName)
                    ).ToList();

                allItems.ForEach(i =>
                {
                    if (i.PurUnitPrice == 0)
                    {
                        var quotation = quotations.Where(q => q.MaterialName == i.MaterialNameTw).OrderByDescending(q => q.EffectiveDate).FirstOrDefault();
                        if (quotation != null)
                        {
                            i.PurUnitPrice = quotation.UnitPrice;
                            i.PurUnitNameTw = quotation.UnitCode;
                            i.PurDollarNameTw = quotation.DollarCode;
                            i.PriceType = 2;
                            i.Vendor = quotation.VendorShortNameTw;
                        }
                    }

                    if (i.PurDollarNameTw != null && i.PurDollarNameTw.Length > 0)
                    {
                        i.ExchangeRate = i.PurDollarNameTw != "USD" && exchangeRates.Where(e => e.CurrencyTw == "USD" && e.TransCurrencyTw == i.PurDollarNameTw).Any() ?
                                        exchangeRates.Where(e => e.CurrencyTw == "USD" && e.TransCurrencyTw == i.PurDollarNameTw).Max(e => e.ReversedBankingRate) : 1;
                    }
                    else
                    {
                        i.PurDollarNameTw = "";
                    }

                });

                // step5: upate order's exchange-rate, 
                cost.MaterialStandardCost = allItems.Sum(i => i.StandardCostAmount);
                cost.MaterialActualCost = allItems.Sum(i => i.ActualCostAmount);
                cost.AvgMaterialStandardCost = cost.MaterialStandardCost != 0 ? cost.MaterialStandardCost / cost.OrderQty : 0;
                cost.AvgMaterialActualCost = cost.MaterialActualCost != 0 ? cost.MaterialActualCost / cost.OrderQty : 0;

                cost.ExchangeRate = exchangeRates.Where(i => i.TransCurrencyTw == cost.ShipmentCurrency && i.CurrencyTw == cost.CostCurrency).Any() ?
                                         exchangeRates.Where(i => i.TransCurrencyTw == cost.ShipmentCurrency && i.CurrencyTw == cost.CostCurrency).Min(i => i.ReversedBankingRate) :
                                         (decimal)1.0000;


                return new ERP.Models.Views.BatchOrdersCostGroup
                {
                    BatchOrdersCost = cost,
                    BatchOrdersCostItem = allItems,
                    ExchangeRate = exchangeRates,
                    MPSOutsource = outsource,
                };
            }

            return new ERP.Models.Views.BatchOrdersCostGroup { };
        }
        public IEnumerable<ERP.Models.Views.ExchangeRate> GetExchangeRatesForCost(DateTime exchangeDate)
        {
            return ExchangeRate.Get().Where(i => i.ExchDate == exchangeDate).ToList();
        }
        public Models.Views.BatchOrdersCostGroup SaveBatchOrdersCostGroup(BatchOrdersCostGroup costGroup)
        {
            var cost = costGroup.BatchOrdersCost;
            var costItems = costGroup.BatchOrdersCostItem;

            try
            {
                UnitOfWork.BeginTransaction();
                if (cost != null)
                {
                    //OrderCost
                    {
                        var _cost = BatchOrdersCost.Get().Where(i => i.OrderNo == cost.OrderNo).FirstOrDefault();
                        if (_cost == null)
                        {
                            cost = BatchOrdersCost.Create(cost);
                        }
                        else
                        {
                            cost.Id = _cost.Id;
                            cost.LocaleId = _cost.LocaleId;
                            cost.OrderNo = _cost.OrderNo;
                            cost.BatchMaterialOrdersId = _cost.BatchMaterialOrdersId;
                            cost.BatchMaterialOrdersLocaleId = _cost.BatchMaterialOrdersLocaleId;
                            cost = BatchOrdersCost.Update(cost);
                        }
                    }
                    //OrderCostItem
                    {
                        if (cost.Id != 0)
                        {
                            // save BatchMaterialCost
                            BatchOrdersCostItem.RemoveRange(i => i.OrderNo == cost.OrderNo && i.LocaleId == cost.BatchMaterialOrdersLocaleId);
                            BatchOrdersCostItem.CreateRange(costItems);
                        }
                    }
                }
                UnitOfWork.Commit();
                return GetBatchOrdersCostGroup(cost.OrderNo, (int)cost.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void RemoveBatchOrderCostsGroup(BatchOrdersCostGroup packPlanGroup)
        {
            var cost = packPlanGroup.BatchOrdersCost;
            var costItems = packPlanGroup.BatchOrdersCostItem;

            try
            {
                UnitOfWork.BeginTransaction();
                if (cost != null)
                {
                    BatchOrdersCost.Remove(cost);
                    BatchOrdersCostItem.RemoveRange(i => i.OrderNo == cost.OrderNo && i.LocaleId == cost.BatchMaterialOrdersLocaleId);
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }

        }
        public IEnumerable<ERP.Models.Views.BatchOrdersCost> CloseBatchOrdersCost(IEnumerable<ERP.Models.Views.BatchOrdersCost> items)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                if (items != null || items.Count() > 0)
                {
                    var localeId = items.Select(i => i.LocaleId).FirstOrDefault();
                    var confirmItems = items.Where(i => i.Status == 1).Select(i => i.OrderNo).Distinct();
                    var unConfirmIds = items.Where(i => i.Status == 0).Select(i => i.OrderNo).Distinct();

                    BatchOrdersCost.UpdateConfirm((int)localeId, confirmItems, unConfirmIds);
                }
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
            }
            return items;
        }
    }
}
