using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MRPService : EntityService<MRP>
    {
        protected new MRPRepository Repository { get { return base.Repository as MRPRepository; } }
        public MRPService(MRPRepository repository) : base(repository)
        {
        }
    }
}
