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
    public class PMStockOutService : EntityService<PMStockOut>
    {
        protected new PMStockOutRepository Repository { get { return base.Repository as PMStockOutRepository; } }

        public PMStockOutService(PMStockOutRepository repository) : base(repository)
        {
        }
    }
}
