using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    /*
     * MRPItem Service inculde tabels : MRPItem(型體), MRPItemOrders(訂單專用)
     * return MRPItemGroup: Orders,MRPItem, MRPItemOrders
     * GetMRPItemGroupForOrder,SaveMRPItemGroupForOrder is for Packing, retun MRPItemGroup without MRPItem data.
     */
    public class MRPItemService : BusinessService
    {
        private ERP.Services.Business.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Business.Entities.MRPItemService MRPItem { get; set; }
        private ERP.Services.Business.Entities.MRPItemOrdersService MRPItemOrders { get; set; }

        public MRPItemService(
            ERP.Services.Business.Entities.OrdersService ordersService,
            ERP.Services.Business.Entities.MRPItemService mrpItemService,
            ERP.Services.Business.Entities.MRPItemOrdersService mrpItemOrdersService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Orders = ordersService;
            MRPItem = mrpItemService;
            MRPItemOrders = mrpItemOrdersService;
        }
        
        public Models.Views.MRPItemGroup GetMRPItemGroup(int id, int localeId)
        {
            return new MRPItemGroup
            {
                Orders = Orders.Get(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault(),
                MRPItem = MRPItem.Get().Where(i => i.OrdersId == id && i.LocaleId == localeId).ToList(),
                MRPItemOrders = MRPItemOrders.Get().Where(i => i.OrdersId == id && i.LocaleId == localeId).ToList(),
            };
        }
        
        //for MRPItemOrders
        public Models.Views.MRPItemGroup GetMRPItemGroupForOrder(int id, int localeId){
             return new MRPItemGroup
            {
                Orders = Orders.Get(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault(),
                MRPItemOrders = MRPItemOrders.Get().Where(i => i.OrdersId == id && i.LocaleId == localeId).ToList(),
            };
        }
        //for MRPItemOrders
        public Models.Views.MRPItemGroup SaveMRPItemGroupForOrder(MRPItemGroup mrpItemGroup)
        {
            var orders = mrpItemGroup.Orders;
            var mrpItemOrders = mrpItemGroup.MRPItemOrders == null ? new List<MRPItemOrders>() : mrpItemGroup.MRPItemOrders.ToList();
            
            try
            {
                UnitOfWork.BeginTransaction();
                if (orders != null && orders.Id != 0)
                {
                    //mrpItemOrders(create,update) is remove mrpItemOrders and Insert.
                    MRPItemOrders.RemoveRange(i => i.OrdersId == orders.Id && i.LocaleId == orders.LocaleId);
                    MRPItemOrders.CreateRange(mrpItemOrders);
                }
                UnitOfWork.Commit();
                return GetMRPItemGroupForOrder((int)mrpItemGroup.Orders.Id, (int)mrpItemGroup.Orders.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
    }
}
