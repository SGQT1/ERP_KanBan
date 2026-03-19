using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class StockIOMonthOutService : EntityService<StockIOMonthOut>
    {
        protected new StockIOMonthOutRepository Repository { get { return base.Repository as StockIOMonthOutRepository; } }

        public StockIOMonthOutService(StockIOMonthOutRepository repository) : base(repository)
        {
        }
    }
}