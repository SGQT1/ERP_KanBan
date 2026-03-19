using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsOrdersService : EntityService<MpsOrders>
    {
        protected new MpsOrdersRepository Repository { get { return base.Repository as MpsOrdersRepository; } }
        public MpsOrdersService(MpsOrdersRepository repository) : base(repository)
        {
        }
    }
}

