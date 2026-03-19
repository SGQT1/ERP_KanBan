using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class ShipmentSizeService : BusinessService
    {
        private ERP.Services.Business.Entities.OrdersService Orders;
        private ERP.Services.Business.Entities.OrdersItemService OrdersItem;
        private ERP.Services.Business.Entities.QuotationService Quotation;

        public ShipmentSizeService(
            ERP.Services.Business.Entities.OrdersService ordersService,
            ERP.Services.Business.Entities.OrdersItemService ordersItemService,
            ERP.Services.Business.Entities.QuotationService quotationService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Orders = ordersService;
            OrdersItem = ordersItemService;
            Quotation = quotationService;
        }
    }
}