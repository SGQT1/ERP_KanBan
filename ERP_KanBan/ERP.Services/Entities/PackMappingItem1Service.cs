using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class PackMappingItem1Service : EntityService<PackMappingItem1>
    {
        protected new PackMappingItem1Repository Repository { get { return base.Repository as PackMappingItem1Repository; } }

        public PackMappingItem1Service(PackMappingItem1Repository repository) : base(repository)
        {
        }
    }
}
