using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class OrdersStockItemService : BusinessService
    {
        private Services.Entities.OrdersService Orders { get; }
        private Services.Entities.CTNLabelService CTNLabel { get; }
        private Services.Entities.CTNLabelItemService CTNLabelItem { get; }
        private Services.Entities.CTNLabelStockInService CTNLabelStockIn { get; }
        private Services.Entities.CTNLabelStockOutService CTNLabelStockOut { get; }

        private Services.Entities.CompanyService Company { get; }
        private Services.Entities.CodeItemService CodeItem { get; }
        public OrdersStockItemService (
            Services.Entities.OrdersService ordersService,
            Services.Entities.CTNLabelService ctnLabelService,
            Services.Entities.CTNLabelItemService ctnLabelItemService,
            Services.Entities.CTNLabelStockInService ctnLabelStockInService,
            Services.Entities.CTNLabelStockOutService ctnLabelStockOutService,

            Services.Entities.CompanyService companyService,
            Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            Orders = ordersService;
            CTNLabel = ctnLabelService;
            CTNLabelItem = ctnLabelItemService;
            CTNLabelStockIn = ctnLabelStockInService;
            CTNLabelStockOut = ctnLabelStockOutService;

            Company = companyService;
            CodeItem = codeItemService;
        }

        public IQueryable<Models.Views.OrdersStockItem> Get()
        {
            var items = (
                from o in Orders.Get()
                join h in CTNLabel.Get() on new { OrderNo = o.OrderNo } equals new { OrderNo = h.OrderNo}
                join cli in CTNLabelItem.Get() on new { CTNLabelId = h.Id, LocaleId = h.LocaleId } equals new { CTNLabelId = cli.CTNLabelId, LocaleId = cli.LocaleId }
                join si in CTNLabelStockIn.Get() on new { LocaleId = cli.LocaleId, LabelCode = cli.LabelCode } equals new { LocaleId = si.LocaleId, LabelCode = si.LabelCode } into siGrp
                from si in siGrp.DefaultIfEmpty()
                join so in CTNLabelStockOut.Get() on new { LocaleId = cli.LocaleId, LabelCode = cli.LabelCode } equals new { LocaleId = so.LocaleId, LabelCode = so.LabelCode } into soGrp
                from so in soGrp.DefaultIfEmpty()
                select new Models.Views.OrdersStockItem
                {
                    Id = cli.Id,
                    LocaleId = cli.LocaleId,
                    OrderId = o.Id,
                    CTNLabelId = cli.CTNLabelId,
                    CompanyId = o.CompanyId,
                    Company = o.CompanyNo,
                    BrandCodeId = (decimal)o.BrandCodeId,
                    BrandCode = o.Brand,
                    OrderNo = o.OrderNo,
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    StyleNo = o.StyleNo,
                    ShoeName = o.ShoeName,
                    Customer = h.Customer,
                    SeqNo = cli.SeqNo,
                    CartonCount = h.CTNS,
                    LabelCode = cli.LabelCode,
                    MinOrderSize = cli.MinRefDisplaySize,
                    MaxOrderSize = cli.MaxRefDisplaySize,
                    OrderQty = o.OrderQty,
                    PackingQty = h.PackingQty,
                    SubPackingQty = cli.SubPackingQty,
                    SubLabelCode = cli.SubLabelCode,

                    SubNetWeight = cli.SubNetWeight,
                    SubGrossWeight = cli.SubGrossWeight,
                    SubMEAS = cli.SubMEAS,
                    SubCBM = cli.SubCBM,
                    SubGrossUpperWeight = (double)si.SubGrossWeight * 0.98,
                    SubGrossLowerWeight = (double)si.SubGrossWeight * 1.02,
                    StockInGrossWeight = si.SubGrossWeight,
                    StockInTime = si.StockInTime,
                    StockOutGrossWeight = so.SubGrossWeight,
                    StockOutTime = so.StockOutTime,

                    GBSPOReferenceNo = o.GBSPOReferenceNo,
                    CustomerOrderNo = o.CustomerOrderNo,
                }
            );
            return items;
        }

        public void StockInRemoveRange(string[] items, decimal localeId) {
            CTNLabelStockIn.RemoveRange(i => items.Contains(i.LabelCode) && i.LocaleId == localeId);
        }

        public void StockInCreateRange(IEnumerable<Models.Views.OrdersStockItem> items)
        {
            CTNLabelStockIn.CreateRange(StockInBuildRange(items));
        }
        private IEnumerable<Models.Entities.CTNLabelStockIn> StockInBuildRange(IEnumerable<Models.Views.OrdersStockItem> items)
        {
            return items.Select(item => new Models.Entities.CTNLabelStockIn
            {
                LocaleId = item.LocaleId,
                LabelCode = item.LabelCode,
                SubGrossWeight = item.StockInGrossWeight,
                StockInTime = (DateTime)item.StockInTime,
                StockInAdjTime = item.StockInTime
            });
        }



        public void StockOutRemoveRange(string[] items, decimal localeId) {
            CTNLabelStockOut.RemoveRange(i => items.Contains(i.LabelCode) && i.LocaleId == localeId);
        }
        public void StockOutCreateRange(IEnumerable<Models.Views.OrdersStockItem> items)
        {
            CTNLabelStockOut.CreateRange(StockOutBuildRange(items));
        }
        private IEnumerable<Models.Entities.CTNLabelStockout> StockOutBuildRange(IEnumerable<Models.Views.OrdersStockItem> items)
        {
            return items.Select(item => new Models.Entities.CTNLabelStockout
            {
                LocaleId = item.LocaleId,
                LabelCode = item.LabelCode,
                SubGrossWeight = item.StockOutGrossWeight,
                StockOutTime = (DateTime)item.StockOutTime,
                StockOutAdjTime = item.StockOutTime
            });
        }

    }
}