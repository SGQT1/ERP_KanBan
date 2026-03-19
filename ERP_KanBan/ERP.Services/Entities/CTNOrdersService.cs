using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System.Linq;

namespace ERP.Services.Entities
{
    public class CTNOrdersService : EntityService<CTNOrders>
    {
        protected new CTNOrdersRepository Repository { get { return base.Repository as CTNOrdersRepository; } }

        public CTNOrdersService(CTNOrdersRepository repository) : base(repository)
        {
        }
    }
}