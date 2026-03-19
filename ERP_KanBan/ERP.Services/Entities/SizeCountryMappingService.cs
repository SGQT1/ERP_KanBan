using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class SizeCountryMappingService : EntityService<SizeCountryMapping>
    {
        protected new SizeCountryMappingRepository Repository { get { return base.Repository as SizeCountryMappingRepository; } }

        public SizeCountryMappingService(SizeCountryMappingRepository repository) : base(repository)
        {
        }
    }
}