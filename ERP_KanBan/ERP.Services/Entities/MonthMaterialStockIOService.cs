using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class MonthMaterialStockIOService : EntityService<MonthMaterialStockIO>
    {
        protected new MonthMaterialStockIORepository Repository { get { return base.Repository as MonthMaterialStockIORepository; } }

        public MonthMaterialStockIOService(MonthMaterialStockIORepository repository) : base(repository)
        {
        }
    }
}