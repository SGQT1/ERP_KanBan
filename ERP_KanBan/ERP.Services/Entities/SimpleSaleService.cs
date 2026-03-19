using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class SimpleSaleService : EntityService<SimpleSale>
    {
        protected new SimpleSaleRepository Repository { get { return base.Repository as SimpleSaleRepository; } }

        public SimpleSaleService(SimpleSaleRepository repository) : base(repository)
        {
        }
    }
}