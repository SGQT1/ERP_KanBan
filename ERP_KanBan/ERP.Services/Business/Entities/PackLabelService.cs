using System;
using System.Linq;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class PackLabelService : BusinessService
    {
        private Services.Entities.CTNLabelService CTNLabel { get; }
        private Services.Entities.OrdersService Orders { get; }
        private Services.Business.Entities.OrdersService OrdersView { get; }

        public PackLabelService(
            Services.Entities.CTNLabelService ctnLabelService,
            Services.Entities.OrdersService ordersService,
            Services.Business.Entities.OrdersService ordersViewService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            Orders = ordersService;
            OrdersView = ordersViewService;
            CTNLabel = ctnLabelService;
        }
        public IQueryable<Models.Views.PackLabel> Get()
        {
            return (
                from i in CTNLabel.Get()
                join o in Orders.Get() on new { OrderNo = i.OrderNo } equals new { OrderNo = o.OrderNo }
                select new Models.Views.PackLabel
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    PLLocaleId = i.PLLocaleId,
                    OrderNo = i.OrderNo,
                    ExFactoryDate = i.ExFactoryDate,
                    TransitType = i.TransitType,
                    TargetPort = i.TargetPort,
                    Customer = i.Customer,
                    StyleNo = i.StyleNo,
                    ShoeName = i.ShoeName,
                    ProductType = i.ProductType,
                    MappingSizeCountryNameTw = i.MappingSizeCountryNameTw,
                    CustomerOrderNo = i.CustomerOrderNo,
                    ColorDesc = i.ColorDesc,
                    OutsoleColorDescTW = i.OutsoleColorDescTW,
                    OrderQty = i.OrderQty,
                    PackingQty = i.PackingQty,
                    CTNS = i.CTNS,
                    CloseDate = i.CloseDate,
                    ModifyUserName = i.ModifyUserName,
                    LastUpdateTime = i.LastUpdateTime,
                    IsPrint = i.IsPrint,
                    CNoFrom = i.CNoFrom,

                    OrderId = o.Id,
                    CompanyId = o.CompanyId,
                    BrandCodeId = o.BrandCodeId
                }
            );
        }
        public Models.Views.PackLabel Create(Models.Views.PackLabel packLabel)
        {
            var _packLabel = CTNLabel.Create(Build(packLabel));
            return Get().Where(i => i.Id == _packLabel.Id && i.LocaleId == _packLabel.LocaleId).FirstOrDefault();
        }
        public Models.Views.PackLabel Update(Models.Views.PackLabel packLabel)
        {
            var _packLabel = CTNLabel.Update(Build(packLabel));
            return Get().Where(i => i.Id == _packLabel.Id && i.LocaleId == _packLabel.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.PackLabel packLabel) {
            CTNLabel.Remove(Build(packLabel));

        }
        private Models.Entities.CTNLabel Build(Models.Views.PackLabel item)
        {
            return new Models.Entities.CTNLabel
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                PLLocaleId = item.PLLocaleId,
                OrderNo = item.OrderNo,
                ExFactoryDate = item.ExFactoryDate,
                TransitType = item.TransitType,
                TargetPort = item.TargetPort,
                Customer = item.Customer,
                StyleNo = item.StyleNo,
                ShoeName = item.ShoeName,
                ProductType = item.ProductType,
                MappingSizeCountryNameTw = item.MappingSizeCountryNameTw,
                CustomerOrderNo = item.CustomerOrderNo,
                ColorDesc = item.ColorDesc,
                OutsoleColorDescTW = item.OutsoleColorDescTW,
                OrderQty = item.OrderQty,
                PackingQty = item.PackingQty,
                CTNS = item.CTNS,
                CloseDate = item.CloseDate,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                IsPrint = item.IsPrint,
                CNoFrom = item.CNoFrom,
            };
        }

        public Models.Views.PackLabel GetPackLabelOrders(string orderNo, decimal localeId)
        {
            return (
                from o in OrdersView.Get().Where(i => i.OrderNo == orderNo)
                select new Models.Views.PackLabel
                {
                    // Id = o.Id,
                    Id = 0,
                    LocaleId = localeId,
                    PLLocaleId = localeId,
                    OrderNo = o.OrderNo,
                    ExFactoryDate = o.ETD,
                    TransitType = (int)o.TransitType,
                    TargetPort = o.Port,
                    Customer = o.Customer,
                    StyleNo = o.StyleNo,
                    ShoeName = o.ShoeName,
                    ProductType = o.ProductType,
                    MappingSizeCountryNameTw = o.OrderSizeCountryCode,
                    CustomerOrderNo = o.CustomerOrderNo,
                    ColorDesc = o.RefColorDesc,
                    OutsoleColorDescTW = o.RefOutsoleColorDesc,
                    OrderQty = o.OrderQty,
                    // PackingQty = o.PackingQty,
                    // CTNS = o.CTNS,
                    // CloseDate = o.CloseDate,
                    // ModifyUserName = o.ModifyUserName,
                    // LastUpdateTime = o.LastUpdateTime,
                    IsPrint = 0,
                    CNoFrom = 1,

                    OrderId = o.Id,
                    CompanyId = o.CompanyId,
                    Company = o.CompanyNo,
                    BrandCodeId = o.BrandCodeId,
                    BrandCode = o.Brand,
                    PLLocale = o.CompanyNo,
                    Transit = o.RefTransitType,
                }
            ).FirstOrDefault();
        }
        public IQueryable<Models.Views.PackLabelItem> GetBarcode()
        {
            var items = (
               from h in CTNLabel.Get()
               join o in Orders.Get() on new { OrderNo = h.OrderNo } equals new { OrderNo = o.OrderNo }
               select new Models.Views.PackLabelItem
               {
                   Id = h.Id,
                   LocaleId = h.LocaleId,
                   OrderNo = h.OrderNo,
                   IsPrint = h.IsPrint,
                   PackingQty = h.PackingQty,
                   OrderId = o.Id,
                   CompanyId = o.CompanyId,
                   BrandCodeId = o.BrandCodeId,
                   ExFactoryDate = h.ExFactoryDate
               }
           );
            return items;
        }
    }
}