using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class BatchProductionCostService : EntityService<BatchProductionCost>
    {
        protected new BatchProductionCostRepository Repository { get { return base.Repository as BatchProductionCostRepository; } }

        public BatchProductionCostService(BatchProductionCostRepository repository) : base(repository)
        {
        }
    }
}