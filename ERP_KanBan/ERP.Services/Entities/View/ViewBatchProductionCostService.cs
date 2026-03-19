using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class ViewBatchProductionCostService : EntityService<VIEW_BATCHPRODUCTIONCOST>
    {
        protected new ViewBatchProductionCostRepository Repository { get { return base.Repository as ViewBatchProductionCostRepository; } }

        public ViewBatchProductionCostService(ViewBatchProductionCostRepository repository) : base(repository)
        {
        }
    }
}