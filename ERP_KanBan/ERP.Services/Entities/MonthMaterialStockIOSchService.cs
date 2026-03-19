using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class MonthMaterialStockIOSchService : EntityService<MonthMaterialStockIOSch>
    {
        protected new MonthMaterialStockIOSchRepository Repository { get { return base.Repository as MonthMaterialStockIOSchRepository; } }

        public MonthMaterialStockIOSchService(MonthMaterialStockIOSchRepository repository) : base(repository)
        {
        }
    }
}