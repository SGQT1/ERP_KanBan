using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsStyleItemUsageService : EntityService<MpsStyleItemUsage>
    {
        protected new MpsStyleItemUsageRepository Repository { get { return base.Repository as MpsStyleItemUsageRepository; } }
        public MpsStyleItemUsageService(MpsStyleItemUsageRepository repository) : base(repository)
        {
        }
    }
}

