using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class MonthMaterialStockBatchCostService : EntityService<MonthMaterialStockBatchCost>
    {
        protected new MonthMaterialStockBatchCostRepository Repository { get { return base.Repository as MonthMaterialStockBatchCostRepository; } }

        public MonthMaterialStockBatchCostService(MonthMaterialStockBatchCostRepository repository) : base(repository)
        {
        }
    }
}