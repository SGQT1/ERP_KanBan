using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class BondMRPItemService : EntityService<BondMRPItem>
    {
        protected new BondMRPItemRepository Repository { get { return base.Repository as BondMRPItemRepository; } }

        public BondMRPItemService(BondMRPItemRepository repository) : base(repository)
        {
        }
    }
}