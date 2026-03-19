using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class PackMappingService : EntityService<PackMapping>
    {
        protected new PackMappingRepository Repository { get { return base.Repository as PackMappingRepository; } }

        public PackMappingService(PackMappingRepository repository) : base(repository)
        {
        }
    }
}
