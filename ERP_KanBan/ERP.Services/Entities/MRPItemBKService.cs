using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MRPItemBKService : EntityService<MRPItemBK>
    {
        protected new MRPItemBKRepository Repository { get { return base.Repository as MRPItemBKRepository; } }
        public MRPItemBKService(MRPItemBKRepository repository) : base(repository)
        {
        }
    }
}
