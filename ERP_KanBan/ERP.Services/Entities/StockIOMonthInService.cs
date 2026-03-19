using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class StockIOMonthInService : EntityService<StockIOMonthIn>
    {
        protected new StockIOMonthInRepository Repository { get { return base.Repository as StockIOMonthInRepository; } }

        public StockIOMonthInService(StockIOMonthInRepository repository) : base(repository)
        {
        }
    }
}