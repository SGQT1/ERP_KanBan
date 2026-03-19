using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class ViewBatchProductionCostStandardService : EntityService<VIEW_BATCHPRODUCTIONCOSTStandard>
    {
        protected new ViewBatchProductionCostStandardRepository Repository { get { return base.Repository as ViewBatchProductionCostStandardRepository; } }

        public ViewBatchProductionCostStandardService(ViewBatchProductionCostStandardRepository repository) : base(repository)
        {
        }
    }
}