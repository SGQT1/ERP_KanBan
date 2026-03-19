using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class BatchMaterialCostService : EntityService<BatchMaterialCost>
    {
        protected new BatchMaterialCostRepository Repository { get { return base.Repository as BatchMaterialCostRepository; } }

        public BatchMaterialCostService(BatchMaterialCostRepository repository) : base(repository)
        {
        }
    }
}