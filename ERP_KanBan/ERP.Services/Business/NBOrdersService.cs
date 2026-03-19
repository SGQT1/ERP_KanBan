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
using ERP.Models.System;
using ERP.Models.Views;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;

// using Z.EntityFramework.Plus;

namespace ERP.Services.Business
{
    public class NBOrdersService : BusinessService
    {
        private ERP.Services.Business.Entities.NBOrdersService NBOrders { get; set; }
        private ERP.Services.Business.Entities.NBOrdersItemService NBOrdersItem { get; set; }
        private ERP.Services.Entities.StyleService Style { get; set; }
        private ERP.Services.Entities.ArticleService Article { get; set; }
        private ERP.Services.Entities.CodeItemService CodeItem { get; set; }
        private ERP.Services.Entities.ArticleSizeRunService ArticleSizeRun { get; set; }
        public NBOrdersService(
            ERP.Services.Business.Entities.NBOrdersService nbOrders,
            ERP.Services.Business.Entities.NBOrdersItemService nbOrdersItem,
            ERP.Services.Entities.ArticleService articleService,
            ERP.Services.Entities.StyleService styleService,
            ERP.Services.Entities.CodeItemService codeItemService,
            ERP.Services.Entities.ArticleSizeRunService articleSizeRunService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            NBOrders = nbOrders;
            NBOrdersItem = nbOrdersItem;
            Article = articleService;
            Style = styleService;
            CodeItem = codeItemService;
            ArticleSizeRun = articleSizeRunService;
        }
        public IQueryable<ERP.Models.Views.NBOrders> Get()
        {
            return NBOrders.Get();
        }
        public IQueryable<ERP.Models.Views.NBOrdersItem> GetItems()
        {
            return NBOrdersItem.Get();
        }
        public List<ERP.Models.Views.NBOrdersItem> Save(List<ERP.Models.Views.NBOrdersItem> items)
        {
            //remove NBOrders,NBOrderItem by CustomerPONo
            //create NBOrders,NBOrderItem from items
            try
            {
                UnitOfWork.BeginTransaction();

                // step1.remove orders.
                var localeId = items.Select(i => i.LocaleId).Max();
                var ordersNos = items.Select(i => i.OrderNo).Distinct().ToList();
                NBOrders.RemoveRange(i => ordersNos.Contains(i.OrderNo) && i.LocaleId == localeId);
                NBOrdersItem.RemoveRange(i => ordersNos.Contains(i.CustomerPONo) && i.LocaleId == localeId);
                // step2.insert orders,ordersItem
                // step2.1 insert orders
                var orders = items.GroupBy(i => new
                {
                    CustomerPONo = i.PONo.Trim(),
                    CustomerName = i.Customer.Trim() + " " + i.Market.Trim(),
                    OrderNo = i.OrderNo,
                    StyleNo = i.StylePartNo,
                    LocaleId = i.LocaleId,
                    ModifyUserName = i.ModifyUserName,
                    CSD = i.OrigReqXFD,
                    ETD = i.ExpOrActXFD,
                    LCSD = i.ExpOrActXFD,
                    OrderDate = i.POReleaseDate,
                    Season = i.Season,
                })
                .Select(i => new ERP.Models.Views.NBOrders
                {
                    LocaleId = i.Key.LocaleId,
                    ModifyUserName = i.Key.ModifyUserName,
                    LastUpdateTime = DateTime.Now,
                    CustomerName = i.Key.CustomerName,
                    PONo = i.Key.CustomerPONo,
                    StyleNo = i.Key.StyleNo,
                    OrderNo = i.Key.OrderNo,
                    CSD = ParseDate(i.Key.CSD),
                    ETD = ParseDate(i.Key.ETD),
                    OrderDate = ParseDate(i.Key.OrderDate),
                    LCSD = ParseDate(i.Key.LCSD),
                    Season = i.Key.Season,
                })
                .ToList();
                NBOrders.CreateRange(orders);
                NBOrdersItem.CreateRange(items);
                UnitOfWork.Commit();
                var result = GetItems().Where(i => ordersNos.Contains(i.OrderNo) && i.LocaleId == i.LocaleId).ToList();
                return result;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        private DateTime? ParseDate(string date)
        {
            DateTime dateTime;
            bool isDateTime = false;

            try
            {
                isDateTime = DateTime.TryParse(date, out dateTime);
                if (isDateTime)
                {

                    return DateTime.Parse(dateTime.ToString("yyyy-MM-dd 00:00:00.000"));
                }
                else
                {
                    return (DateTime?)null;
                }
            }
            catch
            {
                return (DateTime?)null;
            }
        }

        public Models.Views.OrdersGroup GetOrdersGroup(string ordersNo, int localeId)
        {
            var ordersGroup = new Models.Views.OrdersGroup() { ModelState = new ModelState { isValid = false, Code = string.Empty } };
            try
            {
                var msgCode = new List<string>();
                var _orders = NBOrders.Get().Where(i => i.OrderNo.CompareTo(ordersNo) == 0 && i.LocaleId == localeId).FirstOrDefault();

                if (_orders != null)
                {
                    var _ordersItems = NBOrdersItem.Get().Where(i => i.OrderNo == _orders.OrderNo && i.LocaleId == _orders.LocaleId).ToList();
                    if (_ordersItems.Count > 0)
                    {
                        var item = _ordersItems.First();
                        var style = Style.Get().Where(i => i.StyleNo == _orders.StyleNo && i.LocaleId == localeId).FirstOrDefault();
                        var article = style == null ? Article.Get().Where(i => i.ArticleNo == item.StylePartNo && i.LocaleId == localeId).FirstOrDefault() :
                            Article.Get().Where(i => i.Id == style.ArticleId && i.LocaleId == localeId).FirstOrDefault();

                        var dollar = CodeItem.Get().Where(i => i.NameTW == "USD" && i.CodeType == "02" && i.LocaleId == localeId).FirstOrDefault();
                        var articleSize = article == null ? null : ArticleSizeRun.Get().Where(i => i.ArticleId == article.Id && i.LocaleId == article.LocaleId).ToList();
                        var orderSize = CodeItem.Get().Where(i => i.NameTW == "US" && i.LocaleId == localeId && i.CodeType == "35").FirstOrDefault();

                        try
                        {
                            //step 1: get orders
                            var _orderInfo = (
                                from o in _ordersItems
                                group o by new { o.OrderNo, o.LocaleId } into g
                                select new
                                {
                                    OrderNo = g.First().OrderNo,
                                    LocaleId = g.First().LocaleId,
                                    Qty = g.Sum(i => Convert.ToDecimal(i.Quantity)),
                                    CustomerPONo = g.Max(i => i.PONo),
                                    OrderReleaseDate = g.Max(i => i.POReleaseDate),
                                    RHD = g.Max(i => i.POReleaseDate),
                                    EHD = g.Max(i => i.ExpOrActXFD),
                                    Season = g.Max(i => i.Season),
                                    CustomerVendorCode = g.First().VendorShortName,
                                }
                            ).FirstOrDefault();

                            var orders = new Models.Views.Orders
                            {
                                Id = 0,
                                OrderDate = (DateTime)_orders.OrderDate,
                                OrderNo = _orders.OrderNo,
                                // CustomerId
                                ArticleId = article == null ? 0 : article.Id,
                                StyleId = style == null ? 0 : style.Id,
                                OrderType = 2,
                                ProductType = 2,
                                UnitPrice = 0,
                                ReferUnitPrice = 0,
                                ETD = DateTime.Parse(_orderInfo.EHD),
                                // ShippingDate
                                CompanyId = _orders.LocaleId,
                                OrderSizeCountryCodeId = orderSize == null ? 0 : orderSize.Id,
                                OrderSizeCountryCode = orderSize == null ? "" : orderSize.NameTW,

                                CustomerOrderNo = _orderInfo.CustomerPONo,
                                // ModifyUserName
                                // LastUpdateTime
                                LocaleId = localeId,
                                Status = 0,
                                CSD = (DateTime)_orders.CSD,
                                OrderQty = _orderInfo.Qty,
                                LabelDesc = "",
                                SpecialDesc = style == null ? "" : style.IsSpecial == 1 ? "* 注意，" + article.ArticleName + "為重要型體，需特別注意品質要求。\n" : "",
                                CustomerVendorCode = _orderInfo.CustomerVendorCode,
                                // PackingType
                                // Mark1Desc
                                // Mark1PhotoURL
                                // Mark2Desc
                                // Mark2PhotoURL
                                // Mark3Desc
                                // Mark3PhotoURL
                                // Mark4Desc
                                // Mark4PhotoURL
                                // Mark5Desc
                                // Mark5PhotoURL
                                DollarCodeId = dollar == null ? 0 : dollar.Id,
                                doMRP = 1,
                                Version = 1,
                                ProcessSetId = 0,
                                ExportPortId = 0,
                                InsockLabel = style == null ? "" : style.InsockLabel,
                                PackingTypeDesc = "10",
                                CustomerStyleNo = _orders.StyleNo,
                                ShoeName = article == null ? "" : article.ArticleName,
                                // SpecialNote
                                PayType = 3,
                                DeliveryTerms = "FOB",
                                TransitType = 0,
                                ToolingFund = 0,
                                // SpecialPackingStatus
                                // ARCustomerId
                                IsApproved = 1,
                                // PaymentDate
                                ARLocaleId = 6,
                                // ParentOrdersId
                                RefOrdersLocaleId = localeId,
                                LCSD = _orders.CSD,
                                GBSPOReferenceNo = _orderInfo.CustomerPONo,
                                // KeyInDate
                                OWD = _orders.OrderDate,
                                OWRD = _orders.OrderDate,
                                RSD = _orders.CSD,
                                // GBSCD
                                // GBSPUD
                                ArticleNo = article == null ? "" : article.ArticleNo,
                                StyleNo = style == null ? "" : _orders.StyleNo,
                                BrandCodeId = article == null ? 0 : article.BrandCodeId,
                                Season = _orders.Season,
                                RefColorDesc = style == null ? "" : style.ColorDesc,
                                RefStyleState = style == null ? 0 : style.doMRP,

                                ArticleSizeCountryCodeId = style == null ? 0 : style.SizeCountryCodeId,
                            };
                            if (style == null)
                            {
                                msgCode.Add("Style");
                            }
                            if (article == null)
                            {
                                msgCode.Add("Article");
                            }
                            if (dollar == null)
                            {
                                msgCode.Add("Currency");
                            }
                            ordersGroup.Orders = orders;
                        }
                        catch
                        {
                            ordersGroup.ModelState.Code += "Order Date From Excel Incorrect. Cannot Import.  ";
                        }

                        //get size run
                        //step 1:select NBOrdersItems: update size display 1,15,2,25 > 10,15,20,25
                        //step 2:group by Id,OrdersId,ArticleSize,ArticleSizeSuffix,ArticleInnerSize,DisplaySize,LocaleId,
                        //step 3:sum qty and convet OrdersItem obj
                        try
                        {
                            if (articleSize == null || articleSize.Count() == 0)
                            {
                                msgCode.Add("ArticleSize");
                            }
                            else
                            {
                                _ordersItems.ForEach(i => i.Size = i.Size.PadRight(3, '0')); // 往右補零
                                var ordersItems = _ordersItems
                                    .Select(i => new
                                    {
                                        Id = 0,
                                        OrdersId = 0,
                                        ArticleSize = articleSize.Where(a => a.ArticleSize == Convert.ToDecimal(i.Size) / 10 && a.ArticleSizeSuffix == i.SizeSuffix).FirstOrDefault().ArticleSize,
                                        ArticleSizeSuffix = articleSize.Where(a => a.ArticleSize == Convert.ToDecimal(i.Size) / 10 && a.ArticleSizeSuffix == i.SizeSuffix).FirstOrDefault().ArticleSizeSuffix,
                                        ArticleInnerSize = articleSize.Where(a => a.ArticleSize == Convert.ToDecimal(i.Size) / 10 && a.ArticleSizeSuffix == i.SizeSuffix).FirstOrDefault().ArticleInnerSize,
                                        DisplaySize = articleSize.Where(a => a.ArticleSize == Convert.ToDecimal(i.Size) / 10 && a.ArticleSizeSuffix == i.SizeSuffix).FirstOrDefault().ArticleDisplaySize,
                                        Qty = Convert.ToDecimal(i.Quantity),
                                        LocaleId = i.LocaleId,
                                    })
                                    .GroupBy(i => new
                                    {
                                        i.Id,
                                        i.OrdersId,
                                        i.ArticleSize,
                                        i.ArticleSizeSuffix,
                                        i.ArticleInnerSize,
                                        i.DisplaySize,
                                        i.LocaleId,
                                    })
                                    .Select(i => new Models.Views.OrdersItem
                                    {
                                        Id = i.Key.Id,
                                        OrdersId = i.Key.OrdersId,
                                        ArticleSize = i.Key.ArticleSize,
                                        ArticleSizeSuffix = i.Key.ArticleSizeSuffix,
                                        // ArticleInnerSize = (decimal)i.Key.ArticleInnerSize * 1000,
                                        ArticleInnerSize = (decimal)i.Key.ArticleInnerSize,
                                        // DisplaySize = i.Key.ArticleSize.ToString("#0.0"),
                                        DisplaySize = i.Key.DisplaySize,
                                        Qty = i.Sum(s => s.Qty),
                                        LocaleId = i.Key.LocaleId,
                                    })
                                    .Where(i => i.Qty > 0)
                                    .OrderBy(i => i.ArticleInnerSize)
                                    .ToList();

                                ordersGroup.OrdersItem = ordersItems;
                            }
                        }
                        catch
                        {
                            ordersGroup.ModelState.Code += "ArticleSize Data Incorrect Or Not Match Order Size  ";
                        }

                    }
                }
            }
            catch
            {
                ordersGroup.ModelState.Code = "Order Data Incollect, Cannot Import.";
            }

            return ordersGroup;
        }
    }
}