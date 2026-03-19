using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class ShippingService : EntityService<Shipping>
    {
        protected new ShippingRepository Repository { get { return base.Repository as ShippingRepository; } }

        public ShippingService(ShippingRepository repository) : base(repository)
        {
        }
    }
}