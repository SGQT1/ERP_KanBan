using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MRPRemovedService : EntityService<MRPRemoved>
    {
        protected new MRPRemovedRepository Repository { get { return base.Repository as MRPRemovedRepository; } }
        public MRPRemovedService(MRPRemovedRepository repository) : base(repository)
        {
        }
    }
}
