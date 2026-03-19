using System;
using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Models.Views;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class OrdersStockLocaleService : BusinessService
    {
        private Services.Entities.OrdersStockLocaleService OrdersStockLocale { get; }
        private Services.Entities.OrdersStockLocaleInService OrdersStockLocaleIn { get; }
        public OrdersStockLocaleService(
            Services.Entities.OrdersStockLocaleService ordersStockLocaleService, 
            Services.Entities.OrdersStockLocaleInService ordersStockLocaleInService,
            UnitOfWork unitOfWork):base(unitOfWork)
        {
            OrdersStockLocale = ordersStockLocaleService;
            OrdersStockLocaleIn = ordersStockLocaleInService;
        }
        public IQueryable<Models.Views.OrdersStockLocale> Get()
        {
            return OrdersStockLocale.Get().Select(i => new Models.Views.OrdersStockLocale
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                FactoryNo = i.FactoryNo,
                BlockNo = i.BlockNo,
                AreaNo = i.AreaNo,
                LocaleNo = i.LocaleNo,
                MaxQty = i.MaxQty,
                Barcode = i.Barcode,
                LocaleDesc = i.LocaleDesc,
                Disable = i.Disable,
                Qty = OrdersStockLocaleIn.Get().Where(o => o.LocaleId == i.LocaleId && o.OrdersStockLocaleId == i.Id).Count()
            });
        }
        public Models.Views.OrdersStockLocale Create(Models.Views.OrdersStockLocale item)
        {
            var _item = OrdersStockLocale.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.OrdersStockLocale Update(Models.Views.OrdersStockLocale item)
        {
            var _item = OrdersStockLocale.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.OrdersStockLocale item)
        {
            OrdersStockLocale.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.OrdersStockLocale Build(Models.Views.OrdersStockLocale item)
        {
            return new Models.Entities.OrdersStockLocale()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                FactoryNo = item.FactoryNo,
                BlockNo = item.BlockNo,
                AreaNo = item.AreaNo,
                LocaleNo = item.LocaleNo,
                MaxQty = item.MaxQty,
                Barcode = item.Barcode,
                LocaleDesc = item.LocaleDesc,
                Disable = item.Disable,
            };
        }
        
    }
}