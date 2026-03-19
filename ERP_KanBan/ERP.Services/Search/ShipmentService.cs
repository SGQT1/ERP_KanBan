using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;

namespace ERP.Services.Search
{
    public class ShipmentService : SearchService
    {
        private ERP.Services.Business.Entities.InvoiceService Invoice { get; set; }
        public ShipmentService(
            ERP.Services.Business.Entities.InvoiceService invoiceService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Invoice = invoiceService;
        }
        public IQueryable<Models.Views.OrdersShipment> GetOrdersShipment(string predicate)
        {
            return Invoice.GetOrderShipment(predicate);
        }
    }
}