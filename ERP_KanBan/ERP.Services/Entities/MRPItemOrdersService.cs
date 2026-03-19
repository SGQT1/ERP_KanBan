using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MRPItemOrdersService : EntityService<MRPItemOrders>
    {
        protected new MRPItemOrdersRepository Repository { get { return base.Repository as MRPItemOrdersRepository; } }
        public MRPItemOrdersService(MRPItemOrdersRepository repository) : base(repository)
        {
        }
    }
}
