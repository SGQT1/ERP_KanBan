using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MRPItemUsageService : EntityService<MRPItemUsage>
    {
        protected new MRPItemUsageRepository Repository { get { return base.Repository as MRPItemUsageRepository; } }
        public MRPItemUsageService(MRPItemUsageRepository repository) : base(repository)
        {
        }
    }
}
