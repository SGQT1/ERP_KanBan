using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Models.Views;
using ERP.Services.Business.Entities;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    public class OrdersStockLocaleService : BusinessService
    {
        private ERP.Services.Business.Entities.OrdersStockLocaleService OrdersStockLocale { get; set; }
        public OrdersStockLocaleService(
            ERP.Services.Business.Entities.OrdersStockLocaleService ordersStockLocaleService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            OrdersStockLocale = ordersStockLocaleService;
        }

        public ERP.Models.Views.OrdersStockLocale Get(int id, int localeId)
        {
            return OrdersStockLocale.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
        }
        public ERP.Models.Views.OrdersStockLocale Save(OrdersStockLocale item)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                // Id >> exist, ChineseName >> duplicate
                var _item = OrdersStockLocale.Get().Where(i => i.LocaleId == item.LocaleId && i.Id == item.Id).FirstOrDefault();

                if (_item != null)
                {
                    item.Id = _item.Id;
                    item = OrdersStockLocale.Update(item);
                }
                else
                {
                    item.Barcode = item.LocaleId + "*" + DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
                    item = OrdersStockLocale.Create(item);
                }

                UnitOfWork.Commit();
                return Get((int)item.Id, (int)item.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public void Remove(OrdersStockLocale item)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                OrdersStockLocale.Remove(item);
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
    }
}
