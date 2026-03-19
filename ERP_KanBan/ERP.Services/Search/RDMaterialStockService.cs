using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Search
{
    public class RDMaterialStockService : SearchService
    {
        private ERP.Services.Entities.PMStockInService RDMaterialStockIn { get; set; }
        private ERP.Services.Entities.PMStockOutService RDMaterialStockOut { get; set; }
        private ERP.Services.Entities.MaterialService Material { get; set; }
        private ERP.Services.Entities.VendorService Vendor { get; set; }
        private ERP.Services.Entities.CodeItemService CodeItem { get; set; }
        private ERP.Services.Entities.CompanyService Company { get; set; }
        private ERP.Services.Entities.WarehouseService Warehouse { get; set; }

        public RDMaterialStockService(
            ERP.Services.Entities.PMStockInService pmStockInService,
            ERP.Services.Entities.PMStockOutService pmStockOutService,
            ERP.Services.Entities.MaterialService materialService,
            ERP.Services.Entities.VendorService vendorService,
            ERP.Services.Entities.ArticleSizeRunService articleSizeRunService,
            ERP.Services.Entities.CodeItemService codeItemService,
            ERP.Services.Entities.CompanyService companyService,
            ERP.Services.Entities.WarehouseService warehouseService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            RDMaterialStockIn = pmStockInService;
            RDMaterialStockOut = pmStockOutService;

            Material = materialService;
            Vendor = vendorService;
            CodeItem = codeItemService;
            Company = companyService;
            Warehouse = warehouseService;
        }

        public IQueryable<Models.Views.RDMaterialStockIn> GetRDMaterialStockIn(string predicate, string[] filters)
        {
            var result = (
                from si in RDMaterialStockIn.Get()
                join w in Warehouse.Get().Where(i => i.CloseOff == 0) on new { WarehouseNo = si.WarehouseNo, LocaleId = si.LocaleId } equals new { WarehouseNo = w.WarehouseNo, LocaleId = w.LocaleId }
                select new Models.Views.RDMaterialStockIn
                {
                    Id = si.Id,
                    LocaleId = si.LocaleId,
                    Type = si.Type,
                    InRefNo = si.InRefNo,
                    WarehouseNo = si.WarehouseNo,
                    ReceiveRefNo = si.ReceiveRefNo,
                    TypeNo = si.TypeNo,
                    IODate = si.InDate,
                    MaterialName = si.MaterialName,
                    Spec = si.Spec,
                    SubQty = si.SubQty,
                    Unit = si.Unit,
                    Barcode = si.Barcode,
                    Remark = si.Remark,
                    ModifyUserName = si.ModifyUserName,
                    LastUpdateTime = si.LastUpdateTime,
                    NWeight = si.NWeight,
                    GWeight = si.GWeight,
                    LocationDesc = si.LocationDesc,

                    ItemSeqNo = si.ItemSeqNo,
                    UnitPrice = si.UnitPrice,
                    GetValue = si.GetValue,
                    DollarName = si.DollarName,
                    RefPPOIId = si.RefPPOIId,
                    StyleNo = si.RefOutOrderNo,

                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            return result.AsQueryable();
        }

        public IQueryable<Models.Views.RDMaterialStockOut> GetRDMaterialStockOut(string predicate, string[] filters)
        {
            var result = (
                from so in RDMaterialStockOut.Get()
                join w in Warehouse.Get().Where(i => i.CloseOff == 0) on new { WarehouseNo = so.WarehouseNo, LocaleId = so.LocaleId } equals new { WarehouseNo = w.WarehouseNo, LocaleId = w.LocaleId }
                select new Models.Views.RDMaterialStockOut
                {
                    Id = so.Id,
                    LocaleId = so.LocaleId,
                    Type = so.Type,
                    OutRefNo = so.OutRefNo,
                    WarehouseNo = so.WarehouseNo,
                    ReceiveRefNo = so.ReceiveRefNo,
                    TypeNo = so.TypeNo,
                    IODate = so.OutDate,
                    MaterialName = so.MaterialName,
                    Spec = so.Spec,
                    SubQty = so.SubQty,
                    Unit = so.Unit,
                    Barcode = so.Barcode,
                    Remark = so.Remark,
                    ModifyUserName = so.ModifyUserName,
                    LastUpdateTime = so.LastUpdateTime,
                    NWeight = so.NWeight,
                    GWeight = so.GWeight,
                    LocationDesc = so.LocationDesc,

                    OrderNo = so.OrderNo,
                    BookingDate = so.BookingDate,
                    RefDeptNo = so.RefDeptNo,
                    RefUserName = so.RefUserName,
                    IsClose = so.IsClose,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            return result.AsQueryable();
        }

        public IQueryable<Models.Views.RDMaterialStockBalance> GetRDMaterialStockBalance(string predicate, string[] filters)
        {
            var stockBalance = new List<Models.Views.RDMaterialStockBalance>();

            var stockOutPredicate = "";
            if (filters != null && filters.Length > 0)
            {
                //condition
                var tmpFilters = filters.Where(i => i.Contains("OutDate")).ToArray();
                stockOutPredicate = tmpFilters.Length > 0 ? String.Join(" && ", tmpFilters) : "1=1";
            }

            var stockInPredicate = "";
            if (filters != null && filters.Length > 0)
            {
                //condition
                var tmpFilters = filters.Where(i => i.Contains("InDate")).ToArray();
                stockInPredicate = tmpFilters.Length > 0 ? String.Join(" && ", tmpFilters) : "1=1";
            }

            var groupBy = 1;
            if (filters != null && filters.Length > 0)
            {
                //condition
                var items = filters.Where(i => i.Contains("Group = 1")).ToArray();
                groupBy = items.Length > 0 ? 1 : 2;
            }

            if (groupBy == 1)
            {
                var stockIns = RDMaterialStockIn.Get().Where(stockInPredicate).Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .GroupBy(g => new { g.Barcode, g.LocaleId })
                .Select(i => new
                {
                    RefPPOIId = i.Max(g => g.RefPPOIId),
                    Warehouse = i.Max(g => g.WarehouseNo),
                    LocationDesc = i.Max(g => g.LocationDesc),
                    MaterialName = i.Max(g => g.MaterialName),
                    Unit = i.Max(g => g.Unit),
                    Barcode = i.Key.Barcode,
                    LocaleId = i.Key.LocaleId,
                    SubQty = (decimal?)i.Sum(g => g.SubQty),
                    NWeight = i.Sum(g => g.NWeight),
                    GWeight = i.Sum(g => g.GWeight),
                });

                var stockOuts = RDMaterialStockOut.Get().Where(stockOutPredicate).Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                    .GroupBy(g => new { g.Barcode, g.LocaleId })
                    .Select(i => new
                    {
                        Barcode = i.Key.Barcode,
                        LocaleId = i.Key.LocaleId,
                        SubQty = (decimal?)i.Sum(g => g.SubQty),
                        NWeight = i.Sum(g => g.NWeight),
                        GWeight = i.Sum(g => g.GWeight),
                    });

                stockBalance = (
                    from i in stockIns
                    join o in stockOuts on new { Barcode = i.Barcode, LocaleId = i.LocaleId } equals new { Barcode = o.Barcode, LocaleId = o.LocaleId } into oGRP
                    from o in oGRP.DefaultIfEmpty()
                    join w in Warehouse.Get().Select(i => new { i.WarehouseNo, i.CloseOff, i.LocaleId }).Where(i => i.CloseOff == 0) on new { Warehouse = i.Warehouse, LocaleId = i.LocaleId } equals new { Warehouse = w.WarehouseNo, LocaleId = w.LocaleId }
                    select new Models.Views.RDMaterialStockBalance
                    {
                        LocaleId = i.LocaleId,
                        MaterialName = i.MaterialName,
                        WarehouseNo = i.Warehouse,
                        LocationDesc = i.LocationDesc,
                        Unit = i.Unit,
                        StockInQty = i.SubQty ?? (decimal)0,
                        StockOutQty = o.SubQty ?? (decimal)0,
                        StockQty = (i.SubQty ?? (decimal)0) - (o.SubQty ?? (decimal)0),
                        Barcode = i.Barcode,
                    }
                )
                .Where(i => i.StockQty != 0)
                .ToList();
            }
            else
            {
                var stockIns = RDMaterialStockIn.Get().Where(stockInPredicate).Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .GroupBy(g => new { g.MaterialName, g.WarehouseNo, g.Unit, g.LocaleId })
                .Select(i => new
                {
                    RefPPOIId = i.Max(g => g.RefPPOIId),
                    Warehouse = i.Key.WarehouseNo,
                    // LocationDesc = i.Max(g => g.LocationDesc),
                    MaterialName = i.Key.MaterialName,
                    Unit = i.Key.Unit,
                    // Barcode = "",
                    LocaleId = i.Key.LocaleId,
                    SubQty = (decimal?)i.Sum(g => g.SubQty),
                    NWeight = i.Sum(g => g.NWeight),
                    GWeight = i.Sum(g => g.GWeight),
                });

                var stockOuts = RDMaterialStockOut.Get().Where(stockOutPredicate).Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                    .GroupBy(g => new { g.MaterialName, g.WarehouseNo, g.Unit, g.LocaleId })
                    .Select(i => new
                    {
                        // Barcode = "",
                        LocaleId = i.Key.LocaleId,
                        Warehouse = i.Key.WarehouseNo,
                        MaterialName = i.Key.MaterialName,
                        Unit = i.Key.Unit,
                        SubQty = (decimal?)i.Sum(g => g.SubQty),
                        NWeight = i.Sum(g => g.NWeight),
                        GWeight = i.Sum(g => g.GWeight),
                    });

                stockBalance = (
                    from i in stockIns
                    join o in stockOuts on new { MaterialName = i.MaterialName, WarehouseNo = i.Warehouse, Unit = i.Unit, LocaleId = i.LocaleId } equals new { MaterialName = o.MaterialName, WarehouseNo = o.Warehouse, Unit = o.Unit, LocaleId = o.LocaleId }  into oGRP
                    from o in oGRP.DefaultIfEmpty()
                    join w in Warehouse.Get().Select(i => new { i.WarehouseNo, i.CloseOff, i.LocaleId }).Where(i => i.CloseOff == 0) on new { Warehouse = i.Warehouse, LocaleId = i.LocaleId } equals new { Warehouse = w.WarehouseNo, LocaleId = w.LocaleId }
                    select new Models.Views.RDMaterialStockBalance
                    {
                        LocaleId = i.LocaleId,
                        MaterialName = i.MaterialName,
                        WarehouseNo = i.Warehouse,
                        LocationDesc = "",
                        Unit = i.Unit,
                        StockInQty = i.SubQty ?? (decimal)0,
                        StockOutQty = o.SubQty ?? (decimal)0,
                        StockQty = (i.SubQty ?? (decimal)0) - (o.SubQty ?? (decimal)0),
                        Barcode = "",
                    }
                )
                .Where(i => i.StockQty != 0)
                .ToList();
            }

            return stockBalance.AsQueryable();

        }

    }
}