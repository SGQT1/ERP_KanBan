using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class PCLBKService : EntityService<PCLBK>
    {
        protected new PCLBKRepository Repository { get { return base.Repository as PCLBKRepository; } }

        public PCLBKService(PCLBKRepository repository) : base(repository)
        {
        }
    }
}
