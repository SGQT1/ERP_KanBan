using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class MaterialStockBatchCostService : EntityService<MaterialStockBatchCost>
    {
        protected new MaterialStockBatchCostRepository Repository { get { return base.Repository as MaterialStockBatchCostRepository; } }

        public MaterialStockBatchCostService(MaterialStockBatchCostRepository repository) : base(repository)
        {
        }
    }
}