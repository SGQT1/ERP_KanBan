using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class StockIOYMService : EntityService<StockIOYM>
    {
        protected new StockIOYMRepository Repository { get { return base.Repository as StockIOYMRepository; } }

        public StockIOYMService(StockIOYMRepository repository) : base(repository)
        {
        }
    }
}