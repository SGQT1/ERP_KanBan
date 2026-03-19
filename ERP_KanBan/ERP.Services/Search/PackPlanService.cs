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
    public class PackPlanService : SearchService
    {
        private Services.Business.Entities.PackPlanService PackPlan { get; }
        private Services.Business.Entities.OrdersService Orders { get; }
        private Services.Entities.OrdersPLService OrdersPL { get; }
        private Services.Business.Entities.TypeService Type { get; }

        public PackPlanService(
            Services.Business.Entities.PackPlanService packPlanService,
            Services.Business.Entities.OrdersService ordersService,
            Services.Entities.OrdersPLService ordersPLService,
            Services.Business.Entities.TypeService typeService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            PackPlan = packPlanService;
            Orders = ordersService;
            OrdersPL = ordersPLService;
            Type = typeService;
        }

        public IQueryable<Models.Views.PackPlanForOrders> GetPackPlan(string predicate) {
            var packPlan = (
                from o in Orders.GetEntity()
                join op in OrdersPL.Get() on new { OrdersId = o.Id, OrderNo = o.OrderNo } equals
                                           new { OrdersId = (decimal)op.RefOrdersId,  OrderNo = op.OrderNo} into opGRP
                from op in opGRP.DefaultIfEmpty()
                select new Models.Views.PackPlanForOrders
                {
                    HasPL = op == null ? 0 : 1,
                    RefPackPlanId = op.Id,
                    RefPackPlanLocaleId = op.LocaleId,
                    
                    Id = o.Id,
                    LocaleId = o.LocaleId,
                    RefLocaleId = (decimal)op.RefLocaleId,
                    RefOrdersId = (decimal)op.RefOrdersId,
                    OrderNo = o.OrderNo,
                    Edition = op == null ? "" : op.Edition,
                    SizeCountryNameTw = op == null ? "" : op.SizeCountryNameTw,
                    MappingSizeCountryNameTw = op == null ? "" : op.MappingSizeCountryNameTw,
                    PackingQty = op.PackingQty,
                    PackingTypeId = op.PackingType,
                    PackingTypeDesc = op == null ? "" : op.PackingTypeDesc,
                    ModifyUserName = op == null ? "" : op.ModifyUserName,
                    LastUpdateTime = op.LastUpdateTime,
                    CNoFrom = op.CNoFrom,
                    PackingCTNS = op.PackingCTNS,
                    PackingNW = op.PackingNW,
                    PackingGW = op.PackingGW,
                    PackingMEAS = op.PackingMEAS,
                    PackingCBM = op.PackingCBM,

                    RefCustomer = o.Customer,
                    RefCustomerOrderNo = o.CustomerOrderNo,
                    RefSeason = o.Season,
                    RefOrderQty = o.OrderQty,
                    RefArticleId = o.ArticleId,
                    RefArticleNo = o.ArticleNo,
                    RefBrandCodeId = (decimal)o.BrandCodeId,
                    RefBrand = o.Brand,
                    RefStyleId = o.StyleId,
                    RefStyleNo = o.StyleNo,
                    RefCompanyId = o.CompanyId,
                    RefCompany = o.CompanyNo,
                    RefProductTypeId = o.ProductType,
                    // RefProductType = o.RefProductType,
                    RefOrderTypeId = o.OrderType,
                    // RefOrderType = o.RefOrderType,
                    RefPackingType = o.RefPackingType,
                    // RefTransitType = o.RefTransitType,
                    RefTransitTypeId = (int)o.TransitType,
                    RefCSD = o.CSD,
                    RefShoeName = o.ShoeName,
                    RefLCSD = (DateTime)o.LCSD,
                    RefOutsole = o.OutsoleNo,
                    RefLast = o.LastNo,
                    RefOrderDate = o.OrderDate,
                    RefOWD = o.OWD,
                    RefRSD = o.RSD,
                    RefPOCode = o.GBSPOReferenceNo,
                    RefColorDesc = o.RefColorDesc,
                    RefSpecialDesc = o.SpecialDesc,
                    RefSpecialNote = o.SpecialNote,
                    RefCustomerVendorCode = o.CustomerVendorCode,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            packPlan.ForEach(p =>
            {
                p.PackingType = Type.GetPackingType().Where(t => t.Id == p.PackingTypeId).Select(t => t.NameTw).Max();
                p.RefProductType = Type.GetProductType().Where(t => t.Id == p.RefProductTypeId).Select(t => t.NameTw).Max();
                p.RefOrderType = Type.GetOrderType().Where(t => t.Id == p.RefOrderTypeId).Select(t => t.NameTw).Max();
                p.RefPackingType = Type.GetPackingType().Where(t => t.Id == p.PackingTypeId).Select(t => t.NameTw).Max();
                p.RefTransitType = Type.GetTransitType().Where(t => t.Id == p.RefTransitTypeId).Select(t => t.NameTw).Max();
            });
            return packPlan.AsQueryable();
        }
    }
}