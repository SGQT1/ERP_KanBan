using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class StockIOService : EntityService<StockIO>
    {
        protected new StockIORepository Repository { get { return base.Repository as StockIORepository; } }

        public StockIOService(StockIORepository repository) : base(repository)
        {
        }
    }
}