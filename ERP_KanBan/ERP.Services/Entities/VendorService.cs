using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class VendorService : EntityService<Vendor>
    {
        protected new VendorRepository Repository { get { return base.Repository as VendorRepository; } }
        public VendorService(VendorRepository repository) : base(repository)
        {
        }
    }
}