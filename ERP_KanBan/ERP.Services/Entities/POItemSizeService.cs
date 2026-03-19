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
    public class POItemSizeService : EntityService<POItemSize>
    {
        protected new POItemSizeRepository Repository { get { return base.Repository as POItemSizeRepository; } }

        public POItemSizeService(POItemSizeRepository repository) : base(repository)
        {
        }
    }
}
