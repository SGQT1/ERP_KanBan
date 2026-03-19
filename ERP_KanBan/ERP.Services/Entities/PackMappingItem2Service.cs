using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class PackMappingItem2Service : EntityService<PackMappingItem2>
    {
        protected new PackMappingItem2Repository Repository { get { return base.Repository as PackMappingItem2Repository; } }

        public PackMappingItem2Service(PackMappingItem2Repository repository) : base(repository)
        {
        }
    }
}
