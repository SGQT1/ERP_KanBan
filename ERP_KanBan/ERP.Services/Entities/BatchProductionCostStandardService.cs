using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class BatchProductionCostStandardService : EntityService<BatchProductionCostStandard>
    {
        protected new BatchProductionCostStandardRepository Repository { get { return base.Repository as BatchProductionCostStandardRepository; } }

        public BatchProductionCostStandardService(BatchProductionCostStandardRepository repository) : base(repository)
        {
        }
    }
}