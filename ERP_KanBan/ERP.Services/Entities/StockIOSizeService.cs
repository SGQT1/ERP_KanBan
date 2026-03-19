using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class StockIOSizeService : EntityService<StockIOSize>
    {
        protected new StockIOSizeRepository Repository { get { return base.Repository as StockIOSizeRepository; } }

        public StockIOSizeService(StockIOSizeRepository repository) : base(repository)
        {
        }
    }
}