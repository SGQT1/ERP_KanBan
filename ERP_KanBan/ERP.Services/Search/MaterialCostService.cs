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
using ERP.Services.Bases;
using ERP.Services.Business.Entities;

namespace ERP.Services.Search
{
    public class MaterialCostService : SearchService
    {
        // private ERP.Services.Business.Entities.TypeService Type { get; set; }
        private ERP.Services.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Entities.MaterialStockBatchCostService MaterialStockBatchCost { get; set; }
        private ERP.Services.Entities.MonthMaterialStockBatchCostService MonthMaterialStockBatchCost { get; set; }
        private ERP.Services.Entities.SimpleSaleService SimpleSale { get; set; }
        private ERP.Services.Entities.SaleService Sale { get; set; }

        private ERP.Services.Entities.MaterialService Material { get; set; }
        private ERP.Services.Entities.CodeItemService CodeItem { get; set; }

        private ERP.Services.Business.Entities.BatchOrdersCostService BatchOrdersCost { get; set; }
        private ERP.Services.Business.BatchOrdersCostService DataBatchOrdersCost { get; set; }

        private ERP.Services.Business.Entities.InvoiceService Invoice { get; set; }

        public MaterialCostService(
            // ERP.Services.Business.Entities.TypeService typeService,
            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Entities.MaterialStockBatchCostService materialStockBatchCostService,
            ERP.Services.Entities.MonthMaterialStockBatchCostService monthMaterialStockBatchCostService,
            ERP.Services.Entities.SimpleSaleService simpleSaleService,
            ERP.Services.Entities.SaleService saleService,
            ERP.Services.Entities.MaterialService materialService,
            ERP.Services.Entities.CodeItemService codeItemService,
            ERP.Services.Business.Entities.BatchOrdersCostService batchOrdersCostService,
            ERP.Services.Business.BatchOrdersCostService dataBatchOrdersCostService,
            ERP.Services.Business.Entities.InvoiceService invoiceService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            // Type = typeService;
            Orders = ordersService;
            MaterialStockBatchCost = materialStockBatchCostService;
            MonthMaterialStockBatchCost = monthMaterialStockBatchCostService;
            SimpleSale = simpleSaleService;
            Sale = saleService;
            Material = materialService;
            CodeItem = codeItemService;
            BatchOrdersCost = batchOrdersCostService;
            DataBatchOrdersCost = dataBatchOrdersCostService;

            Invoice = invoiceService;
        }

        public IQueryable<Models.Views.OrdersMaterialCost> GetOrdersMaterialStockBatchCost(string predicate, string[] filters)
        {
            // 條件組合:
            // 1. table MaterialStockBatchCost, find out column "IOMonth" ,"CostType" 
            // 2. proformace issue: column "IOAmount > 0" ,"IsClose = false" query after data from db is better. 

            var extendPredicate = "";
            if (filters != null && filters.Length > 0)
            {
                //condition
                var tmpFilters = filters.Where(i => i.Contains("IOMonth") || i.Contains("CostType")).ToArray();
                extendPredicate = tmpFilters.Length > 0 ? String.Join(" && ", tmpFilters) : "1=1";
            }

            var resultPredicate = "";
            if (filters != null && filters.Length > 0)
            {
                //condition
                var tmpFilters = filters.Where(i => i.Contains("IOAmount") || i.Contains("IsClose")).ToArray();
                resultPredicate = tmpFilters.Length > 0 ? String.Join(" && ", tmpFilters) : "1=1";
            }


            var useMonth = filters.Where(i => i.Contains("IOMonth")).ToArray();
            var iomonth = useMonth.Length > 0 ? Convert.ToDecimal(useMonth[0].Substring(useMonth[0].Length - 6)) : 0;
            var ioTypes = new List<int?> {1, 2, 3, 5, 6};

            var ordersMaterialCost = (
                from o in Orders.Get()
                join s in SimpleSale.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = s.OrdersId, LocaleId = s.RefLocaleId } into sGrp
                // join s in Sale.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = s.OrdersId, LocaleId = (decimal)s.RefLocaleId } into sGrp
                from s in sGrp.DefaultIfEmpty()
                join m in MaterialStockBatchCost.Get().Where(extendPredicate).Where(i => ioTypes.Contains(i.IOType)).GroupBy(i => new { i.OrderNo }).Select(i => new { OrderNo = i.Key.OrderNo, IOAmount = i.Sum(g => g.IOAmount) }) on new { OrderNo = o.OrderNo } equals new { OrderNo = m.OrderNo } into mGrp
                from m in mGrp.DefaultIfEmpty()
                select new Models.Views.OrdersMaterialCost
                {
                    Id = o.Id,
                    LocaleId = o.LocaleId,
                    CompanyId = o.CompanyId,
                    Company = o.CompanyNo,
                    Brand = o.Brand,
                    StyleNo = o.StyleNo,
                    ShoeName = o.ShoeName,
                    Customer = o.Customer,
                    OrderQty = o.OrderQty,
                    ETD = o.ETD,
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    OrderNo = o.OrderNo,
                    ModifyUserName = o.ModifyUserName,
                    LastUpdateTime = o.LastUpdateTime,
                    ShipmentDate = s.SaleDate,
                    IOAmount = m.IOAmount
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .GroupBy(g => new { g.Id, g.LocaleId, g.CompanyId, g.Company, g.Brand, g.StyleNo, g.ShoeName, g.Customer, g.OrderQty, g.ETD, g.CSD, g.LCSD, g.OrderNo, g.ModifyUserName, g.LastUpdateTime, g.IOAmount })
            .Select(c => new Models.Views.OrdersMaterialCost
            {
                Id = c.Key.Id,
                LocaleId = c.Key.LocaleId,
                CompanyId = c.Key.CompanyId,
                Company = c.Key.Company,
                Brand = c.Key.Brand,
                StyleNo = c.Key.StyleNo,
                ShoeName = c.Key.ShoeName,
                Customer = c.Key.Customer,
                OrderQty = c.Key.OrderQty,
                ETD = c.Key.ETD,
                CSD = c.Key.CSD,
                LCSD = c.Key.LCSD,
                ShipmentDate = c.Max(g => g.ShipmentDate),
                OrderNo = c.Key.OrderNo,
                IOAmount = c.Key.IOAmount,
            })
            .ToList();


            ordersMaterialCost.ForEach(o =>
            {
                o.IsClosed = (o.ShipmentDate == null ||
                              (iomonth > 0 && Convert.ToDecimal(((DateTime)o.ShipmentDate).ToString("yyyyMM")) > iomonth)) ? false : true;
                o.IOMonth = iomonth > 0 ? iomonth : 0;
            });

            var result = ordersMaterialCost.AsQueryable().Where(resultPredicate);
            return result;
        }
        public List<Models.Views.MaterialStockBatchCost> OrdersMaterialStockBatchCostByIOMonth(string orderNo, int month)
        {

            // month == 0 : not use IOMonth condition, return all stockIO
            var ioTypes = new List<int?> {1, 2, 3, 5, 6};
            var useCost = MaterialStockBatchCost.Get()
                    .Where(i => i.OrderNo == orderNo && i.CostType == 1 && ioTypes.Contains(i.IOType))
                    .Select(i => new Models.Views.MaterialStockBatchCost
                    {
                        IOMonth = i.IOMonth,
                        MaterialName = i.MaterialName,
                        PCLUnitNameTw = i.PCLUnitNameTw,
                        IOQty = i.IOQty,
                        IOAmount = i.IOAmount,
                        ModifyUserName = i.ModifyUserName,
                        LastUpdateTime = i.LastUpdateTime,
                        IOType = i.IOType,
                    })
                    .OrderBy(i => i.IOMonth).ThenBy(i => i.MaterialName).ToList();

            return month == 0 ? useCost : useCost.Where(i => i.IOMonth <= month).ToList();
        }

        public IQueryable<Models.Views.OrdersMaterialCost> GetMonthMaterialStockBatchCost(string predicate, string[] filters)
        {
            // 條件組合:
            // 1. table MaterialStockBatchCost, find out column "IOMonth" ,"CostType" 
            // 2. proformace issue: column "IOAmount > 0" ,"IsClose = false" query after data from db is better. 

            var extendPredicate = "";
            if (filters != null && filters.Length > 0)
            {
                //condition
                var tmpFilters = filters.Where(i => i.Contains("IOMonth") || i.Contains("CostType")).ToArray();
                extendPredicate = tmpFilters.Length > 0 ? String.Join(" && ", tmpFilters) : "1=1";
            }

            var resultPredicate = "";
            if (filters != null && filters.Length > 0)
            {
                //condition
                var tmpFilters = filters.Where(i => i.Contains("IOAmount") || i.Contains("IsClose")).ToArray();
                resultPredicate = tmpFilters.Length > 0 ? String.Join(" && ", tmpFilters) : "1=1";
            }


            var useMonth = filters.Where(i => i.Contains("IOMonth")).ToArray();
            var iomonth = useMonth.Length > 0 ? Convert.ToDecimal(useMonth[0].Substring(useMonth[0].Length - 6)) : 0;
            var ioTypes = new List<int?> {1, 2, 3, 5, 6};

            var ordersMaterialCost = (
                from o in Orders.Get()
                join s in SimpleSale.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = s.OrdersId, LocaleId = s.RefLocaleId } into sGrp
                // join s in Sale.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = s.OrdersId, LocaleId = (decimal)s.RefLocaleId } into sGrp
                from s in sGrp.DefaultIfEmpty()
                join m in MonthMaterialStockBatchCost.Get().Where(extendPredicate).Where(i => ioTypes.Contains(i.IOType)).GroupBy(i => new { i.OrderNo }).Select(i => new { OrderNo = i.Key.OrderNo, IOAmount = i.Sum(g => g.IOAmount) }) on new { OrderNo = o.OrderNo } equals new { OrderNo = m.OrderNo } into mGrp
                from m in mGrp.DefaultIfEmpty()
                select new Models.Views.OrdersMaterialCost
                {
                    Id = o.Id,
                    LocaleId = o.LocaleId,
                    CompanyId = o.CompanyId,
                    Company = o.CompanyNo,
                    Brand = o.Brand,
                    StyleNo = o.StyleNo,
                    ShoeName = o.ShoeName,
                    Customer = o.Customer,
                    OrderQty = o.OrderQty,
                    ETD = o.ETD,
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    OrderNo = o.OrderNo,
                    ModifyUserName = o.ModifyUserName,
                    LastUpdateTime = o.LastUpdateTime,
                    ShipmentDate = s.SaleDate,
                    IOAmount = m.IOAmount
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .GroupBy(g => new { g.Id, g.LocaleId, g.CompanyId, g.Company, g.Brand, g.StyleNo, g.ShoeName, g.Customer, g.OrderQty, g.ETD, g.CSD, g.LCSD, g.OrderNo, g.ModifyUserName, g.LastUpdateTime, g.IOAmount })
            .Select(c => new Models.Views.OrdersMaterialCost
            {
                Id = c.Key.Id,
                LocaleId = c.Key.LocaleId,
                CompanyId = c.Key.CompanyId,
                Company = c.Key.Company,
                Brand = c.Key.Brand,
                StyleNo = c.Key.StyleNo,
                ShoeName = c.Key.ShoeName,
                Customer = c.Key.Customer,
                OrderQty = c.Key.OrderQty,
                ETD = c.Key.ETD,
                CSD = c.Key.CSD,
                LCSD = c.Key.LCSD,
                ShipmentDate = c.Max(g => g.ShipmentDate),
                OrderNo = c.Key.OrderNo,
                IOAmount = c.Key.IOAmount,
            })
            .ToList();


            ordersMaterialCost.ForEach(o =>
            {
                o.IsClosed = (o.ShipmentDate == null ||
                              (iomonth > 0 && Convert.ToDecimal(((DateTime)o.ShipmentDate).ToString("yyyyMM")) > iomonth)) ? false : true;
                o.IOMonth = iomonth > 0 ? iomonth : 0;
            });

            var result = ordersMaterialCost.AsQueryable().Where(resultPredicate);
            return result;
        }
        public List<Models.Views.MonthMaterialStockBatchCost> MonthMaterialStockBatchCostByIOMonth(string orderNo, int month)
        {

            // month == 0 : not use IOMonth condition, return all stockIO
            var ioTypes = new List<int?> {1, 2, 3, 5, 6};
            var useCost = MonthMaterialStockBatchCost.Get()
                    .Where(i => i.OrderNo == orderNo && i.CostType == 1 && ioTypes.Contains(i.IOType))
                    .Select(i => new Models.Views.MonthMaterialStockBatchCost
                    {
                        IOMonth = i.IOMonth,
                        MaterialName = i.MaterialName,
                        PCLUnitNameTw = i.PCLUnitNameTw,
                        IOQty = i.IOQty,
                        IOAmount = i.IOAmount,
                        ModifyUserName = i.ModifyUserName,
                        LastUpdateTime = i.LastUpdateTime,
                        IOType = i.IOType,
                        PurUnitPrice = i.PurUnitPrice,
                        ExchangeRate = i.ExchangeRate,
                        PurDollarCodeId = i.PurDollarCodeId,
                        PurDollarNameTw = i.PurDollarNameTw,
                        PurDollarNameEn = i.PurDollarNameEn
                    })
                    .OrderBy(i => i.IOMonth).ThenBy(i => i.MaterialName).ToList();

            return month == 0 ? useCost : useCost.Where(i => i.IOMonth <= month).ToList();
        }

        public IQueryable<Models.Views.BatchOrdersCost> GetBatchOrdersCost(string predicate)
        {
            return BatchOrdersCost.Get(predicate);
        }
        public ERP.Models.Views.BatchOrdersCostGroup GetBatchOrdersCostGroup(string orderNo, int localeId)
        {
            return DataBatchOrdersCost.GetBatchOrdersCostGroup(orderNo, localeId);
        }
        public IQueryable<Models.Views.OrdersShipmentCost> GetOrdersShipmentCost(string predicate)
        {
            // get orders shipment
            var ordersShipment = Invoice.GetOrderShipment(predicate)
                .Select(i => new Models.Views.OrdersShipmentCost
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    OrderNo = i.OrderNo,
                    CompanyId = i.CompanyId,
                    Company = i.Company,
                    Brand = i.Brand,
                    ArticleNo = i.ArticleNo,
                    StyleNo = i.StyleNo,
                    ShoeName = i.ShoeName,

                    OrderQty = i.OrderQty,
                    CSD = i.CSD,
                    LCSD = i.LCSD,
                    OPD = i.OWD,

                    Season = i.Season,
                    Last = i.Last,
                    Outsole = i.Outsole,

                    AvgPrice = i.AvgPrice,
                    ShippingQty = i.ShippingQty,
                    ShortageQty = i.ShortageQty,
                    ShippingAmount = i.ShippingAmount,

                    LessCharge = i.LessCharge,
                    OtherCharge = i.OtherCharge,
                    FeedbackFund = i.FeedbackFund,
                    OtherCost = i.OtherCost,

                    CLB = i.CLB,
                    ShippingDate = i.ShippingDate,
                    ShippingMonth = i.ShippingMonth,

                    ARSubTotal = i.ARSubTotal,
                    ARTotal = i.ARTotal,
                    InvoiceNo = i.InvoiceNo,
                    Currency = i.Currency,

                    OutsolePrice = i.OutsolePrice,
                    MidsolePrice = i.MidsolePrice,
                    ToolingOtherPrice = i.ToolingOtherPrice,
                    ToolingTotalPrice = i.ToolingTotalPrice,
                }).ToList();
            var styeNos = ordersShipment.Select(i => i.StyleNo).Distinct();
            var articleNos = ordersShipment.Select(i => i.ArticleNo).Distinct();

            // get cost by order's style
            var styleCosts = BatchOrdersCost.Get()
                .Where(i => styeNos.Contains(i.StyleNo) && i.Status == 1).ToList();

            var articleCosts = BatchOrdersCost.Get()
                .Where(i => articleNos.Contains(i.ArticleNo) && i.Status == 1).ToList();

            // combind 
            var ordersCostRate = (
                from o in ordersShipment.AsQueryable()
                join s in styleCosts.AsQueryable() on new { o.OrderNo } equals new { s.OrderNo } into sGrp
                from s in sGrp.DefaultIfEmpty()
                select new Models.Views.OrdersShipmentCost
                {
                    Id = o.Id,
                    LocaleId = o.LocaleId,
                    CompanyId = o.CompanyId,
                    Company = o.Company,
                    ShippingDate = o.ShippingDate,
                    OrderNo = o.OrderNo,
                    StyleNo = o.StyleNo,
                    ShoeName = o.ShoeName,
                    OrderQty = o.OrderQty,
                    ShippingQty = o.ShippingQty,
                    AvgPrice = o.AvgPrice,
                    ShippingAmount = o.ShippingAmount,

                    FeedbackFund = o.FeedbackFund,
                    OtherCharge = o.OtherCharge,
                    CLB = o.CLB,
                    LessCharge = o.LessCharge,
                    OtherCost = o.OtherCost,

                    ARSubTotal = o.ARSubTotal,

                    SMCostRate = (s != null && s.SMCostRate != null) ? (decimal)s.SMCostRate : 0,
                    PMCostRate = (s != null && s.PMCostRate != null) ? (decimal)s.PMCostRate : 0,
                    SMCost = (s != null && s.SMCostRate != null) ? (decimal)(o.ShippingAmount * s.SMCostRate) : 0,
                    PMCost = (s != null && s.PMCostRate != null) ? (decimal)(o.ShippingAmount * s.PMCostRate) : 0,
                    CostDate = s != null ? s.CostDate : (DateTime?)null,
                    RefOrderNo = s != null ? s.OrderNo : "",
                    RefStyle = s != null ? s.StyleNo : "",

                    Brand = o.Brand,
                    ArticleNo = o.ArticleNo,
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    OPD = o.OWD,
                    Season = o.Season,
                    Last = o.Last,
                    Outsole = o.Outsole,
                    ShortageQty = o.ShortageQty,
                    ShippingMonth = o.ShippingMonth,
                    ARTotal = o.ARTotal,
                    InvoiceNo = o.InvoiceNo,
                    Currency = o.Currency,
                    
                    OutsolePrice = o.OutsolePrice,
                    MidsolePrice = o.MidsolePrice,
                    ToolingOtherPrice = o.ToolingOtherPrice,
                    ToolingTotalPrice = o.ToolingTotalPrice,
                }).ToList();

            ordersCostRate.ForEach(i =>
            {
                if (i.PMCostRate == null || i.PMCostRate == 0)
                {

                    var styleCost = styleCosts
                        .Where(s => s.Company == i.Company && s.StyleNo == i.StyleNo && s.ShipmentCurrency == i.Currency)
                        .OrderByDescending(s => s.CostDate)
                        .FirstOrDefault();

                    if (styleCost != null)
                    {
                        i.SMCostRate = (styleCost.SMCostRate != null) ? (decimal)styleCost.SMCostRate : 0;
                        i.PMCostRate = (styleCost.PMCostRate != null) ? (decimal)styleCost.PMCostRate : 0;
                        i.SMCost = (styleCost.SMCostRate != null) ? (decimal)(i.ShippingAmount * styleCost.SMCostRate) : 0;
                        i.PMCost = (styleCost.PMCostRate != null) ? (decimal)(i.ShippingAmount * styleCost.PMCostRate) : 0;
                        i.CostDate = styleCost.CostDate;
                        i.RefOrderNo = styleCost.OrderNo;
                        i.RefStyle = styleCost.StyleNo;
                    }
                    else
                    {
                        var articleCost = articleCosts
                            .Where(a => a.Company == i.Company && a.ArticleNo == i.ArticleNo && a.ShipmentCurrency == i.Currency)
                            .OrderByDescending(a => a.CostDate)
                            .FirstOrDefault();

                        if (articleCost != null)
                        {
                            i.SMCostRate = (articleCost.SMCostRate != null) ? (decimal)articleCost.SMCostRate : 0;
                            i.PMCostRate = (articleCost.PMCostRate != null) ? (decimal)articleCost.PMCostRate : 0;
                            i.SMCost = (articleCost.SMCostRate != null) ? (decimal)(i.ShippingAmount * articleCost.SMCostRate) : 0;
                            i.PMCost = (articleCost.PMCostRate != null) ? (decimal)(i.ShippingAmount * articleCost.PMCostRate) : 0;
                            i.CostDate = articleCost.CostDate;
                            i.RefOrderNo = articleCost.OrderNo;
                            i.RefStyle = articleCost.StyleNo;
                        }
                    }

                }
            });
            return ordersCostRate.AsQueryable();
        }
    }
}