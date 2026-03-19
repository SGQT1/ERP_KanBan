using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class StockIOMonthSchService : EntityService<StockIOMonthSch>
    {
        protected new StockIOMonthSchRepository Repository { get { return base.Repository as StockIOMonthSchRepository; } }

        public StockIOMonthSchService(StockIOMonthSchRepository repository) : base(repository)
        {
        }
    }
}