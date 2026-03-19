using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class VendorItemService : EntityService<VendorItem>
    {
        protected new VendorItemRepository Repository { get { return base.Repository as VendorItemRepository; } }
        public VendorItemService(VendorItemRepository repository) : base(repository)
        {
        }
    }
}