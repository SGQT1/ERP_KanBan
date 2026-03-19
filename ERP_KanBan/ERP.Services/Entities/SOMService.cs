using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class SOMService : EntityService<SOM>
    {
        protected new SOMRepository Repository { get { return base.Repository as SOMRepository; } }

        public SOMService(SOMRepository repository) : base(repository)
        {
        }
    }
}