using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;

namespace ERP.Services.Search
{
    public class MaterialQuotService : SearchService
    {
        private ERP.Services.Business.Entities.MaterialQuotService MaterialQuotation { get; set; }
        public MaterialQuotService(
            ERP.Services.Business.Entities.MaterialQuotService materialQuotation,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MaterialQuotation = materialQuotation;
        }
        public IQueryable<Models.Views.MaterialQuot> GetMaterialQuotation()
        {
             return MaterialQuotation.Get();
        }
    }
}