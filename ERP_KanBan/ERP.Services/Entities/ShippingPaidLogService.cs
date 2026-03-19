using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class ShippingPaidLogService : EntityService<ShippingPaidLog>
    {
        protected new ShippingPaidLogRepository Repository { get { return base.Repository as ShippingPaidLogRepository; } }

        public ShippingPaidLogService(ShippingPaidLogRepository repository) : base(repository)
        {
        }
    }
}