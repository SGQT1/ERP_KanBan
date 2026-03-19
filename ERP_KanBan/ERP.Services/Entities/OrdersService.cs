using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Services.Entities
{
    public class OrdersService : EntityService<Orders>
    {
        protected new OrdersRepository Repository { get { return base.Repository as OrdersRepository; } }

        public OrdersService(OrdersRepository repository) : base(repository)
        {
        }
    }
}
