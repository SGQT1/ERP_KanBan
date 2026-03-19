using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class MonthMaterialStockHistoryService : EntityService<MonthMaterialStockHistory>
    {
        protected new MonthMaterialStockHistoryRepository Repository { get { return base.Repository as MonthMaterialStockHistoryRepository; } }

        public MonthMaterialStockHistoryService(MonthMaterialStockHistoryRepository repository) : base(repository)
        {
        }
    }
}