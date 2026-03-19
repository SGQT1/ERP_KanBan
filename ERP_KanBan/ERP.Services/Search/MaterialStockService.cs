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
using NPOI.SS.Formula.Functions;

namespace ERP.Services.Search
{
    public class MaterialStockService : SearchService
    {
        ERP.Services.Business.MaterialStockService MaterialStock1 { get; }
        ERP.Services.Entities.MaterialStockService MaterialStock { get; }
        ERP.Services.Entities.MaterialService Material { get; }
        ERP.Services.Entities.OrdersService Orders { get; }
        ERP.Services.Entities.ShipmentLogService ShipmentLog { get; }
        ERP.Services.Entities.StockIOService StockIO { get; }
        ERP.Services.Entities.POItemService POItem { get; }
        ERP.Services.Entities.POService PO { get; }
        ERP.Services.Entities.PurBatchItemService PurBatchItem { get; }
        ERP.Services.Entities.MaterialQuotService MaterialQuot { get; }
        ERP.Services.Entities.CodeItemService CodeItem { get; }
        ERP.Services.Entities.ExchangeRateService ExchangeRate { get; }
        ERP.Services.Entities.WarehouseService WareHouse { get; }
        ERP.Services.Entities.ReceivedLogService ReceivedLog { get; }
        ERP.Services.Entities.VendorService Vendor { get; }
        ERP.Services.Entities.MRPItemService MRPItem { get; }
        ERP.Services.Entities.MRPItemOrdersService MRPItemOrders { get; }
        ERP.Services.Business.Entities.MaterialStockItemService MaterialStockItem { get; }

        public MaterialStockService(
            ERP.Services.Business.MaterialStockService materialStock1Service,

            ERP.Services.Entities.MaterialStockService materialStockService,
             ERP.Services.Entities.MaterialService materialService,
            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Entities.ShipmentLogService shipmentLogService,
            ERP.Services.Entities.StockIOService stockIOService,
            ERP.Services.Entities.POItemService poItemService,
            ERP.Services.Entities.POService poService,
            ERP.Services.Entities.PurBatchItemService purBatchItemService,
            ERP.Services.Entities.MaterialQuotService materialQuotService,
            ERP.Services.Entities.CodeItemService codeItemService,
            ERP.Services.Entities.ExchangeRateService exchangeRateService,
            ERP.Services.Entities.WarehouseService warehouseService,
            ERP.Services.Entities.ReceivedLogService receivedLogService,
            ERP.Services.Entities.VendorService vendorService,
            ERP.Services.Entities.MRPItemService mrpItemService,
            ERP.Services.Entities.MRPItemOrdersService mRPItemOrdersService,
            ERP.Services.Business.Entities.MaterialStockItemService materialStockItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MaterialStock1 = materialStock1Service;
            MaterialStock = materialStockService;
            Material = materialService;
            Orders = ordersService;
            ShipmentLog = shipmentLogService;
            StockIO = stockIOService;
            POItem = poItemService;
            PO = poService;
            MaterialQuot = materialQuotService;
            CodeItem = codeItemService;
            ExchangeRate = exchangeRateService;
            WareHouse = warehouseService;
            ReceivedLog = receivedLogService;
            Vendor = vendorService;
            MRPItem = mrpItemService;
            MRPItemOrders = mRPItemOrdersService;

            MaterialStockItem = materialStockItemService;
            PurBatchItem = purBatchItemService;
        }

        // 材料出入庫－表頭
        public IQueryable<Models.Views.MaterialStockCost> GetMaterialStock(string predicate)
        {
            var costSourceType = new int[] { 1, 7, 9, 12 };

            var realStockQty = StockIO.Get().GroupBy(g => new { g.MaterialStockId, g.LocaleId }).Select(g => new { g.Key.MaterialStockId, g.Key.LocaleId, RealStockQty = g.Sum(i => i.PCLIOQty), MaxIODate = g.Max(i => i.IODate) });
            var realStockInQty = StockIO.Get().Where(i => i.PCLIOQty > 0).GroupBy(g => new { g.MaterialStockId, g.LocaleId }).Select(g => new { g.Key.MaterialStockId, g.Key.LocaleId, RealStockInQty = g.Sum(i => i.PCLIOQty) });
            var realStockOutQty = StockIO.Get().Where(i => i.PCLIOQty < 0).GroupBy(g => new { g.MaterialStockId, g.LocaleId }).Select(g => new { g.Key.MaterialStockId, g.Key.LocaleId, RealStockOutQty = g.Sum(i => i.PCLIOQty) });
            var realStockOutCostQty = StockIO.Get().Where(i => i.PCLIOQty < 0 && costSourceType.Contains(i.SourceType)).GroupBy(g => new { g.MaterialStockId, g.LocaleId }).Select(g => new { g.Key.MaterialStockId, g.Key.LocaleId, RealStockOutCostQty = g.Sum(i => i.PCLIOQty) });

            var stocks = (
                from m in MaterialStock.Get()
                join o in Orders.Get() on new { OrderNo = m.OrderNo } equals new { OrderNo = o.OrderNo } into oGRP
                from o in oGRP.DefaultIfEmpty()
                join s in ShipmentLog.Get() on new { OrderNo = m.OrderNo } equals new { OrderNo = s.OrderNo } into sGRP
                from s in sGRP.DefaultIfEmpty()
                join rs in realStockQty on new { MaterialStockId = m.Id, LocaleId = m.LocaleId } equals new { MaterialStockId = rs.MaterialStockId, LocaleId = rs.LocaleId } into rsGRP
                from rs in rsGRP.DefaultIfEmpty()
                join rsi in realStockInQty on new { MaterialStockId = m.Id, LocaleId = m.LocaleId } equals new { MaterialStockId = rsi.MaterialStockId, LocaleId = rsi.LocaleId } into rsiGRP
                from rsi in rsiGRP.DefaultIfEmpty()
                join rso in realStockOutQty on new { MaterialStockId = m.Id, LocaleId = m.LocaleId } equals new { MaterialStockId = rso.MaterialStockId, LocaleId = rso.LocaleId } into rsoGRP
                from rso in rsoGRP.DefaultIfEmpty()
                join rsco in realStockOutCostQty on new { MaterialStockId = m.Id, LocaleId = m.LocaleId } equals new { MaterialStockId = rsco.MaterialStockId, LocaleId = rsco.LocaleId } into rscoGRP
                from rsco in rscoGRP.DefaultIfEmpty()
                select new
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    MaterialId = m.MaterialId,
                    MaterialName = m.MaterialName,
                    ParentMaterialId = m.ParentMaterialId,
                    ParentMaterialNameTw = m.ParentMaterialNameTw,
                    ParentMaterialNameEn = m.ParentMaterialNameEn,
                    WarehouseNo = m.WarehouseNo,
                    OrderNo = m.OrderNo,
                    // PurUnitPrice = pi == null ? 0 : pi.UnitPrice,
                    CSD = o.CSD,
                    ShipmentDate = s.SaleDate,
                    PCLQty = m.PCLQty,
                    Amount = m.Amount,
                    TotalUsageCost = m.TotalUsageCost,
                    PCLUnitNameTw = m.PCLUnitNameTw,
                    StockDollarNameTw = m.StockDollarNameTw,
                    PurUnitPrice = m.PurUnitPrice,
                    AvgUnitPrice = m.AvgUnitPrice,
                    // ExchangeRate = m.ExchangeRate,
                    PurDollarCodeId = m.PurDollarCodeId,
                    PurDollarNameEn = m.PurDollarNameEn,
                    PurDollarNameTw = m.PurDollarNameTw,
                    // PODate = p.PODate,
                    // RealStockQty = StockIO.Get().Where(i => i.MaterialStockId == m.Id && i.LocaleId == m.LocaleId).Sum(i => i.PCLIOQty),
                    // MaxIODate = StockIO.Get().Where(i => i.MaterialStockId == m.Id && i.LocaleId == m.LocaleId).Max(i => i.IODate),
                    // RealStockINQty = StockIO.Get().Where(i => i.MaterialStockId == m.Id && i.LocaleId == m.LocaleId && i.PCLIOQty > 0).Sum(i => i.PCLIOQty),
                    // RealStockOutQty = StockIO.Get().Where(i => i.MaterialStockId == m.Id && i.LocaleId == m.LocaleId && i.PCLIOQty < 0).Sum(i => i.PCLIOQty),
                    // RealStockOutCostQty = StockIO.Get().Where(i => i.MaterialStockId == m.Id && i.LocaleId == m.LocaleId && i.PCLIOQty < 0 && costSourceType.Contains(i.SourceType)).Sum(i => i.PCLIOQty),
                    RealStockQty = (decimal?)rs.RealStockQty ?? 0,
                    MaxIODate = (DateTime?)rs.MaxIODate ?? null,
                    RealStockINQty = (decimal?)rsi.RealStockInQty ?? 0,
                    RealStockOutQty = (decimal?)rso.RealStockOutQty ?? 0,
                    RealStockOutCostQty = (decimal?)rsco.RealStockOutCostQty ?? 0,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Distinct()
            .Select(i => new Models.Views.MaterialStockCost
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                MaterialId = i.MaterialId,
                MaterialName = i.MaterialName,
                ParentMaterialId = i.ParentMaterialId,
                ParentMaterialNameTw = i.ParentMaterialNameTw,
                ParentMaterialNameEn = i.ParentMaterialNameEn,
                WarehouseNo = i.WarehouseNo,
                OrderNo = i.OrderNo,
                RealStockQty = i.RealStockQty,
                RealStockINQty = i.RealStockINQty,
                RealStockOutQty = i.RealStockOutQty,
                RealStockOutCostQty = i.RealStockOutCostQty,
                PurUnitPrice = i.PurUnitPrice,
                AvgUnitPrice = i.AvgUnitPrice,
                PurDollarNameTw = i.PurDollarNameTw,
                PurDollarCodeId = i.PurDollarCodeId,
                PurUnitNameTw = i.PurDollarNameTw,
                // ExchangeRate = i.ExchangeRate,
                CSD = i.CSD,
                ShipmentDate = i.ShipmentDate,
                PCLQty = i.PCLQty,
                Amount = i.Amount,
                PCLUnitNameTw = i.PCLUnitNameTw,
                StockDollarNameTw = i.StockDollarNameTw,
                TotalUsageCost = i.TotalUsageCost,
                // PODate = i.PODate,
                MaxIODate = i.MaxIODate
            })
            .ToList();

            var result = stocks.GroupBy(x => new
            {
                x.Id,
                x.LocaleId,
                x.MaterialId,
                x.OrderNo
            }, (key, g) => g.OrderByDescending(e => e.PODate).First()).ToList();

            result.ForEach(i =>
            {

                i.RealStockINQty = i.RealStockINQty ?? (decimal?)0;
                i.RealStockOutQty = i.RealStockOutQty ?? (decimal?)0;
                // i.RealStockQty = i.RealStockINQty + i.RealStockOutQty;
                i.RealStockQty = i.RealStockQty ?? (decimal?)0;
                i.RealStockOutCostQty = i.RealStockOutCostQty ?? (decimal?)0;

                var today = DateTime.Today;
                i.StockDays = i.RealStockQty > 0 ? ((today - i.MaxIODate) ?? TimeSpan.Zero).Days : 0;
            });

            return result.AsQueryable();
        }
        public IQueryable<Models.Views.MaterialStockCost> GetMaterialStock1(string predicate)
        {
            var costSourceType = new int[] { 1, 7, 9, 12 };

            var stocks = (
                from m in MaterialStock.Get()
                join o in Orders.Get() on new { OrderNo = m.OrderNo } equals new { OrderNo = o.OrderNo } into oGRP
                from o in oGRP.DefaultIfEmpty()
                join s in ShipmentLog.Get() on new { OrderNo = m.OrderNo } equals new { OrderNo = s.OrderNo } into sGRP
                from s in sGRP.DefaultIfEmpty()
                select new
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    MaterialId = m.MaterialId,
                    MaterialName = m.MaterialName,
                    ParentMaterialId = m.ParentMaterialId,
                    ParentMaterialNameTw = m.ParentMaterialNameTw,
                    ParentMaterialNameEn = m.ParentMaterialNameEn,
                    WarehouseNo = m.WarehouseNo,
                    OrderNo = m.OrderNo,
                    // PurUnitPrice = pi == null ? 0 : pi.UnitPrice,
                    CSD = o.CSD,
                    ShipmentDate = s.SaleDate,
                    PCLQty = m.PCLQty,
                    Amount = m.Amount,
                    TotalUsageCost = m.TotalUsageCost,
                    PCLUnitNameTw = m.PCLUnitNameTw,
                    StockDollarNameTw = m.StockDollarNameTw,
                    PurUnitPrice = m.PurUnitPrice,
                    AvgUnitPrice = m.AvgUnitPrice,
                    // ExchangeRate = m.ExchangeRate,
                    PurDollarCodeId = m.PurDollarCodeId,
                    PurDollarNameEn = m.PurDollarNameEn,
                    PurDollarNameTw = m.PurDollarNameTw,
                    // PODate = p.PODate,
                    RealStockQty = StockIO.Get().Where(i => i.MaterialStockId == m.Id && i.LocaleId == m.LocaleId).Sum(i => i.PCLIOQty),
                    // RealStockQty = (decimal?)(Math.Truncate((double)(StockIO.Get().Where(i => i.MaterialStockId == m.Id && i.LocaleId == m.LocaleId).Sum(i => i.PCLIOQty) * 100)) / 100),
                    RealStockINQty = StockIO.Get().Where(i => i.MaterialStockId == m.Id && i.LocaleId == m.LocaleId && i.PCLIOQty > 0).Sum(i => i.PCLIOQty),
                    RealStockOutQty = StockIO.Get().Where(i => i.MaterialStockId == m.Id && i.LocaleId == m.LocaleId && i.PCLIOQty < 0).Sum(i => i.PCLIOQty),
                    RealStockOutCostQty = StockIO.Get().Where(i => i.MaterialStockId == m.Id && i.LocaleId == m.LocaleId && i.PCLIOQty < 0 && costSourceType.Contains(i.SourceType)).Sum(i => i.PCLIOQty),
                    MaxIODate = StockIO.Get().Where(i => i.MaterialStockId == m.Id && i.LocaleId == m.LocaleId).Max(i => i.IODate),
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Distinct()
            .Select(i => new Models.Views.MaterialStockCost
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                MaterialId = i.MaterialId,
                MaterialName = i.MaterialName,
                ParentMaterialId = i.ParentMaterialId,
                ParentMaterialNameTw = i.ParentMaterialNameTw,
                ParentMaterialNameEn = i.ParentMaterialNameEn,
                WarehouseNo = i.WarehouseNo,
                OrderNo = i.OrderNo,
                RealStockQty = i.RealStockQty,
                RealStockINQty = i.RealStockINQty,
                RealStockOutQty = i.RealStockOutQty,
                RealStockOutCostQty = i.RealStockOutCostQty,
                PurUnitPrice = i.PurUnitPrice,
                AvgUnitPrice = i.AvgUnitPrice,
                PurDollarNameTw = i.PurDollarNameTw,
                PurDollarCodeId = i.PurDollarCodeId,
                PurUnitNameTw = i.PurDollarNameTw,
                // ExchangeRate = i.ExchangeRate,
                CSD = i.CSD,
                ShipmentDate = i.ShipmentDate,
                PCLQty = i.PCLQty,
                Amount = i.Amount,
                PCLUnitNameTw = i.PCLUnitNameTw,
                StockDollarNameTw = i.StockDollarNameTw,
                TotalUsageCost = i.TotalUsageCost,
                // PODate = i.PODate,
                MaxIODate = i.MaxIODate
            })
            .ToList();

            var result = stocks.GroupBy(x => new
            {
                x.Id,
                x.LocaleId,
                x.MaterialId,
                x.OrderNo
            }, (key, g) => g.OrderByDescending(e => e.PODate).First()).ToList();

            result.ForEach(i =>
            {

                i.RealStockINQty = i.RealStockINQty ?? (decimal?)0;
                i.RealStockOutQty = i.RealStockOutQty ?? (decimal?)0;
                // i.RealStockQty = i.RealStockINQty + i.RealStockOutQty;
                i.RealStockQty = i.RealStockQty ?? (decimal?)0;
                i.RealStockOutCostQty = i.RealStockOutCostQty ?? (decimal?)0;

                var today = DateTime.Today;
                i.StockDays = i.RealStockQty > 0 ? ((today - i.MaxIODate) ?? TimeSpan.Zero).Days : 0;
            });

            return result.AsQueryable();
        }

        // 材料出入庫－表頭+表身
        public Models.Views.MaterialStockGroup GetMaterialStockGroup(int id, int localeId)
        {
            return MaterialStock1.GetMaterialStockGroup(id, localeId);
        }
        // 庫存總表
        public IQueryable<Models.Views.MaterialStockBalance> GetMaterialStockBalance(string predicate, string[] filters)
        {
            var hasQty = false;
            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                hasQty = (bool)extenFilters.Field9;
            }
            var result = (
                from ms in MaterialStock.Get()
                join w in WareHouse.Get() on new { WareHouseId = ms.WarehouseId, LocaleId = ms.LocaleId } equals new { WareHouseId = w.Id, LocaleId = w.LocaleId } into wGRP
                from w in wGRP.DefaultIfEmpty()
                join m in Material.Get() on new { MaterialId = ms.MaterialId, LocaleId = ms.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
                select new
                {
                    WarehouseId = ms.WarehouseId,
                    WarehouseNo = w.WarehouseNo,
                    MaterialId = ms.MaterialId,
                    MaterialName = m.MaterialName,
                    MaterialNameEng = m.MaterialNameEng,
                    PCLUnitCodeId = ms.PCLUnitCodeId,
                    StockDollarCodeId = ms.StockDollarCodeId,
                    LocaleId = m.LocaleId,
                    PCLQty = ms.PCLQty,
                    PCLUnitNameTw = ms.PCLUnitNameTw,
                    StockDollarNameTw = ms.StockDollarNameTw,
                    PurDollarNameTw = ms.PurDollarNameTw,
                    RealPCLQty = StockIO.Get().Where(i => i.MaterialStockId == ms.Id && i.LocaleId == ms.LocaleId).Sum(g => g.PCLIOQty),
                    APCLQty = StockIO.Get().Where(i => i.MaterialStockId == ms.Id && i.LocaleId == ms.LocaleId && i.PCLIOQty > 0).Sum(g => g.PCLIOQty),
                    AAmount = StockIO.Get().Where(i => i.MaterialStockId == ms.Id && i.LocaleId == ms.LocaleId && i.PCLIOQty > 0).Sum(s => s.PCLIOQty * s.PurUnitPrice * s.BankingRate / s.TransRate),
                }
            );

            if (hasQty)
            {
                result = result.Where(i => i.RealPCLQty > 0);
            }

            var items = result
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            if (items.Any())
            {
                // 把平均單價直接轉換最新的台幣匯率
                var exchangeRate = ExchangeRate.Get().Where(i => i.NameTw == "USD" && i.TransNameTw == "NTD").OrderByDescending(i => i.ExchDate).Select(i => i.BankingRate).FirstOrDefault();
                var dollarCodeId = CodeItem.Get().Where(i => i.NameTW == "USD" && i.LocaleId == items[0].LocaleId && i.CodeType == "02").Select(i => i.Id).FirstOrDefault();

                var gItems = items
                .Select(i => new MaterialStockBalance
                {
                    MaterialId = i.MaterialId,
                    MaterialName = i.MaterialName,
                    MaterialNameEng = i.MaterialNameEng,
                    PCLUnitCodeId = i.PCLUnitCodeId,
                    DollarCodeId = dollarCodeId, //i.StockDollarCodeId,
                    LocaleId = i.LocaleId,
                    PCLQty = i.PCLQty,
                    PCLUnitNameTw = i.PCLUnitNameTw,
                    Dollar = "USD", //i.StockDollarNameTw,
                    NewDollar = i.PurDollarNameTw,
                    WarehouseNo = i.WarehouseNo,
                    RealPCLQty = i.RealPCLQty,
                    PurTotalQty = i.APCLQty,
                    PurTotalAmount = i.StockDollarNameTw == "NTD" ? (i.AAmount / exchangeRate) : i.AAmount,
                })
                .GroupBy(g => new { g.MaterialId, g.MaterialName, g.MaterialNameEng, g.PCLUnitCodeId, g.LocaleId, g.Dollar, g.DollarCodeId, g.WarehouseNo })
                .Select(i => new Models.Views.MaterialStockBalance
                {
                    MaterialId = i.Key.MaterialId,
                    MaterialName = i.Key.MaterialName,
                    MaterialNameEng = i.Key.MaterialNameEng,
                    PCLUnitCodeId = i.Key.PCLUnitCodeId,
                    DollarCodeId = i.Key.DollarCodeId,
                    LocaleId = i.Key.LocaleId,
                    PCLQty = i.Sum(g => g.PCLQty),
                    PCLUnitNameTw = i.Max(g => g.PCLUnitNameTw),
                    Dollar = i.Key.Dollar,
                    NewDollar = i.Max(g => g.NewDollar),
                    WarehouseNo = i.Key.WarehouseNo,
                    RealPCLQty = i.Sum(g => g.RealPCLQty),
                    PurTotalQty = i.Sum(g => g.PurTotalQty),
                    PurTotalAmount = i.Sum(g => g.PurTotalAmount),
                })
                .ToList();

                gItems.ForEach(i =>
                {
                    // i.RealPCLQty = Math.Round((decimal)i.RealPCLQty, 5);
                    i.AvgPrice = i.PurTotalQty == 0 || i.PurTotalAmount == 0 ? 0 : (decimal)(i.PurTotalAmount / i.PurTotalQty);
                    i.Amount = (decimal)(i.AvgPrice * i.RealPCLQty);
                    i.ExchangeRate = exchangeRate;
                });
                return gItems.AsQueryable();
            }

            return new List<Models.Views.MaterialStockBalance>().AsQueryable();
        }

        // 出入庫總表
        public IQueryable<Models.Views.MaterialStockItem> GetMaterialStockItem(string predicate, string[] filters)
        {
            List<decimal> sourceType = new List<decimal> { };
            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                sourceType = Array.ConvertAll(extenFilters.Field3, i => (decimal)i).ToList();
            }
            var items = (
                from s in StockIO.Get()
                join ms in MaterialStock.Get().Select(i => new
                {
                    i.Id,
                    i.LocaleId,
                    i.PurUnitPrice,
                    i.PurDollarNameTw,
                    i.PCLQty,
                    i.PCLUnitNameTw,
                    i.StockDollarNameTw,
                    i.MaterialName,
                    i.MaterialNameEng,
                    i.AvgUnitPrice,
                }).Distinct() on new { MaterialStockId = s.MaterialStockId, LocaleId = s.LocaleId } equals new { MaterialStockId = ms.Id, LocaleId = ms.LocaleId } into msGRP
                from ms in msGRP.DefaultIfEmpty()
                join w in WareHouse.Get() on new { WareHouseId = s.WarehouseId, LocaleId = s.LocaleId } equals new { WareHouseId = w.Id, LocaleId = w.LocaleId } into wGRP
                from w in wGRP.DefaultIfEmpty()
                join r in ReceivedLog.Get() on new { ReceivedLogId = s.ReceivedLogId, LocaleId = s.LocaleId } equals new { ReceivedLogId = (decimal?)r.Id, LocaleId = r.LocaleId } into rGRP
                from r in rGRP.DefaultIfEmpty()
                join v in Vendor.Get() on new { VendorId = r.ShippingListVendorId, LocaleId = r.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                select new
                {
                    Id = s.Id,
                    LocaleId = s.LocaleId,
                    IODate = s.IODate,
                    OrderNo = s.OrderNo,
                    MaterialId = s.MaterialId,
                    MaterialName = ms.MaterialName,
                    MaterialNameEng = ms.MaterialNameEng,
                    PrePCLQty = s.PrePCLQty,
                    PCLIOQty = s.PCLIOQty,
                    PCLUnitCodeId = s.PurUnitCodeId,
                    PCLUnitNameTw = ms.PCLUnitNameTw,
                    PurUnitPrice = s.PurUnitPrice,
                    BankingRate = s.BankingRate,
                    RefNo = s.RefNo,
                    SourceType = s.SourceType,
                    OrgUnitId = s.OrgUnitId,
                    VendorName = v.ShortNameTw,
                    VendorId = v.Id,
                    WarehouseId = s.WarehouseId,
                    WarehouseNo = w.WarehouseNo,
                    ModifyUserName = s.ModifyUserName,
                    LastUpdateTime = s.LastUpdateTime,
                    OrgUnitNameTw = s.OrgUnitNameTw,
                    StockDollarCodeId = s.StockDollarCodeId,
                    PurDollarCodeId = s.PurDollarCodeId,
                    MaterialStockId = s.MaterialStockId,
                    ReceivedLogId = s.ReceivedLogId,

                    AvgUnitPrice = ms.AvgUnitPrice,
                }
            );

            if (sourceType.Any())
            {
                items = items.Where(i => sourceType.Contains(i.SourceType));
            }


            var result = items.Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Select(i => new Models.Views.MaterialStockItem
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    IODate = i.IODate,
                    OrderNo = i.OrderNo,
                    MaterialId = i.MaterialId,
                    MaterialName = i.MaterialName,
                    MaterialNameEng = i.MaterialNameEng,
                    PCLIOQty = i.PCLIOQty,
                    PCLUnitCodeId = i.PCLUnitCodeId,
                    PCLUnitNameTw = i.PCLUnitNameTw,
                    RefNo = i.RefNo,
                    SourceType = i.SourceType,
                    OrgUnitId = i.OrgUnitId,
                    OrgUnitNameTw = i.OrgUnitNameTw,
                    VendorId = i.VendorId,
                    WarehouseId = i.WarehouseId,
                    WarehouseNo = i.WarehouseNo,
                    LastUpdateTime = i.LastUpdateTime,

                    ModifyUserName = i.ModifyUserName,

                    PurUnitPrice = i.PurUnitPrice,
                    AvgUnitPrice = i.AvgUnitPrice,
                    VendorName = i.VendorName,

                    Amount = i.PCLIOQty * i.PurUnitPrice * i.BankingRate,
                    NewAmount = i.PCLIOQty * i.AvgUnitPrice,

                    BankingRate = i.BankingRate,
                    StockDollarCodeId = i.StockDollarCodeId,
                    PurDollarCodeId = i.PurDollarCodeId,
                    MaterialStockId = i.MaterialStockId,
                    ReceivedLogId = i.ReceivedLogId,
                })
                .ToList();

            return result.AsQueryable();
        }
        public IQueryable<Models.Views.MaterialCoordinate> GetMaterialCoordinate(string predicate)
        {
            var items = new List<Models.Views.MaterialCoordinate>();

            try
            {
                // 收料彙總（避免在 Select 內對同一組資料反覆 Max/Sum，且完全可轉 SQL）
                var rlAgg =
                    from r in ReceivedLog.Get()
                    group r by new { r.POItemId, r.RefLocaleId } into g
                    select new
                    {
                        g.Key.POItemId,
                        g.Key.RefLocaleId,
                        ReceivedLogId = (decimal?)g.Max(x => x.Id),
                        ReceivedQty = (decimal?)g.Sum(x => (decimal?)x.IQCGetQty),
                        ReceivedDate = (DateTime?)g.Max(x => x.ReceivedDate),
                    };

                var _allPOItems = (
                    from p in POItem.Get().Where(i => i.Status != 2)
                    join po in PO.Get() on new { p.POId, p.LocaleId } equals new { POId = po.Id, po.LocaleId }
                    join pr in PurBatchItem.Get().Where(i => i.Status != 2) on new { OrdersId = (decimal?)p.Id, LocaleId = p.LocaleId } equals new { OrdersId = pr.POItemId, LocaleId = pr.LocaleId } into prGRP
                    from pr in prGRP.DefaultIfEmpty()
                    join o in Orders.Get() on new { OrdersId = p.OrdersId, p.LocaleId } equals new { OrdersId = o.Id, o.LocaleId } into oGRP
                    from o in oGRP.DefaultIfEmpty()
                    join m in Material.Get() on new { MaterialId = p.MaterialId, LocaleId = p.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGrp
                    from m in mGrp.DefaultIfEmpty()
                    join v in Vendor.Get() on new { VendorId = po.VendorId, LocaleId = po.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGrp
                    from v in vGrp.DefaultIfEmpty()
                    join rl in rlAgg on new { POItemId = (decimal?)p.Id, RefLocaleId = (decimal?)p.LocaleId } equals new { POItemId = (decimal?)rl.POItemId, RefLocaleId = (decimal?)rl.RefLocaleId } into rlJoin
                    from rlx in rlJoin.DefaultIfEmpty()
                    select new
                    {
                        MaterialId = m.Id,
                        LocaleId = m.LocaleId,
                        Material = m.MaterialName,
                        MaterialEn = m.MaterialNameEng,
                        SamplingMethod = m.SamplingMethod,
                        CategoryCodeId = m.CategoryCodeId,
                        SemiGoods = m.SemiGoods,
                        TextureCodeId = m.TextureCodeId,
                        OrderNo = o.OrderNo,
                        OrdersId = o.Id,
                        ParentMaterialId = p.ParentMaterialId,
                        Total = pr.PlanQty,
                        UnitCodeId = p.UnitCodeId,
                        UnitNameTw = "",
                        StyleNo = o.StyleNo,
                        CompanyId = o.CompanyId,
                        CSD = o.CSD,
                        LCSD = o.LCSD,
                        PlanQty = pr.PlanQty,
                        PurQty = pr.PurQty,
                        VendorId = po.VendorId,
                        Vendor = v.ShortNameTw ?? "",
                        PODate = po.PODate,
                        POItemId = po.Id,
                        // 收料彙總（避免空→給預設）
                        ReceivedLogId = rlx.ReceivedLogId,
                        ReceivedQty = rlx.ReceivedQty,
                        ReceivedDate = rlx.ReceivedDate,
                    }
                )
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Distinct()
                .ToList();

                var _poItems = _allPOItems.Select(i => new
                {
                    i.MaterialId,
                    i.LocaleId,
                    i.Material,
                    i.MaterialEn,
                    i.SamplingMethod,
                    i.CategoryCodeId,
                    i.SemiGoods,
                    i.TextureCodeId,
                    i.OrderNo,
                    i.OrdersId,
                    i.ParentMaterialId,
                    i.Total,
                    i.UnitCodeId,
                    i.UnitNameTw,
                    i.StyleNo,
                    i.CompanyId,
                    i.CSD,
                    i.LCSD,
                })
                .ToList();

                var mrpItems = (
                    from o in Orders.Get()
                    join mrp in MRPItem.Get().GroupBy(i => new { i.MaterialId, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw }).Select(i => new { i.Key.MaterialId, i.Key.OrdersId, i.Key.LocaleId, i.Key.ParentMaterialId, i.Key.SizeDivision, i.Key.UnitCodeId, i.Key.UnitNameTw, Usage = i.Sum(g => g.Total) }) on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId }
                    join m in Material.Get() on new { MaterialId = mrp.MaterialId, LocaleId = mrp.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGrp
                    from m in mGrp.DefaultIfEmpty()
                    select new
                    {
                        MaterialId = m.Id,
                        LocaleId = m.LocaleId,
                        Material = m.MaterialName,
                        MaterialEn = m.MaterialNameEng,
                        SamplingMethod = m.SamplingMethod,
                        CategoryCodeId = m.CategoryCodeId,
                        SemiGoods = m.SemiGoods,
                        TextureCodeId = m.TextureCodeId,
                        OrderNo = o.OrderNo,
                        OrdersId = o.Id,
                        ParentMaterialId = mrp.ParentMaterialId,
                        Total = mrp.Usage,
                        UnitCodeId = mrp.UnitCodeId,
                        UnitNameTw = mrp.UnitNameTw,
                        StyleNo = o.StyleNo,
                        CompanyId = o.CompanyId,
                        CSD = o.CSD,
                        LCSD = o.LCSD,
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
                    select new
                    {
                        MaterialId = m.Id,
                        LocaleId = m.LocaleId,
                        Material = m.MaterialName,
                        MaterialEn = m.MaterialNameEng,
                        SamplingMethod = m.SamplingMethod,
                        CategoryCodeId = m.CategoryCodeId,
                        SemiGoods = m.SemiGoods,
                        TextureCodeId = m.TextureCodeId,
                        OrderNo = o.OrderNo,
                        OrdersId = o.Id,
                        ParentMaterialId = mrp.ParentMaterialId,
                        Total = mrp.Usage,
                        UnitCodeId = mrp.UnitCodeId,
                        UnitNameTw = mrp.UnitNameTw,
                        StyleNo = o.StyleNo,
                        CompanyId = o.CompanyId,
                        CSD = o.CSD,
                        LCSD = o.LCSD,
                    }
                )
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Distinct()
                .ToList();

                //Combind,
                var tmpItems = mrpItems.Union(mrpItemOrders).Union(_poItems)
                .Select(i => new
                {
                    OrdersId = i.OrdersId,
                    LocaleId = i.LocaleId,
                    SemiGoods = i.SemiGoods,
                    MaterialId = i.MaterialId,
                    Material = i.Material,
                    MaterialEn = i.MaterialEn,
                    UnitCodeId = i.UnitCodeId,
                    Unit = i.UnitNameTw,
                    Sub = i.ParentMaterialId > 0 ? 1 : 0,
                    StyleNo = i.StyleNo,
                    OrderNo = i.OrderNo,
                    Total = i.Total,
                    CompanyId = i.CompanyId,
                    CSD = i.CSD,
                    LCSD = i.LCSD,
                    ParentMaterialId = i.ParentMaterialId,
                    PlanQty = 0,
                    PurQty = 0,
                });

                if (tmpItems.Any())
                {
                    var bomItems = tmpItems
                    .GroupBy(g => new { g.CSD, g.LCSD, g.CompanyId, g.OrdersId, g.LocaleId, g.MaterialId, g.Material, g.MaterialEn, g.UnitCodeId, g.Unit, g.SemiGoods, g.Sub, g.OrderNo, g.StyleNo, g.ParentMaterialId })
                    .Select(g => new ERP.Models.Views.MaterialCoordinate
                    {
                        OrdersId = g.Key.OrdersId,
                        OrderNo = g.Key.OrderNo,
                        CSD = g.Key.CSD,
                        LCSD = g.Key.LCSD,
                        CompanyId = g.Key.CompanyId,
                        LocaleId = g.Key.LocaleId,
                        Material = g.Key.Material,
                        MaterialEn = g.Key.MaterialEn,
                        MaterialId = g.Key.MaterialId,
                        SemiGoods = g.Key.SemiGoods,
                        SecMat = g.Key.Sub,
                        Unit = g.Key.Unit,
                        UnitCodeId = g.Key.UnitCodeId,
                        StyleNo = g.Key.StyleNo,
                        PlanQty = g.Sum(i => i.Total),
                        ParentMaterialId = g.Key.ParentMaterialId,
                    })
                    .Distinct()
                    .ToList();

                    var localeId = bomItems[0].LocaleId;
                    var oIds = bomItems.Select(i => i.OrdersId).Distinct();
                    var oNos = bomItems.Select(i => i.OrderNo).Distinct();
                    var mIds = bomItems.Select(i => i.MaterialId).Distinct();
                    var poItemIds = _allPOItems.Select(i => i.POItemId).Distinct();
                    // var poItems = POItem.Get().Where(i => i.Status != 2 && oNos.Contains(i.OrderNo) && mIds.Contains(i.MaterialId)).Select(i => new { i.OrderNo, i.MaterialId, i.ParentMaterialId, i.Qty, Sub = i.ParentMaterialId > 0 ? 1 : 0 }).ToList();
                    // var purItems = PurBatchItem.Get().Where(i => i.Status != 2 && oIds.Contains(i.OrdersId) && mIds.Contains(i.MaterialId)).Select(i => new { i.OrdersId, i.MaterialId, i.ParentMaterialId, i.PlanQty, Sub = i.ParentMaterialId > 0 ? 1 : 0 }).ToList();
                    var stock = StockIO.Get().Where(i => oNos.Contains(i.OrderNo) && mIds.Contains(i.MaterialId)).Select(i => new { i.OrderNo, i.PCLIOQty, i.MaterialId, i.SourceType }).ToList();

                    bomItems.ForEach(i =>
                    {
                        var poItems = _allPOItems.Where(p => p.OrderNo == i.OrderNo && i.MaterialId == p.MaterialId && p.ParentMaterialId == i.ParentMaterialId).ToList();
                        if (poItems.Any())
                        {
                            i.PurPlanQty = poItems.Sum(p => p.PlanQty);
                            i.POQty = poItems.Sum(p => p.PurQty);
                            i.Vendor = poItems.Max(i => i.Vendor);
                            i.PODate = poItems.Max(i => i.PODate);
                            i.ReceivedQty = poItems.Sum(i => i.ReceivedQty);
                            i.ReceivedDate = poItems.Max(i => i.ReceivedDate);
                        }

                        i.StockQty = stock.Where(s => s.OrderNo == i.OrderNo && s.MaterialId == i.MaterialId).Sum(p => p.PCLIOQty);
                        i.StockInQty = stock.Where(s => s.OrderNo == i.OrderNo && s.MaterialId == i.MaterialId && s.PCLIOQty > 0 && s.SourceType <= 12).Sum(p => p.PCLIOQty);
                        i.StockOutQty = stock.Where(s => s.OrderNo == i.OrderNo && s.MaterialId == i.MaterialId && s.PCLIOQty < 0 && s.SourceType <= 12).Sum(p => p.PCLIOQty);
                        i.UsageRate = (i.PlanQty == 0 || i.StockOutQty == 0) ? 0 : Math.Round((decimal)((0 - i.StockOutQty) / i.PlanQty), 6, MidpointRounding.AwayFromZero);
                        i.LossRate = i.UsageRate == 0 ? 0 : i.UsageRate - 1;
                    });

                    items = bomItems;
                }

            }
            catch (Exception e)
            {
                throw e;
            }

            return items.AsQueryable();
        }

        // 材料配套表
        public IQueryable<Models.Views.MaterialCoordinate> GetMaterialCoordinate1(string predicate)
        {
            var items = new List<Models.Views.MaterialCoordinate>();

            try
            {
                var mrpItems = (
                    from o in Orders.Get()
                    join mrp in MRPItem.Get().GroupBy(i => new { i.MaterialId, i.OrdersId, i.LocaleId, i.ParentMaterialId, i.SizeDivision, i.UnitCodeId, i.UnitNameTw }).Select(i => new { i.Key.MaterialId, i.Key.OrdersId, i.Key.LocaleId, i.Key.ParentMaterialId, i.Key.SizeDivision, i.Key.UnitCodeId, i.Key.UnitNameTw, Usage = i.Sum(g => g.Total) }) on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId }
                    join m in Material.Get() on new { MaterialId = mrp.MaterialId, LocaleId = mrp.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGrp
                    from m in mGrp.DefaultIfEmpty()
                    select new
                    {
                        MaterialId = m.Id,
                        LocaleId = m.LocaleId,
                        Material = m.MaterialName,
                        MaterialEn = m.MaterialNameEng,
                        SamplingMethod = m.SamplingMethod,
                        CategoryCodeId = m.CategoryCodeId,
                        SemiGoods = m.SemiGoods,
                        TextureCodeId = m.TextureCodeId,
                        OrderNo = o.OrderNo,
                        OrdersId = o.Id,
                        ParentMaterialId = mrp.ParentMaterialId,
                        Total = mrp.Usage,
                        UnitCodeId = mrp.UnitCodeId,
                        UnitNameTw = mrp.UnitNameTw,
                        StyleNo = o.StyleNo,
                        CompanyId = o.CompanyId,
                        CSD = o.CSD,
                        LCSD = o.LCSD,
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
                    select new
                    {
                        MaterialId = m.Id,
                        LocaleId = m.LocaleId,
                        Material = m.MaterialName,
                        MaterialEn = m.MaterialNameEng,
                        SamplingMethod = m.SamplingMethod,
                        CategoryCodeId = m.CategoryCodeId,
                        SemiGoods = m.SemiGoods,
                        TextureCodeId = m.TextureCodeId,
                        OrderNo = o.OrderNo,
                        OrdersId = o.Id,
                        ParentMaterialId = mrp.ParentMaterialId,
                        Total = mrp.Usage,
                        UnitCodeId = mrp.UnitCodeId,
                        UnitNameTw = mrp.UnitNameTw,
                        StyleNo = o.StyleNo,
                        CompanyId = o.CompanyId,
                        CSD = o.CSD,
                        LCSD = o.LCSD,
                    }
                )
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Distinct()
                .ToList();

                //Combind,
                var tmpItems = mrpItems.Union(mrpItemOrders).Select(i => new
                {
                    OrdersId = i.OrdersId,
                    LocaleId = i.LocaleId,
                    SemiGoods = i.SemiGoods,
                    MaterialId = i.MaterialId,
                    Material = i.Material,
                    MaterialEn = i.MaterialEn,
                    UnitCodeId = i.UnitCodeId,
                    Unit = i.UnitNameTw,
                    Sub = i.ParentMaterialId > 0 ? 1 : 0,
                    StyleNo = i.StyleNo,
                    OrderNo = i.OrderNo,
                    Total = i.Total,
                    CompanyId = i.CompanyId,
                    CSD = i.CSD,
                    LCSD = i.LCSD,
                    ParentMaterialId = i.ParentMaterialId,

                });

                if (tmpItems.Any())
                {
                    var bomItems = tmpItems
                    .GroupBy(g => new { g.CSD, g.LCSD, g.CompanyId, g.OrdersId, g.LocaleId, g.MaterialId, g.Material, g.MaterialEn, g.UnitCodeId, g.Unit, g.SemiGoods, g.Sub, g.OrderNo, g.StyleNo, g.ParentMaterialId })
                    .Select(g => new ERP.Models.Views.MaterialCoordinate
                    {
                        OrdersId = g.Key.OrdersId,
                        OrderNo = g.Key.OrderNo,
                        CSD = g.Key.CSD,
                        LCSD = g.Key.LCSD,
                        CompanyId = g.Key.CompanyId,
                        LocaleId = g.Key.LocaleId,
                        Material = g.Key.Material,
                        MaterialEn = g.Key.MaterialEn,
                        MaterialId = g.Key.MaterialId,
                        SemiGoods = g.Key.SemiGoods,
                        SecMat = g.Key.Sub,
                        Unit = g.Key.Unit,
                        UnitCodeId = g.Key.UnitCodeId,
                        StyleNo = g.Key.StyleNo,
                        PlanQty = g.Sum(i => i.Total),
                        ParentMaterialId = g.Key.ParentMaterialId,
                    })
                    .Distinct()
                    .ToList();

                    var localeId = bomItems[0].LocaleId;
                    var oIds = bomItems.Select(i => i.OrdersId).Distinct();
                    var oNos = bomItems.Select(i => i.OrderNo).Distinct();
                    var mIds = bomItems.Select(i => i.MaterialId).Distinct();

                    var poItems = POItem.Get().Where(i => i.Status != 2 && oNos.Contains(i.OrderNo) && mIds.Contains(i.MaterialId)).Select(i => new { i.OrderNo, i.MaterialId, i.ParentMaterialId, i.Qty, Sub = i.ParentMaterialId > 0 ? 1 : 0 }).ToList();
                    var purItems = PurBatchItem.Get().Where(i => i.Status != 2 && oIds.Contains(i.OrdersId) && mIds.Contains(i.MaterialId)).Select(i => new { i.OrdersId, i.MaterialId, i.ParentMaterialId, i.PlanQty, Sub = i.ParentMaterialId > 0 ? 1 : 0 }).ToList();
                    var stock = StockIO.Get().Where(i => oNos.Contains(i.OrderNo) && mIds.Contains(i.MaterialId)).Select(i => new { i.OrderNo, i.PCLIOQty, i.MaterialId, i.SourceType }).ToList();

                    bomItems.ForEach(i =>
                    {
                        i.PurPlanQty = purItems.Where(p => p.OrdersId == i.OrdersId && p.MaterialId == i.MaterialId && p.ParentMaterialId == i.ParentMaterialId).Sum(p => p.PlanQty);
                        i.POQty = poItems.Where(p => p.OrderNo == i.OrderNo && i.MaterialId == p.MaterialId && p.ParentMaterialId == i.ParentMaterialId).Sum(p => p.Qty);
                        i.StockQty = stock.Where(s => s.OrderNo == i.OrderNo && s.MaterialId == i.MaterialId).Sum(p => p.PCLIOQty);
                        i.StockInQty = stock.Where(s => s.OrderNo == i.OrderNo && s.MaterialId == i.MaterialId && s.PCLIOQty > 0 && s.SourceType <= 12).Sum(p => p.PCLIOQty);
                        i.StockOutQty = stock.Where(s => s.OrderNo == i.OrderNo && s.MaterialId == i.MaterialId && s.PCLIOQty < 0 && s.SourceType <= 12).Sum(p => p.PCLIOQty);

                        i.UsageRate = (i.PlanQty == 0 || i.StockOutQty == 0) ? 0 : Math.Round((decimal)((0 - i.StockOutQty) / i.PlanQty), 6, MidpointRounding.AwayFromZero);
                        i.LossRate = i.UsageRate == 0 ? 0 : i.UsageRate - 1;
                    });

                    items = bomItems;
                }

            }
            catch (Exception e)
            {
                throw e;
            }

            return items.AsQueryable();
        }
        public IQueryable<Models.Views.MaterialSimple> GetMaterialForCoordinate(string predicate)
        {
            var items = new List<Models.Views.MaterialSimple>();

            try
            {
                var mrpItems = (
                    from o in Orders.Get()
                    join mrp in MRPItem.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId }
                    join m in Material.Get() on new { MaterialId = mrp.MaterialId, LocaleId = mrp.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGrp
                    from m in mGrp.DefaultIfEmpty()
                    select new
                    {
                        MaterialId = m.Id,
                        LocaleId = m.LocaleId,
                        Material = m.MaterialName,
                        MaterialEn = m.MaterialNameEng,
                        // SamplingMethod = m.SamplingMethod,
                        // CategoryCodeId = m.CategoryCodeId,
                        // SemiGoods = m.SemiGoods,
                        // TextureCodeId = m.TextureCodeId,
                        OrderNo = o.OrderNo,
                        // OrdersId = o.Id,
                        // ParentMaterialId = mrp.ParentMaterialId,
                        // UnitCodeId = mrp.UnitCodeId,
                        // UnitNameTw = mrp.UnitNameTw,
                        // StyleNo = o.StyleNo,
                        CompanyId = o.CompanyId,
                        CSD = o.CSD,
                        LCSD = o.LCSD,
                    }
                )
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Select(i => new Models.Views.MaterialSimple
                {
                    LocaleId = i.LocaleId,
                    Id = i.MaterialId,
                    Material = i.Material,
                    MaterialNameEng = i.MaterialEn,
                })
                .Distinct()
                .ToList();

                var mrpItemOrders = (
                    from o in Orders.Get()
                    join mrp in MRPItemOrders.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId }
                    join m in Material.Get() on new { MaterialId = mrp.MaterialId, LocaleId = mrp.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGrp
                    from m in mGrp.DefaultIfEmpty()
                    select new
                    {
                        MaterialId = m.Id,
                        LocaleId = m.LocaleId,
                        Material = m.MaterialName,
                        MaterialEn = m.MaterialNameEng,
                        // SamplingMethod = m.SamplingMethod,
                        // CategoryCodeId = m.CategoryCodeId,
                        // SemiGoods = m.SemiGoods,
                        // TextureCodeId = m.TextureCodeId,
                        OrderNo = o.OrderNo,
                        // OrdersId = o.Id,
                        // ParentMaterialId = mrp.ParentMaterialId,
                        // UnitCodeId = mrp.UnitCodeId,
                        // UnitNameTw = mrp.UnitNameTw,
                        // StyleNo = o.StyleNo,
                        CompanyId = o.CompanyId,
                        CSD = o.CSD,
                        LCSD = o.LCSD,
                    }
                )
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Select(i => new Models.Views.MaterialSimple
                {
                    LocaleId = i.LocaleId,
                    Id = i.MaterialId,
                    Material = i.Material,
                    MaterialNameEng = i.MaterialEn,
                })
                .Distinct()
                .ToList();

                //Combind,
                var tmpItems = mrpItems.Union(mrpItemOrders).Select(i => new Models.Views.MaterialSimple
                {
                    LocaleId = i.LocaleId,
                    Id = i.Id,
                    Material = i.Material,
                    MaterialNameEng = i.MaterialNameEng,
                })
                .ToList()
                .Distinct();

                return tmpItems.AsQueryable();
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}