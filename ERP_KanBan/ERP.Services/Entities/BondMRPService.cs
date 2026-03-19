using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class BondMRPService : EntityService<BondMRP>
    {
        protected new BondMRPRepository Repository { get { return base.Repository as BondMRPRepository; } }

        public BondMRPService(BondMRPRepository repository) : base(repository)
        {
        }
    }
}