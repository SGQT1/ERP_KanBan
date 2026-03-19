using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsProcessOrgService : EntityService<MpsProcessOrg>
    {
        protected new MpsProcessOrgRepository Repository { get { return base.Repository as MpsProcessOrgRepository; } }
        public MpsProcessOrgService(MpsProcessOrgRepository repository) : base(repository)
        {
        }
    }
}

