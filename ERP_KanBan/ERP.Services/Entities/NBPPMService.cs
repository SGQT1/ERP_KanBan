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
    public class NBPPMService : EntityService<NBPPM>
    {
        protected new NBPPMRepository Repository { get { return base.Repository as NBPPMRepository; } }

        public NBPPMService(NBPPMRepository repository) : base(repository)
        {
        }
    }
}
