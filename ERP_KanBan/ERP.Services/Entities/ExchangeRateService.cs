using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class ExchangeRateService : EntityService<ExchangeRate>
    {
        protected new ExchangeRateRepository Repository { get { return base.Repository as ExchangeRateRepository; } }

        public ExchangeRateService(ExchangeRateRepository repository) : base(repository)
        {
        }
    }
}