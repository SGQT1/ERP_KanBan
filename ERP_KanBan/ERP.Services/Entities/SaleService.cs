using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class SaleService : EntityService<Sale>
    {
        protected new SaleRepository Repository { get { return base.Repository as SaleRepository; } }

        public SaleService(SaleRepository repository) : base(repository)
        {
        }
    }
}