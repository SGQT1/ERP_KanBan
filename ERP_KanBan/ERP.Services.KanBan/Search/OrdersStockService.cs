using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Services.KanBan.Bases;

namespace ERP.Services.KanBan.Search
{
    public class OrdersStockService : KanBanService
    {

        private Services.Entities.OrdersService Orders { get; }
        private Services.Entities.CTNLabelService CTNLabel { get; }
        private Services.Entities.CTNLabelItemService CTNLabelItem { get; }
        private Services.Entities.CTNLabelStockInService CTNLabelStockIn { get; }
        private Services.Entities.CTNLabelStockOutService CTNLabelStockOut { get; }
        private Services.Entities.SimpleSaleService Shipment { get; }
        private Services.Entities.CompanyService Company { get; }
        private Services.Entities.CodeItemService CodeItem { get; }

        private Services.Business.Entities.OrdersStockItemService OrdersStockItem { get; set; }

        public OrdersStockService(
            Services.Entities.OrdersService ordersService,
            Services.Entities.CTNLabelService ctnLabelService,
            Services.Entities.CTNLabelItemService ctnLabelItemService,
            Services.Entities.CTNLabelStockInService ctnLabelStockInService,
            Services.Entities.CTNLabelStockOutService ctnLabelStockOutService,
            Services.Entities.SimpleSaleService shipmentService,
            Services.Entities.CompanyService companyService,
            Services.Entities.CodeItemService codeItemService,
            Services.Business.Entities.OrdersStockItemService ordersStockItemService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            Orders = ordersService;
            CTNLabel = ctnLabelService;
            CTNLabelItem = ctnLabelItemService;
            CTNLabelStockIn = ctnLabelStockInService;
            CTNLabelStockOut = ctnLabelStockOutService;
            Shipment = shipmentService;

            Company = companyService;
            CodeItem = codeItemService;
            OrdersStockItem = ordersStockItemService;
        }


        public List<ERP.Models.KanBan.Views.OrdersStockStyleTaking> GetOrderSotckStyle(string predicate)
        {
            var styleItems = new List<ERP.Models.KanBan.Views.OrdersStockStyleTaking>();
            var baseDate = DateTime.Today.AddDays(-365); // 排除舊資料
                        var items = (
                from si in CTNLabelStockIn.Get()
                join cli in CTNLabelItem.Get() on new { LocaleId = si.LocaleId, LabelCode = si.LabelCode } equals new { LocaleId = cli.LocaleId, LabelCode = cli.LabelCode }
                join h in CTNLabel.Get() on new { CTNLabelId = cli.CTNLabelId, LocaleId = cli.LocaleId } equals new { CTNLabelId = h.Id, LocaleId = h.LocaleId }
                join so in CTNLabelStockOut.Get() on new { LocaleId = cli.LocaleId, LabelCode = cli.LabelCode } equals new { LocaleId = so.LocaleId, LabelCode = so.LabelCode } into soGrp
                from so in soGrp.DefaultIfEmpty()
                join o in Orders.Get() on new { OrderNo = h.OrderNo } equals new { OrderNo = o.OrderNo }
                join s in Shipment.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = s.OrdersId, LocaleId = s.RefLocaleId} into sGRP
                from s in sGRP.DefaultIfEmpty()
                select new
                {
                    LocaleId = si.LocaleId,
                    StyleNo = h.StyleNo,
                    OrderNo = h.OrderNo,
                    StockInTime = si.StockInAdjTime,
                    StockOutTime = so.StockOutAdjTime,
                    PackingQty = cli.SubPackingQty,
                    ShipmentDate = s.SaleDate,
                    OrderQty = o.OrderQty,
                }
            )
            .Where(i => i.ShipmentDate == null && i.StockInTime >= baseDate && i.StockOutTime == null)
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .GroupBy(i => new { i.StyleNo, i.LocaleId, i.OrderNo, i.OrderQty })
            .Select(i => new
            {
                LocaleId = i.Key.LocaleId,
                StyleNo = i.Key.StyleNo,
                StockQty = i.Sum(t => t.PackingQty),
                CartonCount = i.Count(),
                OrderCount = i.Select(g => g.OrderNo).Distinct().Count(),
                OrderQty = i.Key.OrderQty,
            })
            .GroupBy(i => new { i.StyleNo, i.LocaleId })
            .Select(i => new
            {
                LocaleId = i.Key.LocaleId,
                StyleNo = i.Key.StyleNo,
                StockQty = i.Sum(t => t.StockQty),
                CartonCount = i.Sum(t => t.CartonCount),
                OrderCount = i.Sum(t => t.OrderCount),
                OrderQty = i.Sum(t => t.OrderQty),
            })
            .OrderBy(i => i.StyleNo)
            .ToList();
            
            if (items.Any())
            {
                // top 5
                styleItems = items.OrderByDescending(i => i.StockQty).Take(5)
                .Select(i => new ERP.Models.KanBan.Views.OrdersStockStyleTaking
                {
                    LocaleId = i.LocaleId,
                    StyleNo = i.StyleNo,
                    StockQty = i.StockQty,
                    TotalStockQty = items.Sum(i => i.StockQty),
                    StockQtyRate = i.StockQty*100 / items.Sum(i => i.StockQty),

                    OrderCount = i.OrderCount,
                    TotalOrderCount = items.Sum(i => i.OrderCount),
                    OrderCountRate = i.OrderCount*100 / items.Sum(i => i.OrderCount),

                    OrderQty = i.OrderQty,
                    TotalOrderQty = items.Sum(i => i.OrderQty),
                    OrderQtyRate = i.OrderQty*100 / items.Sum(i => i.OrderQty),
                })
                .ToList();

                //others
                styleItems.Add(new ERP.Models.KanBan.Views.OrdersStockStyleTaking
                {
                    LocaleId = styleItems[0].LocaleId,
                    StyleNo = "Others",

                    StockQty = styleItems[0].TotalOrderQty - styleItems.Sum(i => i.StockQty),
                    TotalStockQty = styleItems[0].TotalOrderQty,
                    StockQtyRate = 100 - styleItems.Sum(i => i.StockQtyRate),

                    OrderCount = styleItems[0].TotalOrderCount - styleItems.Sum(i => i.OrderCount),
                    TotalOrderCount = styleItems[0].TotalOrderCount,
                    OrderCountRate = 100 - styleItems.Sum(i => i.OrderCountRate),

                    OrderQty = styleItems[0].TotalOrderQty - styleItems.Sum(i => i.OrderQty),
                    TotalOrderQty = styleItems[0].TotalOrderQty,
                    OrderQtyRate = 100 - styleItems.Sum(i => i.OrderQtyRate),

                });
            }
            return styleItems;
        }
        // public List<Models.Views.OrdersStockTaking> GetStockTakingsByOrder(string predicate)
        // {
        //     var baseDate = DateTime.Today.AddDays(-365); // 排除舊資料
        //     var items = (
        //         from si in CTNLabelStockIn.Get()
        //         join cli in CTNLabelItem.Get() on new { LocaleId = si.LocaleId, LabelCode = si.LabelCode } equals new { LocaleId = cli.LocaleId, LabelCode = cli.LabelCode }
        //         join h in CTNLabel.Get() on new { CTNLabelId = cli.CTNLabelId, LocaleId = cli.LocaleId } equals new { CTNLabelId = h.Id, LocaleId = h.LocaleId }
        //         join so in CTNLabelStockOut.Get() on new { LocaleId = cli.LocaleId, LabelCode = cli.LabelCode } equals new { LocaleId = so.LocaleId, LabelCode = so.LabelCode } into soGrp
        //         from so in soGrp.DefaultIfEmpty()
        //         join o in Orders.Get() on new { OrderNo = h.OrderNo } equals new { OrderNo = o.OrderNo }
        //         join s in Shipment.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = s.OrdersId, LocaleId = s.RefLocaleId} into sGRP
        //         from s in sGRP.DefaultIfEmpty()
        //         select new
        //         {
        //             LocaleId = si.LocaleId,
        //             StyleNo = h.StyleNo,
        //             OrderNo = h.OrderNo,
        //             StockInTime = si.StockInAdjTime,
        //             StockOutTime = so.StockOutAdjTime,
        //             PackingQty = cli.SubPackingQty,
        //             CSD = o.CSD,
        //             LCSD = o.LCSD,
        //             Qty = o.OrderQty,
        //             ETD = h.ExFactoryDate,
        //             ShipmentDate = s.SaleDate,
        //         }
        //     )
        //     .Where(i => i.ShipmentDate == null && i.StockInTime >= baseDate)
        //     .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
        //     .GroupBy(i => new { i.StyleNo, i.OrderNo, i.LocaleId, i.CSD, i.LCSD, i.Qty, i.ETD })
        //     .Select(i => new Models.Views.OrdersStockTaking
        //     {
        //         LocaleId = i.Key.LocaleId,
        //         StyleNo = i.Key.StyleNo,
        //         OrderNo = i.Key.OrderNo,
        //         StockQty = i.Sum(t => t.PackingQty),
        //         CartonCount = i.Count(),
        //         CSD = i.Key.CSD,
        //         LCSD = i.Key.LCSD,
        //         OrderQty = i.Key.Qty,
        //         ETD = i.Key.ETD
        //     })
        //     .OrderBy(i => i.OrderNo)
        //     .ToList();
        //     return items;

        // }
        // public List<Models.Views.OrdersStockTaking> GetStockTakingsByStyle(string predicate)
        // {
        //     var baseDate = DateTime.Today.AddDays(-365); // 排除舊資料
        //     var items = (
        //         from si in CTNLabelStockIn.Get()
        //         join cli in CTNLabelItem.Get() on new { LocaleId = si.LocaleId, LabelCode = si.LabelCode } equals new { LocaleId = cli.LocaleId, LabelCode = cli.LabelCode }
        //         join h in CTNLabel.Get() on new { CTNLabelId = cli.CTNLabelId, LocaleId = cli.LocaleId } equals new { CTNLabelId = h.Id, LocaleId = h.LocaleId }
        //         join so in CTNLabelStockOut.Get() on new { LocaleId = cli.LocaleId, LabelCode = cli.LabelCode } equals new { LocaleId = so.LocaleId, LabelCode = so.LabelCode } into soGrp
        //         from so in soGrp.DefaultIfEmpty()
        //         join o in Orders.Get() on new { OrderNo = h.OrderNo } equals new { OrderNo = o.OrderNo }
        //         join s in Shipment.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = s.OrdersId, LocaleId = s.RefLocaleId} into sGRP
        //         from s in sGRP.DefaultIfEmpty()
        //         select new
        //         {
        //             LocaleId = si.LocaleId,
        //             StyleNo = h.StyleNo,
        //             OrderNo = h.OrderNo,
        //             StockInTime = si.StockInAdjTime,
        //             StockOutTime = so.StockOutAdjTime,
        //             PackingQty = cli.SubPackingQty,
        //             ShipmentDate = s.SaleDate,
        //             OrderQty = o.OrderQty,
        //         }
        //     )
        //     .Where(i => i.ShipmentDate == null && i.StockInTime >= baseDate)
        //     .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
        //     .GroupBy(i => new { i.StyleNo, i.LocaleId })
        //     .Select(i => new Models.Views.OrdersStockTaking
        //     {
        //         LocaleId = i.Key.LocaleId,
        //         StyleNo = i.Key.StyleNo,
        //         StockQty = i.Sum(t => t.PackingQty),
        //         CartonCount = i.Count(),
        //         OrderCount = i.Select(g => g.OrderNo).Distinct().Count(),
        //         OrderQty = i.Sum(t => t.OrderQty),
        //     })
        //     .OrderBy(i => i.StyleNo)
        //     .ToList();
        //     return items;
        // }

    }
}