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

namespace ERP.Services.Search
{
    public class BondService : SearchService
    {
        private ERP.Services.Entities.MaterialStockService MaterialStock { get; }
        private ERP.Services.Entities.MaterialService Material { get; }
        private ERP.Services.Entities.OrdersService Orders { get; }
        private ERP.Services.Entities.StockIOService StockIO { get; }
        private ERP.Services.Entities.WarehouseService WareHouse { get; }
        private ERP.Services.Entities.BondMaterialChinaContrastService BondMaterialChinaContrast { get; }
        private ERP.Services.Entities.BondMRPService BondMRP { get; }
        private ERP.Services.Entities.BondMRPItemService BondMRPItem { get; }

        private ERP.Services.Business.BondMRPService BondMRPGroup { get; set; }

        private ERP.Services.Entities.OrdersItemService OrdersItem { get; }
        private ERP.Services.Entities.ArticleSizeRunService ArticleSizeRun { get; }
        private ERP.Services.Entities.BondProductChinaContrastService BondProductChinaContrast { get; }
        private ERP.Services.Entities.SimpleSaleService SimpleSale { get; }
        private ERP.Services.Entities.POItemService POItem { get; }
        private ERP.Services.Entities.CodeItemService CodeItem { get; }

        public BondService(
            ERP.Services.Entities.MaterialStockService materialStockService,
            ERP.Services.Entities.MaterialService materialService,
            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Entities.StockIOService stockIOService,
            ERP.Services.Entities.WarehouseService warehouseService,
            ERP.Services.Entities.BondMaterialChinaContrastService bondMaterialChinaContrastService,
            ERP.Services.Entities.BondMRPService bondMRPService,
            ERP.Services.Entities.BondMRPItemService bondMRPItemService,
            ERP.Services.Entities.OrdersItemService ordersItemService,
            ERP.Services.Entities.ArticleSizeRunService articleSizeRunService,
            ERP.Services.Entities.BondProductChinaContrastService bondProductChinaContrastService,
            ERP.Services.Business.BondMRPService bondMRPGroupService,
            ERP.Services.Entities.SimpleSaleService simpleSale,
            ERP.Services.Entities.POItemService poItemService,
            ERP.Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MaterialStock = materialStockService;
            Material = materialService;
            Orders = ordersService;
            StockIO = stockIOService;
            WareHouse = warehouseService;
            BondMaterialChinaContrast = bondMaterialChinaContrastService;
            BondMRP = bondMRPService;
            BondMRPItem = bondMRPItemService;
            OrdersItem = ordersItemService;
            ArticleSizeRun = articleSizeRunService;
            BondProductChinaContrast = bondProductChinaContrastService;
            BondMRPGroup = bondMRPGroupService;
            POItem = poItemService;
            SimpleSale = simpleSale;
            CodeItem = codeItemService;
        }
        public IQueryable<Models.Views.BondMaterialStockItem> GetBondMaterialStockItem(string predicate, string[] filters)
        {

            var extendPredicate = (filters == null || filters.Length == 0) ? "1=1" : String.Join(" && ", filters);

            var items = (
                    from sio in StockIO.Get()
                    join ms in MaterialStock.Get() on new { MaterialStockId = sio.MaterialStockId, LocaleId = sio.LocaleId } equals new { MaterialStockId = ms.Id, LocaleId = ms.LocaleId }
                    join w in WareHouse.Get() on new { WarehouseId = sio.WarehouseId, LocaleId = sio.LocaleId } equals new { WarehouseId = w.Id, LocaleId = w.LocaleId }
                    join bm in BondMaterialChinaContrast.Get().Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, extendPredicate) on new { Material = ms.MaterialName } equals new { Material = bm.MaterialName } into bmGRP
                    from bm in bmGRP.DefaultIfEmpty()
                    join b in BondMRP.Get().Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, extendPredicate) on new { OrderNo = sio.OrderNo } equals new { OrderNo = b.OrderNo } into bGRP
                    from b in bGRP.DefaultIfEmpty()
                    select new
                    {
                        StockQty = 0,
                        PurQty = 0,
                        Weight = 0,
                        BelongCompanyId = w.BelongCompanyId,

                        IODate = sio.IODate,
                        OrderNo = sio.OrderNo,
                        SourceType = sio.SourceType,
                        OrgUnitNameTw = sio.OrgUnitNameTw,
                        MaterialId = sio.MaterialId,
                        MaterialName = ms.MaterialName,
                        PCLUnitNameTw = ms.PCLUnitNameTw,

                        LocaleId = sio.LocaleId,
                        PCLIOQty = sio.PCLIOQty,
                        MaterialStockId = sio.MaterialStockId,

                        BondMaterialName = bm.BondMaterialName,
                        WeightEachUnit = bm.WeightEachUnit,
                        SaleDate = b.SalesDate,
                    }
                )
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .GroupBy(g => new { g.SaleDate, g.IODate, g.OrderNo, g.SourceType, g.OrgUnitNameTw, g.MaterialId, g.MaterialName, g.PCLUnitNameTw, g.BondMaterialName, g.WeightEachUnit, g.LocaleId })
                .Select(i => new ERP.Models.Views.BondMaterialStockItem
                {
                    SaleDate = i.Key.SaleDate,
                    IODate = i.Key.IODate,
                    OrderNo = i.Key.OrderNo,
                    SourceType = i.Key.SourceType,
                    OrgUnitNameTw = i.Key.OrgUnitNameTw,
                    MaterialId = i.Key.MaterialId,
                    MaterialName = i.Key.MaterialName,
                    PCLUnitNameTw = i.Key.PCLUnitNameTw,
                    BondMaterialName = i.Key.BondMaterialName,
                    WeightEachUnit = i.Key.WeightEachUnit,
                    PCLIOQty = i.Sum(g => g.PCLIOQty),
                    Weight = i.Key.WeightEachUnit == null ? 0 : i.Key.WeightEachUnit * i.Sum(g => g.PCLIOQty),
                    StockQty = 0,
                    PurQty = 0,
                    Id = 0,
                    LocaleId = i.Key.LocaleId,
                })
                .ToList();

            var orderNos = items.Select(i => i.OrderNo).Distinct();
            var mIds = items.Select(i => i.MaterialId).Distinct();
            // var poItems = POItem.Get().Where(i => orderNos.Contains(i.OrderNo) && mIds.Contains(i.MaterialId)).Select(i => new {i.OrderNo, i.OrdersId, i.MaterialId, i.DollarCodeId});
            var poItems = (
                from pi in POItem.Get()
                join ci in CodeItem.Get() on new { DollarCodeId = pi.DollarCodeId, LocaleId = pi.LocaleId } equals new {  DollarCodeId = ci.Id, LocaleId = ci.LocaleId }
                where orderNos.Contains(pi.OrderNo) && mIds.Contains( pi.MaterialId)
                select new {
                   OrderNo = pi.OrderNo, 
                   OrdersId = pi.OrdersId, 
                   MaterialId = pi.MaterialId, 
                   DollarCodeId = pi.DollarCodeId,
                   Dollar = ci.NameTW,
                }
            )
            .Distinct()
            .ToList();

            items.ForEach(i => {
                i.Currency =  string.Join(",",poItems.Where(p => p.OrderNo == i.OrderNo && p.MaterialId == i.MaterialId).Select(p => p.Dollar).ToArray());
            });
            return items.AsQueryable();
        }

        public IQueryable<Models.Views.BondMaterialStockBalance> GetBondMaterialStockBalance(string predicate, string[] filters)
        {
            var extendPredicate = (filters == null || filters.Length == 0) ? "1=1" : String.Join(" && ", filters);
            var bondMRPItem = BondMRPItem.Get().GroupBy(i => new { i.WeightEachUnit, i.BondMaterialName, i.OrderNo, i.MaterialNameTw, i.LocaleId }).Select(i => new { VendorShortNameTw = i.Max(g => g.VendorShortNameTw), BondMaterialName = i.Key.BondMaterialName, WeightEachUnit = i.Key.WeightEachUnit, OrderNo = i.Key.OrderNo, MaterialNameTw = i.Key.MaterialNameTw, LocaleId = i.Key.LocaleId });
            var items = (
                    from ms in MaterialStock.Get()
                    join w in WareHouse.Get() on new { WareHouseId = ms.WarehouseId, LocaleId = ms.LocaleId } equals new { WareHouseId = w.Id, LocaleId = w.LocaleId }
                    join bm in BondMaterialChinaContrast.Get().Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, extendPredicate) on new { Material = ms.MaterialName } equals new { Material = bm.MaterialName } into bmGRP
                    from bm in bmGRP.DefaultIfEmpty()
                    join b in BondMRP.Get().Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, extendPredicate) on new { OrderNo = ms.OrderNo } equals new { OrderNo = b.OrderNo } into bGRP
                    from b in bGRP.DefaultIfEmpty()
                    join bi in bondMRPItem on new { OrderNo = ms.OrderNo, Material = ms.MaterialName } equals new { OrderNo = bi.OrderNo, Material = bi.MaterialNameTw } into biGRP
                    from bi in biGRP.DefaultIfEmpty()
                    select new
                    {
                        LocaleId = ms.LocaleId,
                        WarehouseNo = ms.WarehouseNo,
                        MaterialId = ms.MaterialId,
                        MaterialName = ms.MaterialName,
                        OrderNo = ms.OrderNo,
                        PCLQty = ms.PCLQty,
                        PCLUnitNameTw = ms.PCLUnitNameTw,
                        BelongCompanyId = w.BelongCompanyId,
                        BondNo = b.BondNo,
                        BondMaterialName = bi.BondMaterialName,
                        WeightEachUnit = bi.WeightEachUnit,
                        VendorShortNameTw = bi.VendorShortNameTw,
                        StockDollarNameTw = ms.StockDollarNameTw,
                        // RealPCLQty = StockIO.Get().Where(i => i.MaterialStockId == ms.Id && i.LocaleId == ms.LocaleId).Sum(g => g.PCLIOQty),
                    }
                )
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .GroupBy(g => new { g.LocaleId, g.MaterialId, g.MaterialName, g.OrderNo, g.PCLUnitNameTw, g.StockDollarNameTw, g.BelongCompanyId, g.BondMaterialName, g.WeightEachUnit, g.VendorShortNameTw, g.BondNo })
                .Select(i => new ERP.Models.Views.BondMaterialStockBalance
                {
                    LocaleId = i.Key.LocaleId,
                    MaterialId = i.Key.MaterialId,
                    MaterialName = i.Key.MaterialName,
                    OrderNo = i.Key.OrderNo,
                    PCLUnitNameTw = i.Key.PCLUnitNameTw,
                    BelongCompanyId = i.Key.BelongCompanyId,
                    BondMaterialName = i.Key.BondMaterialName,
                    WeightEachUnit = i.Key.WeightEachUnit,
                    VendorShortNameTw = i.Key.VendorShortNameTw,
                    StockDollarNameTw = i.Key.StockDollarNameTw,
                    PCLQty = i.Sum(g => g.PCLQty),
                    // RealPCLQty = i.Sum(g => g.RealPCLQty),
                    Weight = i.Key.WeightEachUnit == null ? 0 : i.Key.WeightEachUnit * i.Sum(g => g.PCLQty),
                    // WarehouseNo = i.Key.WarehouseNo,
                    BondNo = i.Key.BondNo,
                })
                .ToList();

            var orderNos = items.Select(i => i.OrderNo).Distinct().ToList();
            var mIds = items.Select(i => i.MaterialId).Distinct().ToList();
            // var poItems = POItem.Get().Where(i => orderNos.Contains(i.OrderNo) && mIds.Contains(i.MaterialId)).Select(i => new {i.OrderNo, i.OrdersId, i.MaterialId, i.DollarCodeId});
            var poItems = (
                from pi in POItem.Get()
                join ci in CodeItem.Get() on new { DollarCodeId = pi.DollarCodeId, LocaleId = pi.LocaleId } equals new {  DollarCodeId = ci.Id, LocaleId = ci.LocaleId }
                where orderNos.Contains(pi.OrderNo) && mIds.Contains( pi.MaterialId)
                select new {
                   OrderNo = pi.OrderNo, 
                   OrdersId = pi.OrdersId, 
                   MaterialId = pi.MaterialId, 
                   DollarCodeId = pi.DollarCodeId,
                   Dollar = ci.NameTW,
                }
            )
            .Distinct()
            .ToList();

            items.ForEach(i => {
                i.Currency =  string.Join(",",poItems.Where(p => p.OrderNo == i.OrderNo && p.MaterialId == i.MaterialId).Select(p => p.Dollar).ToArray());
            });
            

            return items.AsQueryable();

        }
        public IQueryable<Models.Views.BondOrderSizeRun> GetBondOrderSizeRun(string predicate, string[] filters)
        {
            var sizeruns = new List<Models.Views.BondOrderSizeRun>();

            var ordersItems = (
                from o in Orders.Get()
                join oi in OrdersItem.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = (decimal)oi.OrdersId, LocaleId = oi.LocaleId }
                join asr in ArticleSizeRun.Get() on new { ArticleId = o.ArticleId, LocaleId = o.LocaleId, ArticleInnerSize = oi.ArticleInnerSize } equals new { ArticleId = asr.ArticleId, LocaleId = asr.LocaleId, ArticleInnerSize = (decimal)asr.ArticleInnerSize }
                join b in BondProductChinaContrast.Get() on new { StyleNo = o.StyleNo, LocaleId = o.LocaleId } equals new { StyleNo = b.StyleNo, LocaleId = b.LocaleId } into bGRP
                from b in bGRP.DefaultIfEmpty()
                select new
                {
                    OrdersId = o.Id,
                    LocaleId = o.LocaleId,
                    CompanyNo = o.CompanyNo,
                    OrderQty = o.OrderQty,
                    OrderNo = o.OrderNo,
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    Brand = o.Brand,
                    ArticleNo = o.ArticleNo,
                    StyleNo = o.StyleNo,

                    ArticleSize = oi.ArticleSize,
                    ArticleSizeSuffix = oi.ArticleSizeSuffix,
                    ArticleInnerSize = oi.ArticleInnerSize,
                    ArticleDisplay = asr.ArticleDisplaySize,

                    Qty = oi.Qty,
                    BondProductName = b.BondProductName,
                    Status = o.Status,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            var ordersHeads = ordersItems.Select(o => new
            {
                OrdersId = o.OrdersId,
                LocaleId = o.LocaleId,
                CompanyNo = o.CompanyNo,
                OrderQty = o.OrderQty,
                OrderNo = o.OrderNo,
                CSD = o.CSD,
                LCSD = o.LCSD,
                ArticleNo = o.ArticleNo,
                StyleNo = o.StyleNo,
                Brand = o.Brand,
                BondProductName = o.BondProductName,
            })
            .Distinct()
            .ToList();

            var articleHead = new List<string>();
            ordersHeads.ForEach(i =>
            {
                var s = new Models.Views.BondOrderSizeRun();
                var articleSizeRun = new List<string>();

                s.OrdersId = i.OrdersId;
                s.LocaleId = i.LocaleId;
                s.CompanyNo = i.CompanyNo;
                s.OrderQty = i.OrderQty;
                s.OrderNo = i.OrderNo;
                s.CSD = i.CSD;
                s.LCSD = i.LCSD;
                s.ArticleNo = i.ArticleNo;
                s.StyleNo = i.StyleNo;
                s.Brand = i.Brand;
                s.BondProductName = i.BondProductName;

                ordersItems.Where(oi => oi.OrdersId == i.OrdersId && oi.LocaleId == i.LocaleId).ToList().ForEach(oi =>
                {
                    var field = "";

                    // 組合訂單 =========
                    if (oi.ArticleSizeSuffix != null && (oi.ArticleSizeSuffix.Contains("J") || oi.ArticleSizeSuffix.Contains("j")))
                    {
                        field = "S" + String.Format("{0:000000}", (oi.ArticleInnerSize * 10));
                    }
                    else
                    {
                        field = "S" + String.Format("{0:000000}", (oi.ArticleInnerSize * 10));
                    }


                    articleSizeRun.Add("\"" + field + "\":" + oi.Qty);
                    articleHead.Add("\"" + field + "\":\"" + oi.ArticleDisplay + "\"");
                });

                s.ArticleSizeRun = "{" + string.Join(",", articleSizeRun) + "}";
                sizeruns.Add(s);
            });

            sizeruns.ForEach(i =>
            {
                i.ArticleHead = "{" + string.Join(",", articleHead.Distinct().OrderBy(c => c)) + "}";
            });

            return sizeruns.AsQueryable();
        }

        public IQueryable<Models.Views.BondMRP> GetBondMRP(string predicate)
        {
            return BondMRPGroup.Get().Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate);
        }
        public ERP.Models.Views.BondMRPGroup GetBondMRPGroup(int id, int localeId)
        {
            return BondMRPGroup.GetGroup(id, localeId);
        }

        public IQueryable<Models.Views.BondMRPItem> GetAllBondMRPItem(string predicate)
        {
            var result = (
                from o in Orders.Get().Where(i => i.Status != 3 && i.OrderType == 2)
                join b in BondMRP.Get() on new { Orders = o.OrderNo } equals new { Orders = b.OrderNo } into bGRP
                from b in bGRP.DefaultIfEmpty()
                join p in BondProductChinaContrast.Get() on new { StyleNo = o.StyleNo } equals new { StyleNo = p.StyleNo } into pGRP
                from p in pGRP.DefaultIfEmpty()
                join bi in BondMRPItem.Get() on new { b.OrderNo, b.LocaleId } equals new { bi.OrderNo, bi.LocaleId } into biGRP
                from bi in biGRP.DefaultIfEmpty()
                select new
                {
                    Id = bi.Id,
                    LocaleId = bi.LocaleId,
                    OrderNo = bi.OrderNo,
                    IsAdh = bi.IsAdh,
                    IsSub = bi.IsSub,
                    PartNo = bi.PartNo,
                    PartNameTw = bi.PartNameTw,
                    MaterialNameTw = bi.MaterialNameTw,
                    UnitNameTw = bi.UnitNameTw,
                    Total = bi.Total,
                    WeightEachUnit = bi.WeightEachUnit,
                    Weight = bi.Weight,
                    BondMaterialName = bi.BondMaterialName,
                    VendorShortNameTw = bi.VendorShortNameTw,
                    SeqNo = bi.SeqNo,
                    ModifyUserName = bi.ModifyUserName,
                    LastUpdateTime = bi.LastUpdateTime,
                    PurDollarNameTw = bi.PurDollarNameTw,
                    PayDollarNameTw = bi.PayDollarNameTw,
                    ExDollarNameTw = bi.ExDollarNameTw,

                    CompanyId = o.CompanyId,
                    CompanyNo = o.CompanyNo,
                    CustomerName = o.Customer,
                    StyleNo = o.StyleNo,
                    ShoeName = o.ShoeName,
                    OrderQty = o.OrderQty,
                    // ShipQty = SimpleSale.Get().Where(i => i.OrdersId == o.Id && i.RefLocaleId == o.LocaleId).Select(i => (int?)i.SaleQty).Sum() ?? 0,
                    // ShortQty = o.OrderQty - (SimpleSale.Get().Where(i => i.OrdersId == o.Id && i.RefLocaleId == o.LocaleId).Select(i => (int?)i.SaleQty).Sum() ?? 0),
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    LastNo = o.LastNo,
                    OutsoleNo = o.OutsoleNo,
                    CSDYM = "",
                    IsClose = b.IsClose,
                    BondOrderNo = b.OrderNo,
                    BOMLocaleId = b == null ? o.LocaleId : b.BOMLocaleId,
                    BondStyleNo = b.StyleNo,
                    BondProductName = b.BondProductName,
                    SalesDate = b.SalesDate,
                    BondNo = b.BondNo,
                    RefBondProductName = p.BondProductName,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new Models.Views.BondMRPItem
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                OrderNo = i.OrderNo,
                IsAdh = i.IsAdh,
                IsSub = i.IsSub,
                PartNo = i.PartNo,
                PartNameTw = i.PartNameTw,
                MaterialNameTw = i.MaterialNameTw,
                UnitNameTw = i.UnitNameTw,
                Total = i.Total,
                WeightEachUnit = i.WeightEachUnit,
                Weight = i.Weight,
                BondMaterialName = i.BondMaterialName,
                VendorShortNameTw = i.VendorShortNameTw,
                SeqNo = i.SeqNo,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                PurDollarNameTw = i.PurDollarNameTw,
                PayDollarNameTw = i.PayDollarNameTw,
                ExDollarNameTw = i.ExDollarNameTw,

   
                IsClose = i.IsClose,
                BOMLocaleId = i.BOMLocaleId,
                StyleNo = i.StyleNo,
                BondProductName = i.BondProductName,
                SalesDate = i.SalesDate,
                BondNo = i.BondNo,
                CompanyId = i.CompanyId,
                CompanyNo = i.CompanyNo,
                LCSD = i.LCSD,
                CSD = i.CSD,
                OrderQty = i.OrderQty,
            })
            .ToList();

            return result.AsQueryable();
        }
    }
}