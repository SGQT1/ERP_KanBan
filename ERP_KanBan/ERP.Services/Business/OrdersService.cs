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
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Services.Business
{
    public class OrdersService : BusinessService
    {
        private ERP.Services.Business.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Business.Entities.OrdersTDService OrdersTD { get; set; }
        private ERP.Services.Business.Entities.OrdersItemService OrdersItem { get; set; }
        private ERP.Services.Business.Entities.OrdersTransferService OrdersTransfer { get; set; }

        private ERP.Services.Business.PUMAOrdersService PUMAOrders { get; set; }
        private ERP.Services.Business.NBOrdersService NBOrders { get; set; }

        private ERP.Services.Business.Entities.MRPItemOrdersService MRPItemOrders { get; set; }
        private ERP.Services.Business.Entities.MRPQueueService MRPQueue { get; set; }

        private ERP.Services.Business.CacheService Cache { get; set; }

        private readonly IServiceScopeFactory _ServiceScopeFactory;

        public OrdersService(
            ERP.Services.Business.Entities.OrdersService ordersService,
            ERP.Services.Business.Entities.OrdersTDService ordersTDService,
            ERP.Services.Business.Entities.OrdersItemService ordersItemService,
            ERP.Services.Business.Entities.OrdersTransferService ordersTransferService,
            ERP.Services.Business.PUMAOrdersService pumpOrdersService,
            ERP.Services.Business.NBOrdersService nbOrdersService,
            ERP.Services.Business.Entities.MRPQueueService mrpQueueService,
            ERP.Services.Business.Entities.MRPItemOrdersService mRPItemOrders,
            IServiceScopeFactory serviceScopeFactory,  // DI加這行
            ERP.Services.Business.CacheService cacheService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Orders = ordersService;
            OrdersTD = ordersTDService;
            OrdersItem = ordersItemService;
            OrdersTransfer = ordersTransferService;

            PUMAOrders = pumpOrdersService;
            NBOrders = nbOrdersService;

            MRPQueue = mrpQueueService;
            MRPItemOrders = mRPItemOrders;

            Cache = cacheService;
            _ServiceScopeFactory = serviceScopeFactory;
        }
        public ERP.Models.Views.OrdersGroup GetOrdersGroup(int id, int localeId)
        {
            return new OrdersGroup
            {
                Orders = Orders.Get(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault(),
                OrdersItem = OrdersItem.Get().Where(i => i.OrdersId == id & i.LocaleId == localeId).ToList(),
                // OrdersTD = OrdersTD.Get().Where(i => i.OrdersId == id & i.LocaleId == localeId),
                // OrdersTransfer = OrdersTransfer.Get().Where(i => i.OrdersId == id & i.LocaleId == localeId).ToList(),
                // MRPQueueLog = MRPQueue.GetLog().Where(i => i.OrdersId == id & i.LocaleId == localeId).ToList(),
            };
        }
        public ERP.Models.Views.OrdersGroup GetOrdersGroup(string orderNo)
        {
            var order = Orders.Get(i => i.OrderNo == orderNo).FirstOrDefault(); //完全等於
            if (order != null)
            {
                return new OrdersGroup
                {
                    Orders = order,
                    OrdersItem = OrdersItem.Get().Where(i => i.OrdersId == order.Id & i.LocaleId == order.LocaleId).ToList(),
                    // OrdersTD = OrdersTD.Get().Where(i => i.OrdersId == order.Id & i.LocaleId == order.LocaleId),
                    // OrdersTransfer = OrdersTransfer.Get().Where(i => i.OrdersId == order.Id & i.LocaleId == order.LocaleId).ToList(),
                };
            }
            return new OrdersGroup();
        }
        /*
         * step1: when orderId > 0 then check id, if exist to update else create
         * step2: when orderId = 0 then check order no, if exist to update (but will be no states ) else create
         */
        public ERP.Models.Views.OrdersGroup SaveOrdersGroup(OrdersGroup ordersGroup)
        {
            var orders = ordersGroup.Orders;
            var ordersItems = ordersGroup.OrdersItem == null ? new List<OrdersItem>() : ordersGroup.OrdersItem.ToList();
            var ordersTDs = ordersGroup.OrdersTD == null ? new List<OrdersTD>() : ordersGroup.OrdersTD.ToList();
            var ordersTransfers = ordersGroup.OrdersTransfer == null ? new List<OrdersTransfer>() : ordersGroup.OrdersTransfer.ToList();

            try
            {
                UnitOfWork.BeginTransaction();
                if (orders != null)
                {
                    Models.Views.Orders _orders;

                    if (orders.Id > 0)
                    {
                        // Have Id is exsit order
                        _orders = Orders.Get(i => i.Id == orders.Id && i.LocaleId == orders.LocaleId).FirstOrDefault();
                    }
                    else
                    {
                        // Not have id is new order
                        _orders = Orders.Get(i => i.OrderNo == orders.OrderNo && i.LocaleId == orders.LocaleId).FirstOrDefault();
                    }

                    //ordres
                    if (_orders == null)
                    {
                        orders = Orders.Create(orders);
                    }
                    else
                    {
                        orders.Id = _orders.Id;
                        orders.KeyInDate = _orders.KeyInDate;
                        orders.Version = (_orders.Version + 1);
                        orders = Orders.Update(orders);
                    }
                    //ordersItem,ordersTD,ordersTransfer
                    if (orders.Id != 0)
                    {
                        ordersItems.ForEach(i => i.OrdersId = orders.Id);
                        ordersTDs.ForEach(i => i.OrdersId = orders.Id);
                        ordersTransfers.ForEach(i => i.OrdersId = orders.Id);

                        //ordersItem(create,update) is remove OrdersItem and Insert.
                        OrdersItem.RemoveRange(i => i.OrdersId == orders.Id && i.LocaleId == orders.LocaleId);
                        OrdersItem.CreateRange(ordersItems);
                        //ordersTD(create,update) is remove OrdersTD and Insert.
                        OrdersTD.RemoveRange(i => i.OrdersId == orders.Id && i.LocaleId == orders.LocaleId);
                        OrdersTD.CreateRange(ordersTDs);
                        //ordersTD(create,update) is remove OrdersTD and Insert.                        
                        OrdersTransfer.RemoveRange(i => i.OrdersId == orders.Id && i.LocaleId == orders.LocaleId);
                        OrdersTransfer.CreateRange(ordersTransfers);

                        //if order qty less 0, remove Orders Pack Material
                        if (orders.OrderQty <= 0)
                        {
                            MRPItemOrders.RemoveRange(i => i.OrdersId == orders.Id && i.LocaleId == orders.LocaleId);
                        }
                    }
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }

            // Cache.LoadOrdersCache((int)orders.LocaleId);
            return GetOrdersGroup((int)orders.Id, (int)orders.LocaleId);
        }

        public void RemoveOrdersGroup(OrdersGroup ordersGroup)
        {
            var orders = ordersGroup.Orders;
            try
            {
                UnitOfWork.BeginTransaction();
                if (orders != null)
                {
                    // step1: remove OrdersItem
                    OrdersItem.RemoveRange(i => i.OrdersId == orders.Id && i.LocaleId == orders.LocaleId);
                    OrdersTD.RemoveRange(i => i.OrdersId == orders.Id && i.LocaleId == orders.LocaleId);
                    OrdersTransfer.RemoveRange(i => i.OrdersId == orders.Id && i.LocaleId == orders.LocaleId);
                    // step2: remove Orders
                    Orders.Remove(orders);
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
            // Cache.LoadOrdersCache((int)orders.LocaleId);
        }

        public ERP.Models.Views.OrdersGroup GetOrdersGroupFromImport(string ordersNo, int localeId)
        {

            var puma = PUMAOrders.GetOrdersGroup(ordersNo, localeId);
            var nb = NBOrders.GetOrdersGroup(ordersNo, localeId);

            //step 1: Has error Or has Order
            if (puma.ModelState.Code.Length > 0 || puma.Orders != null)
            {
                return puma;
            }
            else if (nb.ModelState.Code.Length > 0 || nb.Orders != null)
            {
                return nb;
            }

            //step 3: cannot find order
            return new OrdersGroup();
        }

        public void ClosedOrders(IEnumerable<ERP.Models.Views.Orders> orders)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                if (orders != null || orders.Count() > 0)
                {
                    var localeId = orders.Select(i => i.LocaleId).FirstOrDefault();
                    var closedIds = orders.Where(i => i.IsClosed == true).Select(i => i.Id);
                    var unClosedIds = orders.Where(i => i.IsClosed == false).Select(i => i.Id);
                    Orders.ClosedOrders((int)localeId, closedIds, unClosedIds);
                }
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
            }
        }
        // get sizerun by ordersId
        public List<ERP.Models.Views.OrdersItem> GetOrdersSizeRun(int id, int localeId)
        {
            // return OrdersItem.Get().Where(i => i.OrdersId == id & i.LocaleId == localeId).ToList();
            return OrdersItem.GetSizeMapping(id, localeId).ToList();
        }

        private void ResetCache(int localeId)
        {
            //非同步更新快取（避免 ObjectDisposed）
            Task.Run(() =>
            {
                using (var scope = _ServiceScopeFactory.CreateScope())
                {
                    try
                    {
                        var scopedCache = scope.ServiceProvider.GetRequiredService<CacheService>();
                        scopedCache.LoadOrdersCache(localeId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("MPSOrder Cache Update Error >>" + ex.Message);
                    }
                }
            });
        }
    }
}
