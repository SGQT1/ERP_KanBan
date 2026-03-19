using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;

namespace ERP.Services.Business
{
    public class PackPlanService : BusinessService
    {
        private ERP.Services.Business.Entities.PackPlanService PackPlan;
        private ERP.Services.Business.Entities.PackPlanItemService PackPlanItem;
        private ERP.Services.Business.Entities.PackSizeService PackSizeItem;
        private ERP.Services.Business.Entities.PackMarkService PackMark;
        private ERP.Services.Business.Entities.OrdersService Orders;
        private ERP.Services.Entities.OrdersPackService OrdersPack { get; set; }
        public PackPlanService(
            ERP.Services.Business.Entities.PackPlanService packPlanService,
            ERP.Services.Business.Entities.PackPlanItemService packPlanItemService,
            ERP.Services.Business.Entities.PackSizeService packSizeItemService,
            ERP.Services.Business.Entities.PackMarkService packMarkService,
            ERP.Services.Business.Entities.OrdersService ordersService,
              ERP.Services.Entities.OrdersPackService ordersPackService,

            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            PackPlan = packPlanService;
            PackPlanItem = packPlanItemService;
            PackSizeItem = packSizeItemService;
            PackMark = packMarkService;
            Orders = ordersService;
            OrdersPack = ordersPackService;
        }
        public ERP.Models.Views.PackPlanGroup GetPackPlanGroup(string orders, int localeId, string edition)
        {
            //因為有舊資料重複的OrdersId,LocalId,改成用OrdersNo來取資料
            var packPlan = PackPlan.Get().Where(i => i.OrderNo == orders && i.LocaleId == localeId && i.Edition == edition).FirstOrDefault();
            if (packPlan != null)
            {
                return ReCalculatePackSizeItem(new PackPlanGroup
                {
                    // from OrdersPL
                    PackPlan = packPlan,
                    // from OrdersPack, OrdersPLSpec, OrdersPLForUSA
                    PackPlanItem = PackPlanItem.Get(packPlan.OrderNo, (int)packPlan.LocaleId, packPlan.Edition),
                    // from OrdersItem,SizeCountryMapping
                    PackSizeItem = PackSizeItem.Get(packPlan.OrderNo, (int)packPlan.RefLocaleId),
                    // Summary
                    PackPlanSummary = GetPackingSummary(packPlan.OrderNo, localeId)
                });
            }
            else
            {
                return new PackPlanGroup
                {
                    PackSizeItem = PackSizeItem.Get(packPlan.OrderNo, (int)packPlan.RefLocaleId),
                };
            }
        }
        public ERP.Models.Views.PackPlanGroup BuildPackPlanGroup(string ordersNo, int localeId)
        {
            // var order = Orders.Get().Where(i => i.OrderNo == ordersNo && i.LocaleId == localeId).FirstOrDefault();
            var order = Orders.Get(i => i.OrderNo == ordersNo).FirstOrDefault();
            if (order != null)
            {
                var summary = GetPackingSummary(order.OrderNo, localeId);

                return ReCalculatePackSizeItem(new PackPlanGroup
                {
                    // from Orders
                    PackPlan = new PackPlan
                    {
                        LocaleId = localeId,
                        OrderNo = order.OrderNo,
                        PackingTypeId = order.PackingType,
                        PackingTypeDesc = order.PackingTypeDesc,
                        SizeCountryNameTw = order.ArticleSizeCountryCode,
                        MappingSizeCountryNameTw = order.OrderSizeCountryCode,
                        RefLocaleId = order.LocaleId,
                        RefCompanyId = order.CompanyId,
                        RefStyleNo = order.StyleNo,
                        RefStyleId = order.StyleId,
                        RefArticleNo = order.ArticleNo,
                        RefArticleId = order.ArticleId,
                        RefOrdersId = order.Id,
                        RefOrderQty = order.OrderQty,
                    },
                    // empty
                    PackPlanItem = new List<PackPlanItem>(),
                    // from OrdersItem,SizeCountryMapping
                    PackSizeItem = PackSizeItem.Get((int)order.Id, (int)order.LocaleId),
                    // Summary
                    PackPlanSummary = summary == null ? new PackPlanSummary
                    {
                        LocaleId = order.LocaleId,
                        OrdersId = order.Id,
                        OrderNo = order.OrderNo,
                        OrderQty = order.OrderQty,
                        MappingSizeCountryNameTw = order.ArticleSizeCountryCode,
                        SizeCountryNameTw = order.OrderSizeCountryCode,
                        PackingQtyTotal = 0
                    } : summary
                });
            }
            else
            {
                return null;
            }
        }
        public Models.Views.PackPlanGroup SavePackPlanGroup(PackPlanGroup packPlanGroup)
        {
            //Calculate TTLNW,TTLGW,TTLCTNS,TTLMEAS,TTLCBM
            packPlanGroup = CalculatePL(packPlanGroup);

            var packPlan = packPlanGroup.PackPlan;
            var packPlanItems = packPlanGroup.PackPlanItem;
            try
            {
                UnitOfWork.BeginTransaction();
                if (packPlan != null)
                {
                    //PackPlan
                    {
                        var _packPlan = PackPlan.Get().Where(i =>
                            i.LocaleId == packPlan.LocaleId &&
                            i.RefOrdersId == packPlan.RefOrdersId &&
                            i.OrderNo == packPlan.OrderNo &&
                            i.Edition == packPlan.Edition
                        ).FirstOrDefault();
                        
                        if (_packPlan == null)
                        {
                            packPlan = PackPlan.Create(packPlan);
                        }
                        else
                        {
                            packPlan.Id = _packPlan.Id;
                            packPlan.LocaleId = _packPlan.LocaleId;
                            packPlan.OrderNo = _packPlan.OrderNo;
                            packPlan.Edition = _packPlan.Edition;
                            packPlan = PackPlan.Update(packPlan);
                        }
                    }
                    //PackPlanItem
                    {
                        if (packPlan.Id != 0)
                        {
                            PackPlanItem.RemoveRange((int)packPlan.RefOrdersId, (int)packPlan.LocaleId, packPlan.Edition);
                            PackPlanItem.CreateRange(packPlanItems, packPlan.PackingTypeId);
                        }
                    }
                    //Update PackMark
                    PackMark.UpdateByPackPlan(packPlan);

                    //Update/Add Other table cancel by dennis 20191203
                    PackPlan.UpdateOrdersPackingType(packPlan);
                }
                UnitOfWork.Commit();
                return this.GetPackPlanGroup(packPlan.OrderNo, (int)packPlan.LocaleId, packPlan.Edition);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public void RemovePackPlanGroup(PackPlanGroup packPlanGroup)
        {
            var packPlan = packPlanGroup.PackPlan;
            var packPlanItems = packPlanGroup.PackPlanItem;
            try
            {
                UnitOfWork.BeginTransaction();
                if (packPlan != null)
                {
                    // step1: remove PackMark >> OrdersPLPhoto
                    PackMark.RemoveByPackPlan(packPlan);
                    // step2: remove PackPlanItem >> OrdersPack,OrdersPLForUSA,OrdersPLSpec
                    PackPlanItem.RemoveRange((int)packPlan.RefOrdersId, (int)packPlan.LocaleId, packPlan.Edition);
                    // step3: remove PackPlan
                    PackPlan.Remove(packPlan);
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public IEnumerable<ERP.Models.Views.PackPlanItem> GetPackPlanItem(string orders, int localeId, string edition)
        {
            return PackPlanItem.Get(orders, localeId, edition);
        }
        public PackPlanSummary GetPackingSummary(string orderNo, int localeId)
        {
            // var pl = PackPlan.Get().Where(i => i.OrderNo == orderNo && i.LocaleId == localeId).ToList();
            var pl = PackPlan.Get().Where(i => i.OrderNo == orderNo).ToList();
            var summary = pl.GroupBy(i => new { i.RefLocaleId, i.RefOrdersId, i.OrderNo, i.MappingSizeCountryNameTw, i.SizeCountryNameTw, i.RefOrderQty })
                .Select(i => new PackPlanSummary
                {
                    LocaleId = i.Key.RefLocaleId,
                    OrdersId = i.Key.RefOrdersId,
                    OrderNo = i.Key.OrderNo,
                    OrderQty = i.Key.RefOrderQty,
                    MappingSizeCountryNameTw = i.Key.MappingSizeCountryNameTw,
                    SizeCountryNameTw = i.Key.SizeCountryNameTw,
                    PackingQtyTotal = i.Sum(g => g.PackingQty)
                }).FirstOrDefault();
            return summary;
        }
        private Models.Views.PackPlanGroup CalculatePL(Models.Views.PackPlanGroup packPlanGroup)
        {
            var packPlan = packPlanGroup.PackPlan;
            var packPlanItems = packPlanGroup.PackPlanItem.ToList();

            packPlanItems.ForEach(i =>
            {
                i.NWOfCTN = Math.Round(i.NWOfCTN, 2, MidpointRounding.AwayFromZero);
                i.GWOfCTN = Math.Round(i.GWOfCTN, 2, MidpointRounding.AwayFromZero);
                i.MEAS = Math.Round(i.MEAS, 2, MidpointRounding.AwayFromZero);
                i.CBM = Math.Round(i.CBM, 2, MidpointRounding.AwayFromZero);
            });

            var pairs = packPlanItems
                .GroupBy(i => new { i.GroupBy, i.Edition, i.CTNS, i.MinCNo, i.MaxCNo, i.NWOfCTN, i.GWOfCTN, i.MEAS, i.CBM })
                .Select(g => new { PairOfCTN = g.Sum(i => i.PairOfCTN) })
                .Max(i => i.PairOfCTN)
                .ToString();

            var groups = packPlanItems
                .Select(i => new { i.GroupBy, i.Edition, i.CTNS, i.MinCNo, i.MaxCNo, i.NWOfCTN, i.GWOfCTN, i.MEAS, i.CBM })
                .Distinct();

            packPlanGroup.PackPlan.PackingTypeDesc = pairs;
            packPlanGroup.PackPlan.PackingCTNS = groups.Sum(i => i.CTNS);
            packPlanGroup.PackPlan.PackingNW = groups.Sum(i => i.CTNS * i.NWOfCTN);
            packPlanGroup.PackPlan.PackingGW = groups.Sum(i => i.CTNS * i.GWOfCTN);
            packPlanGroup.PackPlan.PackingMEAS = groups.Sum(i => i.CTNS * i.MEAS);
            // packPlanGroup.PackPlan.PackingCBM = groups.Sum(i => i.CTNS * i.CBM);
            packPlanGroup.PackPlan.PackingCBM = groups.Sum(i => i.CBM);

            return packPlanGroup;
        }

        /*
         * Packing的版本有兩種定義
         * type 1: A+B 的裝箱數量 = 訂單數量 >> 一般常見的訂單分不同櫃或不同出貨日出貨 
         * type 2: A+B 的裝箱數量 != 訂單數量 >> 建立不同裝法的PL給客人確認，所以裝箱數會大於訂單數
         * 判斷邏輯
         * step 1:如果訂單還未裝箱，傳回完整的訂單總數
         * step 2:如果訂單已裝箱，裝箱數量 >= 訂單數量 ，判斷為type 2 >> 建立不同裝法的PL給客人確認，所以裝箱數會大於訂單數
         * step 3:如果訂單已裝箱，裝箱數量 < 訂單數量，判斷為 type 1 >> 訂單分不同櫃或不同出貨日出貨
         */
        private Models.Views.PackPlanGroup ReCalculatePackSizeItem(Models.Views.PackPlanGroup packPlanGroup)
        {
            var summary = packPlanGroup.PackPlanSummary;
            var packPlan = packPlanGroup.PackPlan;
            var packSize = packPlanGroup.PackSizeItem.ToList();
            var packItem = packPlanGroup.PackPlanItem.ToList();

            if (summary != null && summary.PackingQtyTotal > 0 && summary.PackingQtyTotal < summary.OrderQty)
            {
                var packItemTotal = OrdersPack.Get()
                    .Where(i => i.RefOrdersId == packPlan.RefOrdersId && i.LocaleId == packPlan.LocaleId)
                    .Select(i => new { ItemInnerSize = i.ItemInnerSize, RefDisplaySize = i.RefDisplaySize, Pairs = i.PairOfCTN * i.CTNS })
                    .ToList();

                var packItemTotal2 = packItem
                    .Select(i => new { ItemInnerSize = i.ItemInnerSize, RefDisplaySize = i.RefDisplaySize, Pairs = i.PairOfCTN * i.CTNS })
                    .ToList();

                packSize.ForEach(i =>
                {
                    var packQty = packItemTotal.Where(p => p.ItemInnerSize == i.ItemInnerSize).Sum(p => p.Pairs);
                    var packQty2 = packItemTotal2.Where(p => p.ItemInnerSize == i.ItemInnerSize).Sum(p => p.Pairs);

                    i.AvailableQty = i.Qty - packQty + packQty2;
                    System.Diagnostics.Debug.WriteLine(i.ItemInnerSize + "," + i.Qty + "," + packQty);
                });

                packPlanGroup.PackSizeItem = packSize;
            }
            return packPlanGroup;
        }
    }
}