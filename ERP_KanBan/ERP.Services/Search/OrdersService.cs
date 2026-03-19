using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ERP.Services.Search
{
    public class OrdersService : SearchService
    {
        private ERP.Services.Business.Entities.TypeService Type { get; set; }
        private ERP.Services.Business.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Business.Entities.OrdersItemService OrdersItem { get; set; }
        private ERP.Services.Entities.QuotationService Quotation { get; set; }
        private ERP.Services.Business.Entities.ArticleSizeRunService ArticleSizeRun { get; set; }

        public OrdersService(
            ERP.Services.Business.Entities.TypeService typeService,
            ERP.Services.Business.Entities.OrdersService ordersService,
            ERP.Services.Business.Entities.OrdersItemService ordersItemService,
            ERP.Services.Entities.QuotationService quotationService,
            ERP.Services.Business.Entities.ArticleSizeRunService articleSizeRunService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Type = typeService;
            Orders = ordersService;
            OrdersItem = ordersItemService;
            Quotation = quotationService;
            ArticleSizeRun = articleSizeRunService;
        }

        public IQueryable<Models.Views.Orders> GetOrders(string predicate)
        {
            var result = Orders.GetEntity().Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate).ToList();

            return result.AsQueryable();
        }

        // get sizerun by ordersId
        public List<ERP.Models.Views.OrdersItem> GetOrdersItem(int id, int localeId)
        {
            // return OrdersItem.Get().Where(i => i.OrdersId == id & i.LocaleId == localeId).ToList();
            return OrdersItem.GetSizeMapping(id, localeId).ToList();
        }
        /*
         * get IQueryable Orders of predicate,sort,take,
         * get orders join ordersItem to list,
         * get Orders to list,
         * set Sizerun foreach orders
         */
        public IQueryable<Models.Views.OrdersSizeRun> GetOrdersSizeRun(string predicate)
        {
            var sizeruns = new List<Models.Views.OrdersSizeRun>();

            var ordersItems = (
                from o in Orders.Get()
                join oi in OrdersItem.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = (decimal)oi.OrdersId, LocaleId = oi.LocaleId }
                join asr in ArticleSizeRun.Get() on new { ArticleId = o.ArticleId, LocaleId = o.LocaleId, ArticleInnerSize = oi.ArticleInnerSize } equals new { ArticleId = asr.ArticleId, LocaleId = asr.LocaleId, ArticleInnerSize = (decimal)asr.ArticleInnerSize }
                select new
                {
                    OrdersId = o.Id,
                    LocaleId = o.LocaleId,
                    Customer = o.Customer,
                    CustomerId = o.CustomerId,
                    CustomerOrderNo = o.CustomerOrderNo,
                    GBSPOReferenceNo = o.GBSPOReferenceNo,
                    CompanyId = o.CompanyId,
                    CompanyNo = o.CompanyNo,
                    ProductType = o.ProductType,
                    OrderQty = o.OrderQty,
                    OrderNo = o.OrderNo,
                    OrderDate = o.OrderDate,
                    OWD = o.OWD,
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    BrandCodeId = o.BrandCodeId,
                    RefBrand = o.Brand, //之後再刪除
                    Brand = o.Brand,
                    ArticleId = o.ArticleId,
                    ArticleNo = o.ArticleNo,
                    StyleNo = o.StyleNo,
                    ShoeName = o.ShoeName,
                    ArticleSizeCountry = o.ArticleSizeCountryCode,
                    OrderSizeCountry = o.OrderSizeCountryCode,
                    OutsoleNo = o.OutsoleNo,
                    LastNo = o.LastNo,

                    ArticleSize = oi.ArticleSize,
                    ArticleSizeSuffix = oi.ArticleSizeSuffix,
                    ArticleInnerSize = oi.ArticleInnerSize,
                    ArticleDisplay = asr.ArticleDisplaySize,
                    LastSize = asr.LastSize,
                    LastInnerSize = asr.LastInnerSize,
                    LastSizeSuffix = asr.LastSizeSuffix,
                    LastDisplaySize = asr.LastDisplaySize,
                    Qty = oi.Qty,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            var ordersHeads = ordersItems.Select(o => new
            {
                OrdersId = o.OrdersId,
                LocaleId = o.LocaleId,
                Customer = o.Customer,
                CustomerId = o.CustomerId,
                CustomerOrderNo = o.CustomerOrderNo,
                GBSPOReferenceNo = o.GBSPOReferenceNo,
                CompanyId = o.CompanyId,
                CompanyNo = o.CompanyNo,
                ProductType = o.ProductType,
                OrderQty = o.OrderQty,
                OrderNo = o.OrderNo,
                OrderDate = o.OrderDate,
                OWD = o.OWD,
                CSD = o.CSD,
                LCSD = o.LCSD,
                BrandCodeId = o.BrandCodeId,
                RefBrand = o.RefBrand,
                ArticleId = o.ArticleId,
                ArticleNo = o.ArticleNo,
                StyleNo = o.StyleNo,
                ShoeName = o.ShoeName,
                ArticleSizeCountry = o.ArticleSizeCountry,
                OrderSizeCountry = o.OrderSizeCountry,
                OutsoleNo = o.OutsoleNo,
                LastNo = o.LastNo,
            })
            .Distinct()
            .ToList();

            var lastHead = new List<string>();
            var articleHead = new List<string>();
            ordersHeads.ForEach(i =>
            {
                var s = new Models.Views.OrdersSizeRun();
                var mes = new List<string>();
                var articleSizeRun = new List<string>();
                var lastSizeRun = new List<string>();

                s.OrdersId = i.OrdersId;
                s.LocaleId = i.LocaleId;
                s.CompanyId = i.CompanyId;
                s.CompanyNo = i.CompanyNo;
                s.Customer = i.Customer;
                s.CustomerOrderNo = i.CustomerOrderNo;
                s.GBSPOReferenceNo = i.GBSPOReferenceNo;
                s.ProductType = i.ProductType;
                s.OrderQty = i.OrderQty;
                s.OrderNo = i.OrderNo;
                s.OWD = i.OWD;
                s.OrderDate = i.OrderDate;
                s.CSD = i.CSD;
                s.LCSD = i.LCSD;
                s.BrandCodeId = i.BrandCodeId;
                s.Brand = i.RefBrand;
                s.ArticleNo = i.ArticleNo;
                s.StyleNo = i.StyleNo;
                s.ShoeName = i.ShoeName;
                s.OrderSizeCountry = i.OrderSizeCountry;
                s.ArticleSizeCountry = i.ArticleSizeCountry;
                s.OutsoleNo = i.OutsoleNo;
                s.LastNo = i.LastNo;

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

                    mes.Add("'" + oi.ArticleSize + "':" + oi.Qty);
                    
                    articleSizeRun.Add("\"" + field + "\":" + oi.Qty);
                    articleHead.Add("\"" + field + "\":\"" + oi.ArticleDisplay + "\"");

                    lastSizeRun.Add("\"" + field + "\":" + oi.Qty);
                    lastHead.Add("\"" + field + "\":\"" + oi.LastDisplaySize + "\"");
                });

                s.MESFormat = "{" + string.Join(",", mes) + "}";
                s.LastSizeRun = "{" + string.Join(",", lastSizeRun) + "}";
                s.ArticleSizeRun = "{" + string.Join(",", articleSizeRun) + "}";
                sizeruns.Add(s);
            });

            sizeruns.ForEach(i =>
            {
                i.LastHead = "{" + string.Join(",", lastHead.Distinct().OrderBy(c => c)) + "}";
                i.ArticleHead = "{" + string.Join(",", articleHead.Distinct().OrderBy(c => c)) + "}";
            });

            return sizeruns.AsQueryable();
        }
        public IQueryable<Models.Views.OrdersSizeRun> GetOrdersSizeRun1(string predicate)
        {
            var sizeruns = new List<Models.Views.OrdersSizeRun>();

            var ordersItems = (
                from o in Orders.Get()
                join oi in OrdersItem.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = (decimal)oi.OrdersId, LocaleId = oi.LocaleId }
                join asr in ArticleSizeRun.Get() on new { ArticleId = o.ArticleId, LocaleId = o.LocaleId, ArticleInnerSize = oi.ArticleInnerSize } equals new { ArticleId = asr.ArticleId, LocaleId = asr.LocaleId, ArticleInnerSize = (decimal)asr.ArticleInnerSize }
                select new
                {
                    OrdersId = o.Id,
                    LocaleId = o.LocaleId,
                    Customer = o.Customer,
                    CustomerId = o.CustomerId,
                    CustomerOrderNo = o.CustomerOrderNo,
                    GBSPOReferenceNo = o.GBSPOReferenceNo,
                    CompanyId = o.CompanyId,
                    CompanyNo = o.CompanyNo,
                    ProductType = o.ProductType,
                    OrderQty = o.OrderQty,
                    OrderNo = o.OrderNo,
                    OrderDate = o.OrderDate,
                    OWD = o.OWD,
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    BrandCodeId = o.BrandCodeId,
                    RefBrand = o.Brand, //之後再刪除
                    Brand = o.Brand,
                    ArticleId = o.ArticleId,
                    ArticleNo = o.ArticleNo,
                    StyleNo = o.StyleNo,
                    ShoeName = o.ShoeName,
                    ArticleSizeCountry = o.ArticleSizeCountryCode,
                    OrderSizeCountry = o.OrderSizeCountryCode,
                    OutsoleNo = o.OutsoleNo,
                    LastNo = o.LastNo,

                    ArticleSize = oi.ArticleSize,
                    ArticleSizeSuffix = oi.ArticleSizeSuffix,
                    ArticleInnerSize = oi.ArticleInnerSize,
                    ArticleDisplay = asr.ArticleDisplaySize,
                    LastSize = asr.LastSize,
                    LastInnerSize = asr.LastInnerSize,
                    LastSizeSuffix = asr.LastSizeSuffix,
                    LastDisplaySize = asr.LastDisplaySize,
                    Qty = oi.Qty,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            var ordersHeads = ordersItems.Select(o => new
            {
                OrdersId = o.OrdersId,
                LocaleId = o.LocaleId,
                Customer = o.Customer,
                CustomerId = o.CustomerId,
                CustomerOrderNo = o.CustomerOrderNo,
                GBSPOReferenceNo = o.GBSPOReferenceNo,
                CompanyId = o.CompanyId,
                CompanyNo = o.CompanyNo,
                ProductType = o.ProductType,
                OrderQty = o.OrderQty,
                OrderNo = o.OrderNo,
                OrderDate = o.OrderDate,
                OWD = o.OWD,
                CSD = o.CSD,
                LCSD = o.LCSD,
                BrandCodeId = o.BrandCodeId,
                RefBrand = o.RefBrand,
                ArticleId = o.ArticleId,
                ArticleNo = o.ArticleNo,
                StyleNo = o.StyleNo,
                ShoeName = o.ShoeName,
                ArticleSizeCountry = o.ArticleSizeCountry,
                OrderSizeCountry = o.OrderSizeCountry,
                OutsoleNo = o.OutsoleNo,
                LastNo = o.LastNo,
            })
            .Distinct()
            .ToList();

            var lastHead = new List<string>();
            var articleHead = new List<string>();
            ordersHeads.ForEach(i =>
            {
                var s = new Models.Views.OrdersSizeRun();
                var mes = new List<string>();
                var articleSizeRun = new List<string>();
                var lastSizeRun = new List<string>();

                s.OrdersId = i.OrdersId;
                s.LocaleId = i.LocaleId;
                s.CompanyId = i.CompanyId;
                s.CompanyNo = i.CompanyNo;
                s.Customer = i.Customer;
                s.CustomerOrderNo = i.CustomerOrderNo;
                s.GBSPOReferenceNo = i.GBSPOReferenceNo;
                s.ProductType = i.ProductType;
                s.OrderQty = i.OrderQty;
                s.OrderNo = i.OrderNo;
                s.OWD = i.OWD;
                s.OrderDate = i.OrderDate;
                s.CSD = i.CSD;
                s.LCSD = i.LCSD;
                s.BrandCodeId = i.BrandCodeId;
                s.Brand = i.RefBrand;
                s.ArticleNo = i.ArticleNo;
                s.StyleNo = i.StyleNo;
                s.ShoeName = i.ShoeName;
                s.OrderSizeCountry = i.OrderSizeCountry;
                s.ArticleSizeCountry = i.ArticleSizeCountry;
                s.OutsoleNo = i.OutsoleNo;
                s.LastNo = i.LastNo;

                ordersItems.Where(oi => oi.OrdersId == i.OrdersId && oi.LocaleId == i.LocaleId).ToList().ForEach(oi =>
                {
                    var field = "";
                    if (oi.ArticleSizeSuffix != null && (oi.ArticleSizeSuffix.Contains("J") || oi.ArticleSizeSuffix.Contains("j")))
                    {
                        field = "SJ" + String.Format("{0:000}", (oi.ArticleInnerSize * 10));
                    }
                    else
                    {
                        field = "S" + String.Format("{0:000}", (oi.ArticleInnerSize / 100));
                    }

                    var lastField = "";
                    if (oi.LastSizeSuffix != null && (oi.LastSizeSuffix.Contains("J") || oi.LastSizeSuffix.Contains("j")))
                    {
                        lastField = "SJ" + String.Format("{0:000}", (oi.LastInnerSize * 10));
                    }
                    else
                    {
                        lastField = "S" + String.Format("{0:000}", (oi.LastInnerSize / 100));
                    }

                    mes.Add("'" + oi.ArticleSize + "':" + oi.Qty);
                    
                    articleSizeRun.Add("\"" + field + "\":" + oi.Qty);
                    articleHead.Add("\"" + field + "\":\"" + oi.ArticleDisplay + "\"");

                    lastSizeRun.Add("\"" + lastField + "\":" + oi.Qty);
                    lastHead.Add("\"" + lastField + "\":\"" + oi.LastDisplaySize + "\"");
                });

                s.MESFormat = "{" + string.Join(",", mes) + "}";
                s.LastSizeRun = "{" + string.Join(",", lastSizeRun) + "}";
                s.ArticleSizeRun = "{" + string.Join(",", articleSizeRun) + "}";
                sizeruns.Add(s);
            });

            sizeruns.ForEach(i =>
            {
                i.LastHead = "{" + string.Join(",", lastHead.Distinct().OrderBy(c => c)) + "}";
                i.ArticleHead = "{" + string.Join(",", articleHead.Distinct().OrderBy(c => c)) + "}";
            });

            return sizeruns.AsQueryable();
        }
        public IQueryable<Models.Views.OrdersIncome> GetOrdersIncome(string predicate)
        {
            var income = (
                from o in Orders.Get()
                select new
                {
                    Id = o.Id,
                    LocaleId = o.LocaleId,
                    Brand = o.Brand,
                    CompanyId = o.CompanyId,
                    Company = o.CompanyNo,
                    OrderNo = o.OrderNo,
                    Customer = o.Customer,
                    CustomerOrderNo = o.CustomerOrderNo,
                    GBSPOReferenceNo = o.GBSPOReferenceNo,
                    ArticleNo = o.ArticleNo,
                    StyleNo = o.StyleNo,
                    ShoeName = o.ShoeName,
                    OrderQty = o.OrderQty,
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    OrderDate = o.OrderDate,
                    OPD = o.OWD,
                    OWD = o.OWD,
                    Season = o.Season,
                    ProductType = o.RefProductType,
                    ProductTypeId = o.ProductType,
                    OrderType = o.RefProductType,
                    OrderTypeId = o.OrderType,
                    Currency = o.Dollar,
                    InvoicePriceByStyle = Quotation.Get().Where(i => (i.StyleNo.Length > 0 && i.StyleNo == o.StyleNo) && i.Dollar == o.Dollar && i.EffectiveDate <= o.OrderDate && i.Confirmed == 1 && i.ProductType == o.ProductType).OrderByDescending(i => i.EffectiveDate).Any() ?
                                          Quotation.Get().Where(i => (i.StyleNo.Length > 0 && i.StyleNo == o.StyleNo) && i.Dollar == o.Dollar && i.EffectiveDate <= o.OrderDate && i.Confirmed == 1 && i.ProductType == o.ProductType).OrderByDescending(i => i.EffectiveDate).FirstOrDefault().InvoicePriceIntel :
                                          0,
                    FactoryPriceByStyle = Quotation.Get().Where(i => (i.StyleNo.Length > 0 && i.StyleNo == o.StyleNo) && i.Dollar == o.Dollar && i.EffectiveDate <= o.OrderDate && i.Confirmed == 1 && i.ProductType == o.ProductType).OrderByDescending(i => i.EffectiveDate).Any() ?
                                          Quotation.Get().Where(i => (i.StyleNo.Length > 0 && i.StyleNo == o.StyleNo) && i.Dollar == o.Dollar && i.EffectiveDate <= o.OrderDate && i.Confirmed == 1 && i.ProductType == o.ProductType).OrderByDescending(i => i.EffectiveDate).FirstOrDefault().FactoryPriceIntel :
                                          0,

                    InvoicePriceByArticle = Quotation.Get().Where(i => (i.ArticleNo == o.ArticleNo && i.StyleNo.Trim().Length == 0 && i.Dollar == o.Dollar && i.EffectiveDate <= o.OrderDate && i.Confirmed == 1 && i.ProductType == o.ProductType)).Any() ?
                                            Quotation.Get().Where(i => (i.ArticleNo == o.ArticleNo && i.StyleNo.Trim().Length == 0 && i.Dollar == o.Dollar && i.EffectiveDate <= o.OrderDate && i.Confirmed == 1 && i.ProductType == o.ProductType)).OrderByDescending(i => i.EffectiveDate).FirstOrDefault().InvoicePriceIntel :
                                            0,
                    FactoryPriceByArticle = Quotation.Get().Where(i => (i.ArticleNo == o.ArticleNo && i.StyleNo.Trim().Length == 0 && i.Dollar == o.Dollar && i.EffectiveDate <= o.OrderDate && i.Confirmed == 1 && i.ProductType == o.ProductType)).Any() ?
                                            Quotation.Get().Where(i => (i.ArticleNo == o.ArticleNo && i.StyleNo.Trim().Length == 0 && i.Dollar == o.Dollar && i.EffectiveDate <= o.OrderDate && i.Confirmed == 1 && i.ProductType == o.ProductType)).OrderByDescending(i => i.EffectiveDate).FirstOrDefault().FactoryPriceIntel :
                                            0,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            return income.Select(i => new Models.Views.OrdersIncome
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                Brand = i.Brand,
                CompanyId = i.CompanyId,
                Company = i.Company,
                OrderNo = i.OrderNo,
                Customer = i.Customer,
                CustomerOrderNo = i.CustomerOrderNo,
                GBSPOReferenceNo = i.GBSPOReferenceNo,
                ArticleNo = i.ArticleNo,
                StyleNo = i.StyleNo,
                ShoeName = i.ShoeName,
                OrderQty = i.OrderQty,
                CSD = i.CSD,
                LCSD = i.LCSD,
                OrderDate = i.OrderDate,
                OPD = i.OWD,
                OWD = i.OWD,
                Season = i.Season,
                ProductType = Type.GetProductType().Where(t => t.Id == i.ProductTypeId).Select(i => i.NameTw).Max(),
                ProductTypeId = i.ProductTypeId,
                OrderType = Type.GetOrderType().Where(t => t.Id == i.OrderTypeId).Select(i => i.NameTw).Max(),
                OrderTypeId = i.OrderTypeId,
                Currency = i.Currency,
                InvoicePrice = i.InvoicePriceByStyle == 0 ? i.InvoicePriceByArticle : i.InvoicePriceByStyle,
                FactoryPrice = i.FactoryPriceByStyle == 0 ? i.FactoryPriceByArticle : i.FactoryPriceByStyle,
                // InvoicePrice = 0,
                InvoiceAmount = (i.InvoicePriceByStyle == 0 ? i.InvoicePriceByArticle : i.InvoicePriceByStyle) * i.OrderQty,
                // FactoryPrice = 0,
                FactoryAmount = (i.FactoryPriceByStyle == 0 ? i.FactoryPriceByArticle : i.FactoryPriceByStyle) * i.OrderQty,
            }).AsQueryable();
        }
        public IQueryable<Models.Views.OrdersSummary> GetOrdersSummary(string predicate)
        {
            var orders = Orders.Get()
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(o => new Models.Views.OrdersSummary
            {
                CompanyId = o.CompanyId,
                Company = o.CompanyNo,
                BrandCode = o.Brand,
                ArticleNo = o.ArticleNo,
                StyleNo = o.StyleNo,
                ShoeName = o.ShoeName,
                OrderQty = o.OrderQty,
            })
                .ToList()
                .GroupBy(i => new { i.CompanyId, i.Company, i.BrandCode, i.ArticleNo, i.StyleNo, i.ShoeName })
                .Select(o => new Models.Views.OrdersSummary
                {
                    CompanyId = o.Key.CompanyId,
                    Company = o.Key.Company,
                    BrandCode = o.Key.BrandCode,
                    ArticleNo = o.Key.ArticleNo,
                    StyleNo = o.Key.StyleNo,
                    ShoeName = o.Key.ShoeName,
                    OrderQty = o.Sum(i => i.OrderQty),
                })
                .AsQueryable();
            return orders;
        }

        public IQueryable<Models.Views.OrdersRecord> GetOrderRecordSummary(string predicate) {

            var items = Orders.GetEntity().Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new {
                Year = ((DateTime)i.KeyInDate).Year,
                Month = ((DateTime)i.KeyInDate).Month,
                CompanyNo = i.CompanyNo,
                CompanyId = i.CompanyId,
                Brand = i.Brand,
                BrandCodeId = i.BrandCodeId
            })
            .GroupBy(i => new { i.Year, i.Month, i.CompanyNo, i.CompanyId, i.Brand, i.BrandCodeId})
            .Select(i => new {
                Year = i.Key.Year,
                Month = i.Key.Month,
                CompanyNo = i.Key.CompanyNo,
                CompanyId = i.Key.CompanyId,
                Brand = i.Key.Brand,
                BrandCodeId = i.Key.BrandCodeId,
                Records = i.Count()
            })
            .ToList();


        var result = items.Select(i => new Models.Views.OrdersRecord {
                CompanyNo = i.CompanyNo,
                CompanyId = i.CompanyId,
                Brand = i.Brand,
                BrandCodeId = i.BrandCodeId,
                Records = i.Records,
                KeyInMonth = Convert.ToDecimal(i.Year.ToString("0000") + i.Month.ToString("00"))
            })
            .ToList();

            return result.AsQueryable();

        }
    }
}