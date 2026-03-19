using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class MonthMaterialStockIOInService : EntityService<MonthMaterialStockIOIn>
    {
        protected new MonthMaterialStockIOInRepository Repository { get { return base.Repository as MonthMaterialStockIOInRepository; } }

        public MonthMaterialStockIOInService(MonthMaterialStockIOInRepository repository) : base(repository)
        {
        }
    }
}