using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MRPItemOrdersBKService : EntityService<MRPItemOrdersBK>
    {
        protected new MRPItemOrdersBKRepository Repository { get { return base.Repository as MRPItemOrdersBKRepository; } }
        public MRPItemOrdersBKService(MRPItemOrdersBKRepository repository) : base(repository)
        {
        }
    }
}
