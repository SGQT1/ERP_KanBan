using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class BatchOrdersCostService : BusinessService
    {
        private Services.Entities.OrdersService Orders { get; }
        private Services.Entities.OrdersItemService OrdersItem { get; }
        private Services.Business.Entities.ShipmentService Shipment { get; }

        private Services.Entities.BatchMaterialOrdersService BatchMaterialOrders { get; }
        private Services.Entities.BatchMaterialCostService BatchMaterialCost { get; }
        private Services.Entities.BatchProductionCostService BatchProductionCost { get; }



        public BatchOrdersCostService(
            Services.Entities.OrdersService ordersService,
            Services.Entities.OrdersItemService ordersItemService,
            Services.Business.Entities.ShipmentService shipmentService,

            Services.Entities.BatchMaterialOrdersService batchMaterialOrdersService,
            Services.Entities.BatchProductionCostService batchProductionCostService,
            Services.Entities.BatchMaterialCostService batchMaterialCostService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.Orders = ordersService;
            this.OrdersItem = ordersItemService;
            this.Shipment = shipmentService;

            this.BatchMaterialOrders = batchMaterialOrdersService;
            this.BatchProductionCost = batchProductionCostService;
            this.BatchMaterialCost = batchMaterialCostService;

        }
        public IQueryable<ERP.Models.Views.BatchOrdersCost> Get()
        {
            var orders = (
                from bpc in BatchProductionCost.Get()
                join o in Orders.Get().Where(i => i.OrderQty > 0) on new { OrdersNo = bpc.OrderNo, LocaleId = bpc.RefLocaleId } equals new { OrdersNo = o.OrderNo, LocaleId = o.LocaleId }
                join bmo in BatchMaterialOrders.Get() on new { OrdersNo = bpc.OrderNo, LocaleId = bpc.LocaleId } equals new { OrdersNo = bmo.OrderNo, LocaleId = bmo.LocaleId }
                select new ERP.Models.Views.BatchOrdersCost
                {
                    Id = bpc.Id,
                    LocaleId = bpc.LocaleId,
                    OrdersId = o.Id,
                    OrderNo = bpc.OrderNo,
                    RefLocaleId = bpc.RefLocaleId,
                    CostDate = bpc.CostDate,
                    OrderQty = bpc.OrderQty,
                    ShipmentDate = bpc.CloseDate,
                    ShipmentQty = bpc.ShippingQty,
                    ShipmentUnitPrice = bpc.UnitPrice,
                    ShipmentCurrency = bpc.DollarNameTw,
                    ExchangeRate = bpc.ExchangeRate,
                    CostCurrency = bpc.CostDollarNameTw,
                    ToolingFund = bpc.ToolingFund,
                    ToolingCost = bpc.ToolingCost,
                    MaterialStandardCost = bpc.MaterialStandardCost,
                    MaterialActualCost = bpc.MaterialActualCost,
                    AvgMaterialStandardCost = Math.Round((decimal)(bpc.MaterialStandardCost / bpc.OrderQty), 4, MidpointRounding.AwayFromZero),
                    AvgMaterialActualCost = Math.Round((decimal)(bpc.MaterialActualCost / bpc.OrderQty), 4, MidpointRounding.AwayFromZero),

                    Status = bpc.Status,
                    PMCostRate = bpc.PMCostRate,
                    SMCostRate = bpc.SMCostRate,
                    ModifyUserName = bpc.ModifyUserName,
                    LastUpdateTime = bpc.LastUpdateTime,
                    DisplaySize = bpc.DisplaySize,

                    ArticleNo = o.ArticleNo,
                    StyleNo = bpc.StyleNo,
                    ShoeName = bpc.ShoeName,
                    OutsoleNo = bpc.OutsoleNo,
                    CSD = bpc.CSD,
                    CompanyId = o.CompanyId,
                    Company = o.CompanyNo,
                    Brand = o.Brand,
                    Customer = o.Customer,
                    ETD = o.ETD,
                    ProductType = o.ProductType,
                    OrderType = o.OrderType,
                    ExchangeDate = bmo.ExchangeDate,
                    BatchMaterialOrdersId = bmo.Id,
                    BatchMaterialOrdersLocaleId = bmo.LocaleId,
                    IsMRP = bmo.IsMRP,
                    IsCost = bmo.IsCost,

                    CostFee2 = BatchMaterialCost.Get().Where(i => i.OrderNo == o.OrderNo && i.UsageType == 2).Sum(i => i.ActualCostAmount) / o.OrderQty,
                    CostFee3 = BatchMaterialCost.Get().Where(i => i.OrderNo == o.OrderNo && i.UsageType == 3).Sum(i => i.ActualCostAmount) / o.OrderQty,
                    RMBExchange = BatchMaterialCost.Get().Where(i => i.OrderNo == o.OrderNo && i.PurDollarNameTw == "RMB").Max(i => i.ExchangeRate),
                }
            );
            return orders;
        }
        public IQueryable<ERP.Models.Views.BatchOrdersCost> Get(string predicate)
        {
            var orders = (
                from bpc in BatchProductionCost.Get()
                join o in Orders.Get() on new { OrdersNo = bpc.OrderNo, LocaleId = bpc.RefLocaleId } equals new { OrdersNo = o.OrderNo, LocaleId = o.LocaleId }
                join bmo in BatchMaterialOrders.Get() on new { OrdersNo = bpc.OrderNo, LocaleId = bpc.LocaleId } equals new { OrdersNo = bmo.OrderNo, LocaleId = bmo.LocaleId }
                select new ERP.Models.Views.BatchOrdersCost
                {
                    Id = bpc.Id,
                    LocaleId = bpc.LocaleId,
                    OrdersId = o.Id,
                    OrderNo = bpc.OrderNo,
                    RefLocaleId = bpc.RefLocaleId,
                    CostDate = bpc.CostDate,
                    OrderQty = bpc.OrderQty,
                    ShipmentDate = bpc.CloseDate,
                    ShipmentQty = bpc.ShippingQty,
                    ShipmentUnitPrice = bpc.UnitPrice,
                    ShipmentCurrency = bpc.DollarNameTw,
                    ExchangeRate = bpc.ExchangeRate,
                    CostCurrency = bpc.CostDollarNameTw,
                    ToolingFund = bpc.ToolingFund,
                    ToolingCost = bpc.ToolingCost,
                    MaterialStandardCost = bpc.MaterialStandardCost,
                    MaterialActualCost = bpc.MaterialActualCost,
                    AvgMaterialStandardCost = Math.Round((decimal)(bpc.MaterialStandardCost / bpc.OrderQty), 4, MidpointRounding.AwayFromZero),
                    AvgMaterialActualCost = Math.Round((decimal)(bpc.MaterialActualCost / bpc.OrderQty), 4, MidpointRounding.AwayFromZero),

                    Status = bpc.Status,
                    PMCostRate = bpc.PMCostRate,
                    SMCostRate = bpc.SMCostRate,
                    ModifyUserName = bpc.ModifyUserName,
                    LastUpdateTime = bpc.LastUpdateTime,
                    DisplaySize = bpc.DisplaySize,

                    StyleNo = bpc.StyleNo,
                    ShoeName = bpc.ShoeName,
                    OutsoleNo = bpc.OutsoleNo,
                    CSD = bpc.CSD,
                    CompanyId = o.CompanyId,
                    Company = o.CompanyNo,
                    Brand = o.Brand,
                    Customer = o.Customer,
                    ETD = o.ETD,
                    ProductType = o.ProductType,
                    OrderType = o.OrderType,
                    ExchangeDate = bmo.ExchangeDate,
                    BatchMaterialOrdersId = bmo.Id,
                    BatchMaterialOrdersLocaleId = bmo.LocaleId,
                    IsMRP = bmo.IsMRP,
                    IsCost = bmo.IsCost,

                    CostFee2 = BatchMaterialCost.Get().Where(i => i.OrderNo == o.OrderNo && i.IsNew == 1).Sum(i => i.ActualCostAmount),
                    CostFee3 = BatchMaterialCost.Get().Where(i => i.OrderNo == o.OrderNo && i.UsageType == 3).Sum(i => i.ActualCostAmount),
                    RMBExchange = BatchMaterialCost.Get().Where(i => i.OrderNo == o.OrderNo && i.PurDollarNameTw == "RMB").Max(i => i.ExchangeRate),
                })
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .ToList();
            orders.ForEach(i =>
            {
                i.NetPrice = i.ShipmentUnitPrice - i.ToolingFund - i.ToolingCost;
                i.CostFee2 = i.CostFee2 == 0 ? 0 : i.CostFee2 / i.OrderQty;
                i.CostFee3 = i.CostFee3 == 0 ? 0 : i.CostFee3 / i.OrderQty;
                i.RMBExchange = i.RMBExchange == null ? i.RMBExchange : Math.Round((decimal)(1 / i.RMBExchange), 4);
            });
            return orders.AsQueryable();
        }
        public ERP.Models.Views.BatchOrdersCost Build(string orderNo, int localeId)
        {
            var o = Orders.Get().Where(i => i.OrderNo == orderNo).FirstOrDefault();
            var s = Shipment.Get().Where(i => i.OrderNo == orderNo).OrderByDescending(i => i.SaleQty).FirstOrDefault();

            if (o != null)
            {
                var oi = OrdersItem.Get().Where(i => i.OrdersId == o.Id && i.LocaleId == o.LocaleId).ToList();
                var orders = new ERP.Models.Views.BatchOrdersCost
                {
                    Id = 0,
                    LocaleId = localeId,
                    OrdersId = o.Id,
                    OrderNo = o.OrderNo,
                    RefLocaleId = o.LocaleId,
                    CostDate = DateTime.Now,
                    OrderQty = o.OrderQty,

                    ExchangeRate = (decimal)1.0000,
                    CostCurrency = "USD",
                    ToolingFund = (decimal)0.0000,
                    ToolingCost = (decimal)0.0000,
                    MaterialStandardCost = (decimal)0.0000,
                    MaterialActualCost = (decimal)0.0000,
                    AvgMaterialStandardCost = (decimal)0.0000,
                    AvgMaterialActualCost = (decimal)0.0000,

                    Status = 0,
                    PMCostRate = 0,

                    ExchangeDate = o.CSD,
                    BatchMaterialOrdersId = 0,
                    BatchMaterialOrdersLocaleId = localeId,
                    StyleNo = o.StyleNo,
                    ShoeName = o.ShoeName,
                    OutsoleNo = o.OutsoleNo,
                    CSD = o.CSD,
                    CompanyId = o.CompanyId,
                    Company = o.CompanyNo,
                    Brand = o.Brand,
                    Customer = o.Customer,
                    ETD = o.ETD,
                    ProductType = o.ProductType,
                    OrderType = o.OrderType,
                    IsMRP = 1,
                    IsCost = 1,
                    DisplaySize = oi.Count() == 0 ? "" :
                                  oi.OrderBy(i => i.ArticleInnerSize).Select(i => i.DisplaySize).FirstOrDefault() + " - " +
                                  oi.OrderByDescending(i => i.ArticleInnerSize).Select(i => i.DisplaySize).FirstOrDefault()
                };
                if (s != null)
                {
                    orders.ShipmentDate = s.SaleDate;
                    orders.ShipmentQty = s.SaleQty;
                    orders.ShipmentUnitPrice = (decimal)s.Price;
                    orders.ShipmentCurrency = s.RefCurrency;
                    orders.ToolingCost = s.ToolingTotalPrice;
                    orders.ToolingFund = s.ToolFundIntel;
                }

                return orders;
            }
            return null;
        }
        public ERP.Models.Views.BatchOrdersCost Create(ERP.Models.Views.BatchOrdersCost item)
        {
            BatchMaterialOrders.Create(BatchMaterialOrdersBuild(item));
            BatchProductionCost.Create(BatchProductionCostBuild(item));

            return Get().Where(i => i.OrderNo == item.OrderNo && i.LocaleId == item.LocaleId).FirstOrDefault();

        }
        public ERP.Models.Views.BatchOrdersCost Update(ERP.Models.Views.BatchOrdersCost item)
        {
            BatchMaterialOrders.Update(BatchMaterialOrdersBuild(item));
            BatchProductionCost.Update(BatchProductionCostBuild(item));

            return Get().Where(i => i.OrderNo == item.OrderNo && i.LocaleId == item.LocaleId).FirstOrDefault();

        }
        public void Remove(ERP.Models.Views.BatchOrdersCost item)
        {
            BatchMaterialOrders.RemoveRange(i => i.OrderNo == item.OrderNo && i.LocaleId == item.LocaleId);
            BatchProductionCost.RemoveRange(i => i.OrderNo == item.OrderNo && i.LocaleId == item.LocaleId);
        }
        public void UpdateConfirm(int localeId, IEnumerable<string> confirmItems, IEnumerable<string> unConfirmItems)
        {
            // update Sale Id = 0 ,remove shipping Id, umcloseed Only
            BatchProductionCost.UpdateRange(
                i => confirmItems.Contains(i.OrderNo) && i.LocaleId == localeId && i.Status == 0,
                // u => new Models.Entities.BatchProductionCost { Status = 1, LastUpdateTime = DateTime.Now }
                u => u.SetProperty(p => p.Status, v => 1).SetProperty(p => p.LastUpdateTime, v => DateTime.Now)
            );

            // updat shipping id = paymentId where Id in ShipmentId, closed only
            BatchProductionCost.UpdateRange(
                i => unConfirmItems.Contains(i.OrderNo) && i.LocaleId == localeId && i.Status == 1,
                // u => new Models.Entities.BatchProductionCost { Status = 0, LastUpdateTime = DateTime.Now  }
                u => u.SetProperty(p => p.Status, v => 0).SetProperty(p => p.LastUpdateTime, v => DateTime.Now)
            );
        }
        public ERP.Models.Entities.BatchMaterialOrders BatchMaterialOrdersBuild(ERP.Models.Views.BatchOrdersCost item)
        {
            return new Models.Entities.BatchMaterialOrders
            {
                Id = item.BatchMaterialOrdersId,
                LocaleId = item.BatchMaterialOrdersLocaleId,
                OrderNo = item.OrderNo,
                RefLocaleId = item.RefLocaleId,
                OrdersId = item.OrdersId,
                IsMRP = item.IsMRP,
                IsCost = item.IsCost,
                ExchangeDate = item.ExchangeDate,
            };
        }
        public ERP.Models.Entities.BatchProductionCost BatchProductionCostBuild(ERP.Models.Views.BatchOrdersCost item)
        {
            return new Models.Entities.BatchProductionCost()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                RefLocaleId = item.RefLocaleId,
                OrderNo = item.OrderNo,
                CostDate = item.CostDate,
                ShippingQty = item.ShipmentQty,
                DollarNameTw = item.ShipmentCurrency,
                UnitPrice = item.ShipmentUnitPrice,
                ToolingFund = item.ToolingFund == null ? 0 : (decimal)item.ToolingFund,
                ToolingCost = item.ToolingCost,
                CostDollarNameTw = item.CostCurrency,
                ExchangeRate = item.ExchangeRate,
                MaterialStandardCost = item.MaterialStandardCost,
                MaterialActualCost = item.MaterialActualCost,

                Status = item.Status,
                CloseDate = item.ShipmentDate,
                StyleNo = item.StyleNo,
                ShoeName = item.ShoeName,
                OutsoleNo = item.OutsoleNo,
                CSD = item.CSD,
                OrderQty = item.OrderQty,

                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                PMCostRate = item.PMCostRate,
                SMCostRate = item.SMCostRate,
                DisplaySize = item.DisplaySize,

                // SemiProcessedExpenses = item.SemiProcessedExpenses,
                // PersonalLaborCost = item.PersonalLaborCost,
                // GroupLaborCost = item.GroupLaborCost,
                // SemiProcessedLaborCost = item.SemiProcessedLaborCost,
                // DirectManufacturingExpenses = item.DirectManufacturingExpenses,
                // IndirectManufacturingExpenses = item.IndirectManufacturingExpenses,
                // FixedSAExpenses = item.FixedSAExpenses,
                // VariableSAExpenses = item.VariableSAExpenses,
                // TargetOutput = item.TargetOutput,
                // PersonalLaborCost1 = item.PersonalLaborCost1,
                // GroupLaborCost1 = item.GroupLaborCost1,
                // SemiProcessedLaborCost1 = item.SemiProcessedLaborCost1,
                // DirectManufacturingExpenses1 = item.DirectManufacturingExpenses1,
                // IndirectManufacturingExpenses1 = item.IndirectManufacturingExpenses1,
                // FixedSAExpenses1 = item.FixedSAExpenses1,
                // VariableSAExpenses1 = item.VariableSAExpenses1,
            };
        }
    }
}