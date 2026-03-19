using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class BatchMaterialOrdersService : EntityService<BatchMaterialOrders>
    {
        protected new BatchMaterialOrdersRepository Repository { get { return base.Repository as BatchMaterialOrdersRepository; } }

        public BatchMaterialOrdersService(BatchMaterialOrdersRepository repository) : base(repository)
        {
        }
    }
}