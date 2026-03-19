using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class LastService : EntityService<Last>
    {
        protected new LastRepository Repository { get { return base.Repository as LastRepository; } }

        public LastService(LastRepository repository) : base(repository)
        {
        }
    }
}