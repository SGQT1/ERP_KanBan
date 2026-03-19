using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class ShipmentLogService : EntityService<ShipmentLog>
    {
        protected new ShipmentLogRepository Repository { get { return base.Repository as ShipmentLogRepository; } }

        public ShipmentLogService(ShipmentLogRepository repository) : base(repository)
        {
        }
    }
}