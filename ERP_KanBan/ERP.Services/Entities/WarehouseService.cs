using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class WarehouseService : EntityService<Warehouse>
    {
        protected new WarehouseRepository Repository { get { return base.Repository as WarehouseRepository; } }
        public WarehouseService(WarehouseRepository repository) : base(repository)
        {
        }
    }
}