using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class MonthMaterialStockIOOutService : EntityService<MonthMaterialStockIOOut>
    {
        protected new MonthMaterialStockIOOutRepository Repository { get { return base.Repository as MonthMaterialStockIOOutRepository; } }

        public MonthMaterialStockIOOutService(MonthMaterialStockIOOutRepository repository) : base(repository)
        {
        }
    }
}