using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class OrdersBKService : BusinessService
    {
        private Services.Entities.OrdersBKService OrdersBK { get; }
        private Services.Entities.CompanyService Company { get; }
        private Services.Entities.MRPRemovedService MRPRemoved { get; }
        
        private Services.Business.Entities.TypeService Type { get; }
        public OrdersBKService(
            Services.Entities.OrdersBKService ordersBKService,
            Services.Entities.CompanyService companyService,
            Services.Entities.MRPRemovedService mrpRemovedService,

            Services.Business.Entities.TypeService typeService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            OrdersBK = ordersBKService;
            Company = companyService;
            MRPRemoved = mrpRemovedService;
            Type = typeService;
        }
        public IQueryable<Models.Views.OrdersBK> Get()
        {
            var ordersBK = (
                from o in OrdersBK.Get()
                select new Models.Views.OrdersBK
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
                    OrderSizeCountryCodeId = o.SizeCountryCodeId,
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
                    Mark1Desc = o.Mark1Desc,
                    Mark1PhotoURL = o.Mark1PhotoURL,
                    Mark2Desc = o.Mark2Desc,
                    Mark2PhotoURL = o.Mark2PhotoURL,
                    Mark3Desc = o.Mark3Desc,
                    Mark3PhotoURL = o.Mark3PhotoURL,
                    Mark4Desc = o.Mark4Desc,
                    Mark4PhotoURL = o.Mark4PhotoURL,
                    Mark5Desc = o.Mark5Desc,
                    Mark5PhotoURL = o.Mark5PhotoURL,
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
                    GBSPUD = o.GBSPUD
                }
            );
            return ordersBK;
        }
        public IQueryable<Models.Views.OrdersBK> ClosedOrders()
        {
            var companies = Company.Get().Select(i => new { Id = i.Id, CompanyNo = i.CompanyNo }).ToList();
            var closeOrdersBK = (
                from o in OrdersBK.Get()
                join r in MRPRemoved.Get() on new { OrdersId =  o.Id, LocaleId = o.LocaleId } equals new { OrdersId =  r.OrdersId, LocaleId = r.LocaleId }  
                select new Models.Views.OrdersBK
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    OrderNo = o.OrderNo,
                    CustomerId = o.CustomerId,
                    ArticleId = o.ArticleId,
                    StyleId = o.StyleId,
                    OrderType = o.OrderType,
                    ProductType = o.ProductType,
                    // UnitPrice = o.UnitPrice,
                    // ReferUnitPrice = o.ReferUnitPrice,
                    ETD = o.ETD,
                    // ShippingDate = o.ShippingDate,
                    CompanyId = o.CompanyId,
                    // SizeCountryCodeId = o.SizeCountryCodeId,
                    // PackingDescTw = o.PackingDescTw,
                    // PackingDescEng = o.PackingDescEng,
                    // SafeCode = o.SafeCode,
                    // BarcodeCodeId = o.BarcodeCodeId,
                    // Mark = o.Mark,
                    // SideMark = o.SideMark,
                    CustomerOrderNo = o.CustomerOrderNo,
                    ModifyUserName = o.ModifyUserName,
                    LastUpdateTime = o.LastUpdateTime,
                    LocaleId = o.LocaleId,
                    Status = o.Status,
                    CSD = o.CSD,
                    OrderQty = o.OrderQty,
                    PackingType = o.PackingType,
                    // Mark1Desc = o.Mark1Desc,
                    // Mark1PhotoUrl = o.Mark1PhotoUrl,
                    // Mark2Desc = o.Mark2Desc,
                    // Mark2PhotoUrl = o.Mark2PhotoUrl,
                    // Mark3Desc = o.Mark3Desc,
                    // Mark3PhotoUrl = o.Mark3PhotoUrl,
                    // Mark4Desc = o.Mark4Desc,
                    // Mark4PhotoUrl = o.Mark4PhotoUrl,
                    // Mark5Desc = o.Mark5Desc,
                    // Mark5PhotoUrl = o.Mark5PhotoUrl,
                    // DollarCodeId = o.DollarCodeId,
                    doMRP = o.doMRP,
                    Version = o.Version,
                    // ProcessSetId = o.ProcessSetId,
                    // ExportPortId = o.ExportPortId,
                    // InsockLabel = o.InsockLabel,
                    // PackingTypeDesc = o.PackingTypeDesc,
                    // CustomerStyleNo = o.CustomerStyleNo,
                    ShoeName = o.ShoeName,
                    // SpecialNote = o.SpecialNote,
                    // PayType = o.PayType,
                    // DeliveryTerms = o.DeliveryTerms,
                    TransitType = o.TransitType,
                    // ToolingFund = o.ToolingFund,
                    // SpecialPackingStatus = o.SpecialPackingStatus,
                    // ArcustomerId = o.ArcustomerId,
                    IsApproved = o.IsApproved,
                    // PaymentDate = o.PaymentDate,
                    ARLocaleId = o.ARLocaleId,
                    // ParentOrdersId = o.ParentOrdersId,
                    // RefOrdersLocaleId = o.RefOrdersLocaleId,
                    LCSD = o.LCSD,
                    GBSPOReferenceNo = o.GBSPOReferenceNo,
                    KeyInDate = o.KeyInDate,
                    OWD = o.OWD,
                    OWRD = o.OWRD,
                    RSD = o.RSD,
                    GBSCD = o.GBSCD,
                    GBSPUD = o.GBSPUD,

                    RefLocale = companies.Where(i => i.Id == o.LocaleId).FirstOrDefault().CompanyNo,
                    RefCompany = companies.Where(i => i.Id == o.CompanyId).FirstOrDefault().CompanyNo,
                    RefApproved = Type.GetBooleanType().Where(i => i.Id == o.IsApproved).Select(i => i.NameTw).FirstOrDefault(),
                    RefdoMRP = Type.GetBooleanType().Where(i => i.Id == o.doMRP).Select(i => i.NameTw).FirstOrDefault(),
                    RefOrdersStatus = Type.GetOrderStatus().Where(i=>i.Id == o.Status).Select(i => i.NameTw).FirstOrDefault(),
                    RefProductType = Type.GetProductType().Where(i => i.Id == o.ProductType).Select(i => i.NameTw).FirstOrDefault(),
                    RefOrderType = Type.GetOrderType().Where(i => i.Id == o.OrderType).Select(i => i.NameTw).FirstOrDefault(),
                    RefPayType = Type.GetPayType().Where(i => i.Id == o.PayType).Select(i => i.NameTw).FirstOrDefault(),
                    RefDeliveryTerm = o.DeliveryTerms,
                    RefPackingType = Type.GetPackingType().Where(i => i.Id == o.PackingType).Select(i => i.NameTw).FirstOrDefault(),
                    RefTransitType = Type.GetTransitType().Where(i => i.Id == o.TransitType).Select(i => i.NameTw).FirstOrDefault(),
                }
            );
            return closeOrdersBK;
        }

    }
}