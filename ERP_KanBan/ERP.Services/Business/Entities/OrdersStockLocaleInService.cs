using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Models.Views;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class OrdersStockLocaleInService : BusinessService
    {
        private Services.Entities.OrdersService Orders { get; }
        private Services.Entities.OrdersStockLocaleInService OrdersStockLocaleIn { get; }
        private Services.Entities.OrdersStockLocaleInLogService OrdersStockLocaleInLog { get; }
        public OrdersStockLocaleInService(
            Services.Entities.OrdersService ordersService,
            Services.Entities.OrdersStockLocaleInService ordersStockLocaleService,
            Services.Entities.OrdersStockLocaleInLogService ordersStockLocaleInLogService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            Orders = ordersService;
            OrdersStockLocaleIn = ordersStockLocaleService;
            OrdersStockLocaleInLog = ordersStockLocaleInLogService;
        }
        public IQueryable<Models.Views.OrdersStockLocaleIn> Get()
        {
            var result = (
                from os in OrdersStockLocaleIn.Get()
                join o in Orders.Get() on new { OrderNo = os.OrderNo, LocaleId = os.LocaleId } equals new { OrderNo = o.OrderNo, LocaleId = o.LocaleId } into oGRP
                from o in oGRP.DefaultIfEmpty()
                select new Models.Views.OrdersStockLocaleIn
                {
                    Id = os.Id,
                    LocaleId = os.LocaleId,
                    ModifyUserName = os.ModifyUserName,
                    LastUpdateTime = os.LastUpdateTime,
                    OrdersStockLocaleId = os.OrdersStockLocaleId,
                    OrdersStockLocaleCode = os.OrdersStockLocaleCode,
                    CTNLabelId = os.CTNLabelId,
                    CTNLabelCode = os.CTNLabelCode,
                    StockInTime = os.StockInTime,
                    Remark = os.Remark,
                    Version = os.Version,
                    OrderNo = os.OrderNo,
                    SeqNo = os.SeqNo,
                    SubLabelCode = os.SubLabelCode,
                    CustomerOrderNo = o.CustomerOrderNo,
                    Customer = o.Customer
                }
            );
            return result;
            // return OrdersStockLocaleIn.Get().Select(i => new Models.Views.OrdersStockLocaleIn
            // {
            //     Id = i.Id,
            //     LocaleId = i.LocaleId,
            //     ModifyUserName = i.ModifyUserName,
            //     LastUpdateTime = i.LastUpdateTime,
            //     OrdersStockLocaleId = i.OrdersStockLocaleId,
            //     OrdersStockLocaleCode = i.OrdersStockLocaleCode,
            //     CTNLabelId = i.CTNLabelId,
            //     CTNLabelCode = i.CTNLabelCode,
            //     StockInTime = i.StockInTime,
            //     Remark = i.Remark,
            //     Version = i.Version,
            //     OrderNo = i.OrderNo,
            //     SeqNo = i.SeqNo,
            // });
        }
        public Models.Views.OrdersStockLocaleIn Create(Models.Views.OrdersStockLocaleIn item)
        {
            var _item = OrdersStockLocaleIn.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.OrdersStockLocaleIn Update(Models.Views.OrdersStockLocaleIn item)
        {
            var _item = OrdersStockLocaleIn.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.OrdersStockLocaleIn item)
        {
            OrdersStockLocaleIn.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.OrdersStockLocaleIn Build(Models.Views.OrdersStockLocaleIn item)
        {
            return new Models.Entities.OrdersStockLocaleIn()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,

                OrdersStockLocaleId = item.OrdersStockLocaleId,
                OrdersStockLocaleCode = item.OrdersStockLocaleCode,
                CTNLabelId = item.CTNLabelId,
                CTNLabelCode = item.CTNLabelCode,
                StockInTime = item.StockInTime,
                Remark = item.Remark,
                Version = item.Version,
                OrderNo = item.OrderNo,
                SeqNo = item.SeqNo,
                SubLabelCode = item.SubLabelCode,
            };
        }

        public void CreateRange(IEnumerable<Models.Views.OrdersStockLocaleIn> ordersItems)
        {
            OrdersStockLocaleIn.CreateRange(BuildRange(ordersItems));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.OrdersStockLocaleIn, bool>> predicate)
        {
            OrdersStockLocaleIn.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.OrdersStockLocaleIn> BuildRange(IEnumerable<Models.Views.OrdersStockLocaleIn> items)
        {
            var stockIntiem = DateTime.Now;
            return items.Select(item => new ERP.Models.Entities.OrdersStockLocaleIn
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,

                OrdersStockLocaleId = item.OrdersStockLocaleId,
                OrdersStockLocaleCode = item.OrdersStockLocaleCode,
                CTNLabelId = item.CTNLabelId,
                CTNLabelItemId = item.CTNLabelItemId,
                CTNLabelCode = item.CTNLabelCode,
                StockInTime = stockIntiem,
                Remark = item.Remark,
                Version = item.Version,
                OrderNo = item.OrderNo,
                SeqNo = item.SeqNo,
                SubLabelCode = item.SubLabelCode,
            });
        }
    }
}