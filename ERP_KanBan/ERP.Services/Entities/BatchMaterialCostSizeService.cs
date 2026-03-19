using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class BatchMaterialCostSizeService : EntityService<BatchMaterialCostSize>
    {
        protected new BatchMaterialCostSizeRepository Repository { get { return base.Repository as BatchMaterialCostSizeRepository; } }

        public BatchMaterialCostSizeService(BatchMaterialCostSizeRepository repository) : base(repository)
        {
        }
    }
}