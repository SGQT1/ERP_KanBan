using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Diamond.DataSource.Extensions;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.Business.Entities
{
    public class OrdersService : BusinessService
    {
        private Services.Entities.OrdersService Orders { get; }
        private Services.Entities.CompanyService Company { get; }
        private Services.Entities.CustomerService Customer { get; }
        private Services.Entities.ArticleService Article { get; }
        private Services.Entities.StyleService Style { get; }
        private Services.Entities.OutsoleService Outsole { get; }
        private Services.Entities.LastService Last { get; }
        private Services.Entities.CodeItemService CodeItem { get; }
        private Services.Entities.PortService Port { get; }
        private Services.Entities.MRPRemovedService MRPRemoved { get; }
        private Services.Entities.OrdersItemService OrdersItem { get; }
        private Services.Entities.QueueDoMRPLogService QueueDoMRPLog { get; set; }

        private Services.Business.Entities.TypeService Type { get; }
        private Services.Business.Entities.PUMAOrdersService PUMAOrders { get; set; }
        private Services.Business.Entities.PUMAOrdersItemService PUMAOrdersItem { get; set; }

        public OrdersService(
            Services.Entities.OrdersService ordersService,
            Services.Entities.CompanyService companyService,
            Services.Entities.CustomerService customerService,
            Services.Entities.ArticleService articleService,
            Services.Entities.StyleService styleService,
            Services.Entities.OutsoleService outsoleService,
            Services.Entities.LastService lastService,
            Services.Entities.CodeItemService codeitemService,
            Services.Entities.PortService portService,
            Services.Entities.MRPRemovedService mrpRemovedService,
            Services.Entities.QueueDoMRPLogService queueDoMRPLogService,
            Services.Entities.OrdersItemService ordersItemService,

            Services.Business.Entities.PUMAOrdersService pumaOrdersService,
            Services.Business.Entities.PUMAOrdersItemService pumaOrdersItemService,
            Services.Business.Entities.TypeService typeService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            Orders = ordersService;
            Company = companyService;
            Customer = customerService;
            Article = articleService;
            Style = styleService;
            Outsole = outsoleService;
            Last = lastService;
            CodeItem = codeitemService;
            Port = portService;
            MRPRemoved = mrpRemovedService;
            QueueDoMRPLog = queueDoMRPLogService;
            OrdersItem = ordersItemService;

            Type = typeService;
            PUMAOrders = pumaOrdersService;
            PUMAOrdersItem = pumaOrdersItemService;

        }

        public IQueryable<Models.Entities.Orders> GetPreLoadOrders()
        {
            return Orders.Get();
        }

        public IQueryable<Models.Views.Orders> GetEntity()
        {
            var orders = (
                from o in Orders.Get()
                select new Models.Views.Orders
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    OrderNo = o.OrderNo,
                    CustomerId = o.CustomerId,
                    ArticleId = o.ArticleId,
                    StyleId = o.StyleId,
                    OrderType = o.OrderType,
                    ProductType = o.ProductType,
                    UnitPrice = o.UnitPrice,
                    ReferUnitPrice = o.ReferUnitPrice,
                    ETD = o.ETD,
                    ShippingDate = o.ShippingDate,
                    CompanyId = o.CompanyId,
                    OrderSizeCountryCodeId = o.SizeCountryCodeId, // Order Size
                    PackingDescTW = o.PackingDescTW,
                    PackingDescEng = o.PackingDescEng,
                    SafeCode = o.SafeCode,
                    BarcodeCodeId = o.BarcodeCodeId,
                    Mark = o.Mark,
                    SideMark = o.SideMark,
                    CustomerOrderNo = o.CustomerOrderNo,
                    ModifyUserName = o.ModifyUserName,
                    LastUpdateTime = o.LastUpdateTime,
                    LocaleId = o.LocaleId,
                    Status = o.Status,
                    CSD = o.CSD,
                    OrderQty = o.OrderQty,
                    PackingType = o.PackingType,
                    LabelDesc = o.Mark1Desc == null ? "" : o.Mark1Desc,
                    SpecialDesc = o.Mark5Desc == null ? "" : o.Mark5Desc,
                    CustomerVendorCode = o.Mark4Desc == null ? "" : o.Mark4Desc,
                    DollarCodeId = o.DollarCodeId,
                    doMRP = o.doMRP,
                    Version = o.Version,
                    ProcessSetId = o.ProcessSetId,
                    ExportPortId = o.ExportPortId,
                    InsockLabel = o.InsockLabel,
                    PackingTypeDesc = o.PackingTypeDesc,
                    CustomerStyleNo = o.CustomerStyleNo,
                    ShoeName = o.ShoeName,
                    SpecialNote = o.SpecialNote,
                    PayType = o.PayType,
                    DeliveryTerms = o.DeliveryTerms,
                    TransitType = o.TransitType,
                    ToolingFund = o.ToolingFund,
                    SpecialPackingStatus = o.SpecialPackingStatus,
                    ARCustomerId = o.ARCustomerId,
                    IsApproved = o.IsApproved,
                    PaymentDate = o.PaymentDate,
                    ARLocaleId = o.ARLocaleId,
                    ParentOrdersId = o.ParentOrdersId,
                    RefOrdersLocaleId = o.RefOrdersLocaleId,
                    LCSD = o.LCSD,
                    GBSPOReferenceNo = o.GBSPOReferenceNo,
                    KeyInDate = o.KeyInDate,
                    OWD = o.OWD,
                    OWRD = o.OWRD,
                    RSD = o.RSD,
                    GBSCD = o.GBSCD,
                    GBSPUD = o.GBSPUD,
                    ArticleNo = o.ArticleNo,
                    StyleNo = o.StyleNo,
                    BrandCodeId = o.BrandCodeId,
                    Season = o.Season,

                    LocaleNo = o.LocaleNo,
                    CompanyNo = o.CompanyNo,
                    Customer = o.Customer,
                    LastNo = o.LastNo,
                    OutsoleNo = o.OutsoleNo,
                    Dollar = o.Dollar,
                    Brand = o.Brand,
                    OrderSizeCountryCode = o.OrderSizeCountryCode,
                    ArticleSizeCountryCodeId = o.ArticleSizeCountryCodeId,
                    ArticleSizeCountryCode = o.ArticleSizeCountryCode,
                    Port = o.Port,
                    ARLocaleNo = o.ARLocaleNo,
                    OrdersLocaleNo = o.OrdersLocaleNo,

                    RefDeliveryTerm = o.DeliveryTerms,
                    IsClosed = o.Status == 2 ? true : false,
                    RefColorDesc = Style.Get().Where(i => i.Id == o.StyleId && i.LocaleId == o.LocaleId).Max(i => i.ColorDesc),
                    RefOutsoleColorDesc = Style.Get().Where(i => i.Id == o.StyleId && i.LocaleId == o.LocaleId).Max(i => i.OutsoleColorDescTW),
                    SpecialCustomer = Customer.Get().Where(i => i.Id == o.CustomerId && i.LocaleId == o.LocaleId).Max(i => i.IsSpecial) == 1 ? true : false,
                }
            );
            return orders;
        }
        public IQueryable<Models.Views.Orders> GetCache()
        {
            return Orders.Get().Select(i => new Models.Views.Orders
            {
                Id = i.Id,
                OrderNo = i.OrderNo,
                StyleNo = i.StyleNo,
                ShoeName = i.ShoeName,
                CompanyId = i.CompanyId,
                LocaleId = i.LocaleId,
                OrderDate = i.OrderDate,
                CSD = i.CSD,
                LCSD = i.LCSD,
                Brand = i.Brand,
                Status = i.Status,
                OrderQty = i.OrderQty,
            });
        }
        public IQueryable<Models.Views.Orders> Get()
        {
            return Query();
        }
        public IQueryable<Models.Views.Orders> Get(Expression<Func<Models.Views.Orders, bool>> predicate)
        {
            var orders = Query().Where(predicate).ToList();
            return TypeMap(orders);
        }
        public IQueryable<Models.Views.Orders> Get(string predicate)
        {
            var orders = Query().Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate).ToList();
            return TypeMap(orders);
        }
        public IQueryable<Models.Views.Orders> Get(string predicate, string sort, int take, int skip)
        {
            var orders = Query().Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate);

            if (sort.Length > 0)
            {
                orders = orders.OrderBy(sort);
            }
            if (take > 0)
            {
                orders = orders.Take(take);
            }
            if (skip > 0)
            {
                orders = orders.Skip(skip);
            }

            var data = orders.Select(i => new Models.Views.Orders
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                Brand = i.Brand,
                CompanyNo = i.CompanyNo,
                OrderNo = i.OrderNo,
                RefOrdersStatus = i.RefOrdersStatus,
                Customer = i.Customer,
                CustomerOrderNo = i.CustomerOrderNo,
                GBSPOReferenceNo = i.GBSPOReferenceNo,
                StyleNo = i.StyleNo,
                ShoeName = i.ShoeName,
                RefColorDesc = i.RefColorDesc,
                OrderQty = i.OrderQty,
                CSD = i.CSD,
                LCSD = i.LCSD,
                ETD = i.ETD,
                Dollar = i.Dollar,
                OrderDate = i.OrderDate,
                OWD = i.OWD,
                OWRD = i.OWRD,
                RSD = i.RSD,
                GBSCD = i.GBSCD,
                GBSPUD = i.GBSPUD,
                Season = i.Season,
                RefOrderType = i.RefOrderType,
                RefProductType = i.RefProductType,
                OutsoleNo = i.OutsoleNo,
                LastNo = i.LastNo,
                InsockLabel = i.InsockLabel,
                RefOutsoleColorDesc = i.RefOutsoleColorDesc,
                RefTransitType = i.RefTransitType,
                SpecialNote = i.SpecialNote,
                SpecialCustomer = i.SpecialCustomer,
                SpecialStyle = i.SpecialStyle,
                SpecialDesc = i.SpecialDesc,
                ProductType = i.ProductType,
                OrderType = i.OrderType,
                TransitType = i.TransitType,
                Status = i.Status,
                PayType = i.PayType,
                PackingType = i.PackingType,
                IsClosed = i.IsClosed,
                CustomerVendorCode = i.CustomerVendorCode,
            })
            .ToList();
            return TypeMap(data);
        }

        private IQueryable<Models.Views.Orders> TypeMap(List<Models.Views.Orders> orders)
        {
            orders.ForEach(o =>
            {
                o.RefApproved = Type.GetBooleanType().Where(i => i.Id == o.IsApproved).Select(i => i.NameTw).Max();
                o.RefdoMRP = Type.GetBooleanType().Where(i => i.Id == o.doMRP).Select(i => i.NameTw).Max();
                o.RefOrdersStatus = Type.GetOrderStatus().Where(i => i.Id == o.Status).Select(i => i.NameTw).Max();
                o.RefProductType = Type.GetProductType().Where(i => i.Id == o.ProductType).Select(i => i.NameTw).Max();
                o.RefOrderType = Type.GetOrderType().Where(i => i.Id == o.OrderType).Select(i => i.NameTw).Max();
                o.RefPayType = Type.GetPayType().Where(i => i.Id == o.PayType).Select(i => i.NameTw).Max();
                o.RefPackingType = Type.GetPackingType().Where(i => i.Id == o.PackingType).Select(i => i.NameTw).Max();
                o.RefTransitType = Type.GetTransitType().Where(i => i.Id == o.TransitType).Select(i => i.NameTw).Max();
            });
            return orders.AsQueryable();
        }
        private IQueryable<Models.Views.Orders> Query()
        {
            var orders = (
                from o in Orders.Get()
                join a in Article.Get() on new { ArticleId = o.ArticleId, LocaleId = o.LocaleId } equals new { ArticleId = a.Id, LocaleId = a.LocaleId }
                join s in Style.Get() on new { StyleId = o.StyleId, LocaleId = o.LocaleId } equals new { StyleId = s.Id, LocaleId = s.LocaleId }
                join c in Customer.Get() on new { CustomerId = o.CustomerId, LocaleId = o.LocaleId } equals new { CustomerId = c.Id, LocaleId = c.LocaleId }
                join arc in Customer.Get() on new { CustomerId = o.ARCustomerId, LocaleId = o.LocaleId } equals new { CustomerId = arc.Id, LocaleId = arc.LocaleId }
                select new Models.Views.Orders
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    OrderNo = o.OrderNo,
                    CustomerId = o.CustomerId,
                    ArticleId = o.ArticleId,
                    StyleId = o.StyleId,
                    OrderType = o.OrderType,
                    ProductType = o.ProductType,
                    UnitPrice = o.UnitPrice,
                    ReferUnitPrice = o.ReferUnitPrice,
                    ETD = o.ETD,
                    ShippingDate = o.ShippingDate,
                    CompanyId = o.CompanyId,
                    OrderSizeCountryCodeId = o.SizeCountryCodeId, // Order Size
                    PackingDescTW = o.PackingDescTW,
                    PackingDescEng = o.PackingDescEng,
                    SafeCode = o.SafeCode,
                    BarcodeCodeId = o.BarcodeCodeId,
                    Mark = o.Mark,
                    SideMark = o.SideMark,
                    CustomerOrderNo = o.CustomerOrderNo,
                    ModifyUserName = o.ModifyUserName,
                    LastUpdateTime = o.LastUpdateTime,
                    LocaleId = o.LocaleId,
                    Status = o.Status,
                    CSD = o.CSD,
                    OrderQty = o.OrderQty,
                    PackingType = o.PackingType,
                    LabelDesc = o.Mark1Desc == null ? "" : o.Mark1Desc,
                    SpecialDesc = o.Mark5Desc == null ? "" : o.Mark5Desc,
                    CustomerVendorCode = o.Mark4Desc == null ? "" : o.Mark4Desc,
                    DollarCodeId = o.DollarCodeId,
                    doMRP = o.doMRP,
                    Version = o.Version,
                    ProcessSetId = o.ProcessSetId,
                    ExportPortId = o.ExportPortId,
                    InsockLabel = o.InsockLabel,
                    PackingTypeDesc = o.PackingTypeDesc,
                    CustomerStyleNo = o.CustomerStyleNo,
                    ShoeName = o.ShoeName,
                    SpecialNote = o.SpecialNote,
                    PayType = o.PayType,
                    DeliveryTerms = o.DeliveryTerms,
                    TransitType = o.TransitType,
                    ToolingFund = o.ToolingFund,
                    SpecialPackingStatus = o.SpecialPackingStatus,
                    ARCustomerId = o.ARCustomerId,
                    IsApproved = o.IsApproved,
                    PaymentDate = o.PaymentDate,
                    ARLocaleId = o.ARLocaleId,
                    ParentOrdersId = o.ParentOrdersId,
                    RefOrdersLocaleId = o.RefOrdersLocaleId,
                    LCSD = o.LCSD,
                    GBSPOReferenceNo = o.GBSPOReferenceNo,
                    KeyInDate = o.KeyInDate,
                    OWD = o.OWD,
                    OWRD = o.OWRD,
                    RSD = o.RSD,
                    GBSCD = o.GBSCD,
                    GBSPUD = o.GBSPUD,
                    ArticleNo = o.ArticleNo,
                    StyleNo = o.StyleNo,
                    BrandCodeId = o.BrandCodeId,
                    Season = o.Season,

                    LocaleNo = o.LocaleNo,
                    CompanyNo = o.CompanyNo,
                    Customer = o.Customer,
                    LastNo = o.LastNo,
                    OutsoleNo = o.OutsoleNo,
                    ArticleName = a.ArticleName,
                    Dollar = o.Dollar,
                    Brand = o.Brand,
                    OrderSizeCountryCode = o.OrderSizeCountryCode,
                    ArticleSizeCountryCodeId = o.ArticleSizeCountryCodeId,
                    ArticleSizeCountryCode = o.ArticleSizeCountryCode,
                    Port = o.Port,
                    ARLocaleNo = o.ARLocaleNo,
                    OrdersLocaleNo = o.OrdersLocaleNo,

                    RefArticleNo = a.ArticleNo,
                    RefStyleNo = s.StyleNo,
                    RefStyleState = s.doMRP,
                    RefColorDesc = s.ColorDesc,
                    RefOutsoleColorDesc = s.OutsoleColorDescTW,
                    RefDeliveryTerm = o.DeliveryTerms,
                    RefBarcode = CodeItem.Get().Where(i => i.Id == o.BarcodeCodeId && i.LocaleId == o.LocaleId && i.CodeType == "33").Select(i => i.NameTW).Max(),
                    RefARCustomer = arc.ChineseName,

                    // RefApproved = Type.GetBooleanType().Where(i => i.Id == o.IsApproved).Select(i => i.NameTw).Max(),
                    // RefdoMRP = Type.GetBooleanType().Where(i => i.Id == o.doMRP).Select(i => i.NameTw).Max(),
                    // RefOrdersStatus = Type.GetOrderStatus().Where(i => i.Id == o.Status).Select(i => i.NameTw).Max(),
                    // RefProductType = Type.GetProductType().Where(i => i.Id == o.ProductType).Select(i => i.NameTw).Max(),
                    // RefOrderType = Type.GetOrderType().Where(i => i.Id == o.OrderType).Select(i => i.NameTw).Max(),
                    // RefPayType = Type.GetPayType().Where(i => i.Id == o.PayType).Select(i => i.NameTw).Max(),
                    // RefPackingType = Type.GetPackingType().Where(i => i.Id == o.PackingType).Select(i => i.NameTw).Max(),
                    // RefTransitType = Type.GetTransitType().Where(i => i.Id == o.TransitType).Select(i => i.NameTw).Max(),
                    SpecialStyle = s.IsSpecial == 1 ? true : false,
                    SpecialCustomer = c.IsSpecial == 1 ? true : false,
                    // HasBOM = QueueDoMRPLog.Get().Where(i => i.OrdersId == o.Id && i.LocaleId == o.LocaleId).Any(),
                    IsClosed = o.Status == 2 ? true : false,
                }
            );
            return orders;
        }
        public Models.Views.Orders Create(Models.Views.Orders orders)
        {
            var _orders = Orders.Create(Build(orders));

            return Get(i => i.Id == _orders.Id && i.LocaleId == _orders.LocaleId).FirstOrDefault();
        }
        public Models.Views.Orders Update(Models.Views.Orders orders)
        {
            var _orders = Orders.Update(Build(orders));
            return Get(i => i.Id == _orders.Id && i.LocaleId == _orders.LocaleId).FirstOrDefault();
        }
        public void UpdatePackingType(int id, int localeId, int packType)
        {
            Orders.UpdateRange(
                i => i.Id == id && i.LocaleId == localeId,
                // i => new Models.Entities.Orders { PackingType = packType });
                u => u.SetProperty(p => p.PackingType, v => packType)
            );
        }
        public void ClosedOrders(int localeId, IEnumerable<decimal> closedIds, IEnumerable<decimal> unClosedIds)
        {
            // update Status Id = 2
            Orders.UpdateRange(
                i => closedIds.Contains(i.Id) && i.LocaleId == localeId && i.Status != 2,
                // u => new Models.Entities.Orders { Status = 2 }
                u => u.SetProperty(p => p.Status, v => 2)
            );

            // updat Status id = paymentId where Id in ShipmentId, closed only
            Orders.UpdateRange(
                i => unClosedIds.Contains(i.Id) && i.LocaleId == localeId && i.Status == 2,
                // u => new Models.Entities.Orders { Status = 0 }
                u => u.SetProperty(p => p.Status, v => 0)
            );
        }
        public void Remove(Models.Views.Orders orders)
        {
            Orders.Remove(Build(orders));
        }
        //for update, transfer view model to entity
        private Models.Entities.Orders Build(Models.Views.Orders item)
        {
            var article = Article.Get().Where(i => i.Id == item.ArticleId && i.LocaleId == item.LocaleId).FirstOrDefault();
            var style = Style.Get().Where(i => i.Id == item.StyleId && i.LocaleId == item.LocaleId).FirstOrDefault();
            var company = Company.Get().ToList();

            item.LocaleNo = company.Where(i => i.Id == item.LocaleId).Select(i => i.CompanyNo).Max();
            item.CompanyNo = company.Where(i => i.Id == item.CompanyId).Select(i => i.CompanyNo).Max();
            item.Customer = Customer.Get().Where(i => i.Id == item.CustomerId && i.LocaleId == item.LocaleId).Select(i => i.ChineseName).Max();
            item.LastNo = Last.Get().Where(i => i.Id == style.LastId && i.LocaleId == item.LocaleId).Select(i => i.LastNo).Max();
            item.OutsoleNo = Outsole.Get().Where(i => i.Id == style.OutsoleId && i.LocaleId == item.LocaleId).Select(i => i.OutsoleNo).Max();
            item.ArticleName = article.ArticleName;
            item.Dollar = CodeItem.Get().Where(i => i.Id == item.DollarCodeId && i.LocaleId == item.LocaleId && i.CodeType == "02").Select(i => i.NameTW).Max();
            item.Brand = CodeItem.Get().Where(i => i.Id == item.BrandCodeId && i.LocaleId == item.LocaleId && i.CodeType == "25").Select(i => i.NameTW).Max();
            item.OrderSizeCountryCode = CodeItem.Get().Where(i => i.Id == item.OrderSizeCountryCodeId && i.LocaleId == item.LocaleId && i.CodeType == "35").Select(i => i.NameTW).Max();
            item.ArticleSizeCountryCodeId = style.SizeCountryCodeId;
            item.ArticleSizeCountryCode = CodeItem.Get().Where(i => i.Id == style.SizeCountryCodeId && i.LocaleId == style.LocaleId && i.CodeType == "35").Select(i => i.NameTW).Max();
            item.Port = item.ExportPortId == 0 ? "" : Port.Get().Where(i => i.Id == item.ExportPortId && i.LocaleId == item.LocaleId).Select(i => i.PortName).Max();
            item.ARLocaleNo = company.Where(i => i.Id == item.ARLocaleId).Select(i => i.CompanyNo).Max();
            item.OrdersLocaleNo = company.Where(i => i.Id == item.RefOrdersLocaleId).Select(i => i.CompanyNo).Max();

            return new Models.Entities.Orders()
            {
                Id = item.Id,
                OrderDate = item.OrderDate,
                OrderNo = item.OrderNo,
                CustomerId = item.CustomerId,
                ArticleId = item.ArticleId,
                StyleId = item.StyleId,
                OrderType = item.OrderType,
                ProductType = item.ProductType,
                UnitPrice = item.UnitPrice,
                ReferUnitPrice = item.ReferUnitPrice,
                ETD = item.ETD,
                ShippingDate = item.ShippingDate,
                CompanyId = item.CompanyId,
                SizeCountryCodeId = item.OrderSizeCountryCodeId,
                PackingDescTW = item.PackingDescTW,
                PackingDescEng = item.PackingDescEng,
                SafeCode = item.SafeCode,
                BarcodeCodeId = item.BarcodeCodeId,
                Mark = item.Mark,
                SideMark = item.SideMark,
                CustomerOrderNo = item.CustomerOrderNo,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = DateTime.Now,
                LocaleId = item.LocaleId,
                Status = item.Status,
                CSD = item.CSD,
                OrderQty = item.OrderQty,
                PackingType = item.PackingType,
                Mark1Desc = item.LabelDesc,
                // Mark1PhotoURL = item.Mark1PhotoURL,
                // Mark2Desc = item.Mark2Desc,
                // Mark2PhotoURL = item.Mark2PhotoURL,
                // Mark3Desc = item.Mark3Desc,
                // Mark3PhotoURL = item.Mark3PhotoURL,
                Mark4Desc = item.CustomerVendorCode,
                // Mark4PhotoURL = item.Mark4PhotoURL,
                Mark5Desc = item.SpecialDesc,
                // Mark5PhotoURL = item.Mark5PhotoURL,
                DollarCodeId = item.DollarCodeId,
                doMRP = item.doMRP,
                Version = item.Version,
                ProcessSetId = item.ProcessSetId,
                ExportPortId = item.ExportPortId,
                InsockLabel = item.InsockLabel,
                PackingTypeDesc = item.PackingTypeDesc,
                CustomerStyleNo = item.CustomerStyleNo,
                ShoeName = item.ShoeName,
                SpecialNote = item.SpecialNote,
                PayType = item.PayType,
                DeliveryTerms = item.DeliveryTerms,
                TransitType = item.TransitType,
                ToolingFund = item.ToolingFund,
                SpecialPackingStatus = item.SpecialPackingStatus,
                ARCustomerId = item.ARCustomerId,
                IsApproved = item.IsApproved,
                PaymentDate = item.PaymentDate,
                ARLocaleId = item.ARLocaleId,
                ParentOrdersId = item.ParentOrdersId,
                RefOrdersLocaleId = item.RefOrdersLocaleId,
                LCSD = item.LCSD,
                GBSPOReferenceNo = item.GBSPOReferenceNo,
                KeyInDate = item.KeyInDate == null ? DateTime.Now : item.KeyInDate,
                OWD = item.OWD,
                OWRD = item.OWRD,
                RSD = item.RSD,
                GBSCD = item.GBSCD,
                GBSPUD = item.GBSPUD,
                ArticleNo = item.ArticleNo,
                StyleNo = item.StyleNo,
                BrandCodeId = item.BrandCodeId,
                Season = item.Season,
                LocaleNo = item.LocaleNo,
                CompanyNo = item.CompanyNo,
                Customer = item.Customer,
                LastNo = item.LastNo,
                OutsoleNo = item.OutsoleNo,
                ArticleName = item.ArticleName,
                Dollar = item.Dollar,
                Brand = item.Brand,
                OrderSizeCountryCode = item.OrderSizeCountryCode,
                ArticleSizeCountryCodeId = item.ArticleSizeCountryCodeId,
                ArticleSizeCountryCode = item.ArticleSizeCountryCode,
                Port = item.Port,
                ARLocaleNo = item.ARLocaleNo,
                OrdersLocaleNo = item.OrdersLocaleNo,
            };
        }

        public bool CheckOrderNo(string orderNo, int localeId)
        {
            return Orders.Get().Where(i => i.OrderNo == orderNo && i.LocaleId == localeId).Any();
        }
        public OrderItem GetOrderItem(string orderNo, int localeId)
        {
            return Orders.Get()
                .Where(i => i.OrderNo == orderNo && i.LocaleId == localeId)
                .Select(i => new OrderItem
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    Status = i.Status,
                    OrderNo = i.OrderNo,
                    StyleNo = i.StyleNo,
                    ShoeName = i.ShoeName,
                    CompanyId = i.CompanyId,
                })
                .FirstOrDefault();
        }
    }

    public class OrderItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string OrderNo { get; set; }
        public decimal Status { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public decimal CompanyId { get; set; }
    }
}