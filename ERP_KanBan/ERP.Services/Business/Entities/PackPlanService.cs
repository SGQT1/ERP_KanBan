using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class PackPlanService : BusinessService
    {
        private Services.Business.Entities.OrdersService Orders { get; }
        private Services.Entities.OrdersPLService OrdersPL { get; }
        private Services.Business.Entities.TypeService Type { get; }

        private Services.Entities.OrdersPackInvoiceService OrdersPackInvoice {get;}

        public PackPlanService(
            Services.Business.Entities.OrdersService ordersService,
            Services.Entities.OrdersPLService ordersPLService,
            Services.Entities.OrdersPackInvoiceService ordersPackInvoiceService,
            Services.Business.Entities.TypeService typeService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Orders = ordersService;
            OrdersPL = ordersPLService;
            Type = typeService;

            OrdersPackInvoice = ordersPackInvoiceService;
        }
        public IQueryable<Models.Views.PackPlan> Get()
        {
            var packPlan = (
                from o in Orders.Get()
                join op in OrdersPL.Get() on new { OrdersId = o.Id, OrderNo = o.OrderNo } equals
                                           new { OrdersId = (decimal)op.RefOrdersId,  OrderNo = op.OrderNo}                                
                select new Models.Views.PackPlan
                {
                    Id = op.Id,
                    LocaleId = op.LocaleId,
                    RefLocaleId = (decimal)op.RefLocaleId,
                    RefOrdersId = (decimal)op.RefOrdersId,
                    OrderNo = o.OrderNo,
                    Edition = op.Edition,
                    SizeCountryNameTw = op.SizeCountryNameTw,
                    MappingSizeCountryNameTw = op.MappingSizeCountryNameTw,
                    PackingQty = op.PackingQty,
                    PackingTypeId = op.PackingType,
                    // PackingType = Type.GetPackingType().Where(i => i.Id == op.PackingType).Select(i => i.NameTw).Max(),
                    PackingType = op.PackingType == 0 ? "單號裝" : "混裝",
                    PackingTypeDesc = op.PackingTypeDesc,
                    ModifyUserName = op.ModifyUserName,
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
                    RefProductType = o.RefProductType,
                    RefOrderTypeId = o.OrderType,
                    RefOrderType = o.RefOrderType,
                    // RefPackingType = o.RefPackingType,
                    RefPackingType = o.PackingType == 0 ? "單號裝" : "混裝",
                    RefTransitType = o.RefTransitType,
                    RefTransitTypeId = (int)o.TransitType,
                    RefCSD = o.CSD,
                    RefShoeName = o.ShoeName,
                    RefLCSD = (DateTime)o.LCSD,
                    RefOutsole = o.OutsoleNo,
                    RefLast = o.LastNo,
                    RefOrderDate = o.OrderDate,
                }
            );
            return packPlan;
        }
        public IQueryable<Models.Views.PackPlan> GetWithInvoce()
        {
            var packPlan = (
                from op in Get()
                join pi in OrdersPackInvoice.Get() on new { OrderNo = op.OrderNo, Edition = op.Edition, LocaleId = op.LocaleId } 
                                    equals new { OrderNo = pi.OrderNo, Edition = pi.Edition, LocaleId = pi.LocaleId } into piGRP
                from pi in piGRP.DefaultIfEmpty()                                    
                select new Models.Views.PackPlan
                {
                    Id = op.Id,
                    LocaleId = op.LocaleId,
                    RefLocaleId = (decimal)op.RefLocaleId,
                    RefOrdersId = (decimal)op.RefOrdersId,
                    OrderNo = op.OrderNo,
                    Edition = op.Edition,
                    SizeCountryNameTw = op.SizeCountryNameTw,
                    MappingSizeCountryNameTw = op.MappingSizeCountryNameTw,
                    PackingQty = op.PackingQty,
                    PackingTypeId = op.PackingTypeId,
                    PackingType = op.PackingType,
                    PackingTypeDesc = op.PackingTypeDesc,
                    ModifyUserName = op.ModifyUserName,
                    LastUpdateTime = op.LastUpdateTime,
                    CNoFrom = op.CNoFrom,
                    PackingCTNS = op.PackingCTNS,
                    PackingNW = op.PackingNW,
                    PackingGW = op.PackingGW,
                    PackingMEAS = op.PackingMEAS,
                    PackingCBM = op.PackingCBM,

                    RefCustomer = op.RefCustomer,
                    RefCustomerOrderNo = op.RefCustomerOrderNo,
                    RefSeason = op.RefSeason,
                    RefOrderQty = op.RefOrderQty,
                    RefArticleId = op.RefArticleId,
                    RefArticleNo = op.RefArticleNo,
                    RefBrandCodeId = op.RefBrandCodeId,
                    RefBrand = op.RefBrand,
                    RefStyleId = op.RefStyleId,
                    RefStyleNo = op.RefStyleNo,
                    RefCompanyId = op.RefCompanyId,
                    RefCompany = op.RefCompany,
                    RefProductTypeId = op.RefProductTypeId,
                    RefProductType = op.RefProductType,
                    RefOrderTypeId = op.RefOrderTypeId,
                    RefOrderType = op.RefOrderType,
                    RefPackingType = op.RefPackingType,
                    RefTransitType = op.RefTransitType,
                    RefTransitTypeId = op.RefTransitTypeId,
                    RefCSD = op.RefCSD,
                    RefShoeName = op.RefShoeName,
                    RefLCSD = op.RefLCSD,
                    RefOutsole = op.RefOutsole,
                    RefLast = op.RefLast,
                    RefOrderDate = op.RefOrderDate,
                    InvoiceNo = pi.InvoiceNo,
                }
            );
            return packPlan;
        }

        public Models.Views.PackPlan Create(Models.Views.PackPlan packPlan)
        {
            var _packPlan = OrdersPL.Create(Build(packPlan));
            return Get().Where(i => i.Id == _packPlan.Id).FirstOrDefault();
        }
        public Models.Views.PackPlan Update(Models.Views.PackPlan packPlan)
        {
            var _packPlan = OrdersPL.Update(Build(packPlan));
            return Get().Where(i => i.Id == _packPlan.Id).FirstOrDefault();
        }
        public void Remove(Models.Views.PackPlan packPlan)
        {
            OrdersPL.Remove(Build(packPlan));
        }
        public void UpdateOrdersPackingType(Models.Views.PackPlan packPlan)
        {
            Orders.UpdatePackingType((int)packPlan.RefOrdersId, (int)packPlan.LocaleId, packPlan.PackingTypeId);
        }
        private Models.Entities.OrdersPL Build(Models.Views.PackPlan item)
        {
            return new Models.Entities.OrdersPL
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                RefLocaleId = item.RefLocaleId,
                RefOrdersId = item.RefOrdersId,
                OrderNo = item.OrderNo,
                Edition = item.Edition,
                SizeCountryNameTw = item.SizeCountryNameTw,
                MappingSizeCountryNameTw = item.MappingSizeCountryNameTw,
                PackingQty = item.PackingQty,
                PackingType = item.PackingTypeId,
                PackingTypeDesc = item.PackingTypeDesc,
                CNoFrom = item.CNoFrom,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = DateTime.Now,

                PackingCTNS = item.PackingCTNS,
                PackingNW = item.PackingNW,
                PackingGW = item.PackingGW,
                PackingMEAS = item.PackingMEAS,
                PackingCBM = item.PackingCBM,
            };
        }
    }
}