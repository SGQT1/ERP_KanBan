using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Diamond.DataSource.Extensions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class OrdersStockService : BusinessService
    {
        private Services.Entities.OrdersService Orders { get; }
        private Services.Entities.CTNLabelService CTNLabel { get; }
        private Services.Entities.CTNLabelItemService CTNLabelItem { get; }
        private Services.Entities.CTNLabelStockInService CTNLabelStockIn { get; }
        private Services.Entities.CTNLabelStockOutService CTNLabelStockOut { get; }

        private Services.Entities.CompanyService Company { get; }
        private Services.Entities.CodeItemService CodeItem { get; }

        private Services.Business.Entities.OrdersStockItemService OrdersStockItem { get; set; }

        // private Services.Views.OrdersStockService OrdersStock {get;}
        public OrdersStockService(
            Services.Entities.OrdersService ordersService,
            Services.Entities.CTNLabelService ctnLabelService,
            Services.Entities.CTNLabelItemService ctnLabelItemService,
            Services.Entities.CTNLabelStockInService ctnLabelStockInService,
            Services.Entities.CTNLabelStockOutService ctnLabelStockOutService,

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

            Company = companyService;
            CodeItem = codeItemService;

            OrdersStockItem = ordersStockItemService;
        }

        public IQueryable<Models.Views.OrdersStockItem> Get(string predicate)
        {
            var items = OrdersStockItem.Get()
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .ToList();
                items.ForEach(si =>
                {
                    si.StockInCTNS = items.Where(i => i.OrderId == si.OrderId && i.StockInTime != null).Count();
                });
            return items.AsQueryable();
        }

        public List<Models.Views.OrdersStock> GetGroup(string predicate)
        {

            var items = OrdersStockItem.Get()
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Select(i => new
                {
                    LocaleId = i.LocaleId,
                    OrderId = i.OrderId,
                    CompanyId = i.CompanyId,
                    Company = i.Company,
                    BrandCodeId = i.BrandCodeId,
                    BrandCode = i.BrandCode,
                    OrderNo = i.OrderNo,
                    CSD = i.CSD,
                    LCSD = i.LCSD,
                    StyleNo = i.StyleNo,
                    ShoeName = i.ShoeName,
                    Customer = i.Customer,
                    OrderQty = i.OrderQty,
                    PackingQty = i.PackingQty,
                    CartonCount = i.CartonCount,
                    CTNLabelId = i.CTNLabelId,
                    StockInTime = i.StockInTime,
                    StockOutTime = i.StockOutTime,
                    StockInCTNS = i.StockInTime == null ? 0 : 1,
                    StockOutCTNS = i.StockOutTime == null ? 0 : 1,
                    StockBalanceCTNS = (i.StockInTime != null && i.StockOutTime == null) ? 1 : 0,
                    StockBalanceQty = (i.StockInTime != null && i.StockOutTime == null) ? i.SubPackingQty : 0,
                })
                .GroupBy(i => new { i.LocaleId, i.OrderId, i.CompanyId, i.Company, i.BrandCodeId, i.BrandCode, i.OrderNo, i.CSD, i.LCSD, i.StyleNo, i.ShoeName, i.Customer, i.OrderQty, i.PackingQty, i.CartonCount, i.CTNLabelId })
                .Select(i => new Models.Views.OrdersStock
                {
                    LocaleId = i.Key.LocaleId,
                    OrderId = i.Key.OrderId,
                    CompanyId = i.Key.CompanyId,
                    Company = i.Key.Company,
                    BrandCodeId = i.Key.BrandCodeId,
                    BrandCode = i.Key.BrandCode,
                    OrderNo = i.Key.OrderNo,
                    CSD = i.Key.CSD,
                    LCSD = i.Key.LCSD,
                    StyleNo = i.Key.StyleNo,
                    ShoeName = i.Key.ShoeName,
                    Customer = i.Key.Customer,
                    OrderQty = i.Key.OrderQty,
                    PackingQty = i.Key.PackingQty,
                    CartonCount = i.Key.CartonCount,
                    StockInCTNS = i.Sum(t => t.StockInCTNS),
                    StockOutCTNS = i.Sum(t => t.StockOutCTNS),
                })
                .ToList();
            return items;
        }

        public IQueryable<Models.Views.OrdersStock> GetSummary(string predicate)
        {
            var items = OrdersStockItem.Get()
                .Select(i => new
                {
                    LocaleId = i.LocaleId,
                    OrderId = i.OrderId,
                    CompanyId = i.CompanyId,
                    Company = i.Company,
                    BrandCodeId = i.BrandCodeId,
                    BrandCode = i.BrandCode,
                    OrderNo = i.OrderNo,
                    CSD = i.CSD,
                    LCSD = i.LCSD,
                    StyleNo = i.StyleNo,
                    ShoeName = i.ShoeName,
                    Customer = i.Customer,
                    OrderQty = i.OrderQty,
                    PackingQty = i.PackingQty,
                    CartonCount = i.CartonCount,
                    CTNLabelId = i.CTNLabelId,
                    StockInTime = i.StockInTime,
                    StockOutTime = i.StockOutTime,
                    StockInCTNS = i.StockInTime == null ? 0 : 1,
                    StockOutCTNS = i.StockOutTime == null ? 0 : 1,
                    StockBalanceCTNS = (i.StockInTime != null && i.StockOutTime == null) ? 1 : 0,
                    StockBalanceQty = (i.StockInTime != null && i.StockOutTime == null) ? i.SubPackingQty : 0,
                })
                .GroupBy(i => new { i.LocaleId, i.OrderId, i.CompanyId, i.Company, i.BrandCodeId, i.BrandCode, i.OrderNo, i.CSD, i.LCSD, i.StyleNo, i.ShoeName, i.Customer, i.OrderQty, i.PackingQty, i.CartonCount, i.CTNLabelId })
                .Select(i => new Models.Views.OrdersStock
                {
                    LocaleId = i.Key.LocaleId,
                    OrderId = i.Key.OrderId,
                    CompanyId = i.Key.CompanyId,
                    Company = i.Key.Company,
                    BrandCodeId = i.Key.BrandCodeId,
                    BrandCode = i.Key.BrandCode,
                    OrderNo = i.Key.OrderNo,
                    CSD = i.Key.CSD,
                    LCSD = i.Key.LCSD,
                    StyleNo = i.Key.StyleNo,
                    ShoeName = i.Key.ShoeName,
                    Customer = i.Key.Customer,
                    OrderQty = i.Key.OrderQty,
                    PackingQty = i.Key.PackingQty,
                    CartonCount = i.Key.CartonCount,
                    StockInCTNS = i.Sum(t => t.StockInCTNS),
                    StockOutCTNS = i.Sum(t => t.StockOutCTNS),
                    StockBalanceCTNS = i.Sum(t => t.StockBalanceCTNS),
                    StockBalanceQty = i.Sum(t => t.StockBalanceQty),
                    FirstStockInTime = i.Min(t => t.StockInTime),
                    LastStockInTime = i.Max(t => t.StockInTime),
                    FirstStockOutTime = i.Min(t => t.StockOutTime),
                    LastStockOutTime = i.Max(t => t.StockOutTime),
                })
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .ToList();
            items.ForEach(i => {
                i.DiffDays = i.LastStockInTime == null ? -1 : ((DateTime)i.LCSD).Subtract((DateTime)i.LastStockInTime).Days;
            });
            return items.AsQueryable();

        }

        public List<Models.Views.OrdersStockTaking> GetStockTakingsByOrder(string predicate)
        {
            var items = (
                from si in CTNLabelStockIn.Get()
                join cli in CTNLabelItem.Get() on new { LocaleId = si.LocaleId, LabelCode = si.LabelCode } equals new { LocaleId = cli.LocaleId, LabelCode = cli.LabelCode }
                join h in CTNLabel.Get() on new { CTNLabelId = cli.CTNLabelId, LocaleId = cli.LocaleId } equals new { CTNLabelId = h.Id, LocaleId = h.LocaleId }
                join so in CTNLabelStockOut.Get() on new { LocaleId = cli.LocaleId, LabelCode = cli.LabelCode } equals new { LocaleId = so.LocaleId, LabelCode = so.LabelCode } into soGrp
                from so in soGrp.DefaultIfEmpty()
                join o in Orders.Get() on new { OrderNo = h.OrderNo } equals new { OrderNo = o.OrderNo }
                select new
                {
                    LocaleId = si.LocaleId,
                    StyleNo = h.StyleNo,
                    OrderNo = h.OrderNo,
                    StockInTime = si.StockInAdjTime,
                    StockOutTime = so.StockOutAdjTime,
                    PackingQty = cli.SubPackingQty,
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    Qty = o.OrderQty,
                    ETD = h.ExFactoryDate
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .GroupBy(i => new { i.StyleNo, i.OrderNo, i.LocaleId, i.CSD, i.LCSD, i.Qty, i.ETD })
            .Select(i => new Models.Views.OrdersStockTaking
            {
                LocaleId = i.Key.LocaleId,
                StyleNo = i.Key.StyleNo,
                OrderNo = i.Key.OrderNo,
                StockQty = i.Sum(t => t.PackingQty),
                CartonCount = i.Count(),
                CSD = i.Key.CSD,
                LCSD = i.Key.LCSD,
                OrderQty = i.Key.Qty,
                ETD = i.Key.ETD
            })
            .OrderBy(i => i.OrderNo)
            .ToList();
            return items;

        }
        public List<Models.Views.OrdersStockTaking> GetStockTakingsByStyle(string predicate)
        {
            var items = (
                from si in CTNLabelStockIn.Get()
                join cli in CTNLabelItem.Get() on new { LocaleId = si.LocaleId, LabelCode = si.LabelCode } equals new { LocaleId = cli.LocaleId, LabelCode = cli.LabelCode }
                join h in CTNLabel.Get() on new { CTNLabelId = cli.CTNLabelId, LocaleId = cli.LocaleId } equals new { CTNLabelId = h.Id, LocaleId = h.LocaleId }
                join so in CTNLabelStockOut.Get() on new { LocaleId = cli.LocaleId, LabelCode = cli.LabelCode } equals new { LocaleId = so.LocaleId, LabelCode = so.LabelCode } into soGrp
                from so in soGrp.DefaultIfEmpty()
                join o in Orders.Get() on new { OrderNo = h.OrderNo } equals new { OrderNo = o.OrderNo }
                select new
                {
                    LocaleId = si.LocaleId,
                    StyleNo = h.StyleNo,
                    OrderNo = h.OrderNo,
                    StockInTime = si.StockInAdjTime,
                    StockOutTime = so.StockOutAdjTime,
                    PackingQty = cli.SubPackingQty,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .GroupBy(i => new { i.StyleNo, i.LocaleId })
            .Select(i => new Models.Views.OrdersStockTaking
            {
                LocaleId = i.Key.LocaleId,
                StyleNo = i.Key.StyleNo,
                StockQty = i.Sum(t => t.PackingQty),
                CartonCount = i.Count()
            })
            .OrderBy(i => i.StyleNo)
            .ToList();
            return items;
        }

    }
}