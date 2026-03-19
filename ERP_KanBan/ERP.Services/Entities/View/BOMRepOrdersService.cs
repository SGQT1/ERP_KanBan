
using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class BOMRepOrdersService : EntityService<REP_ORDERS>
    {
        protected new RelOrdersRepository Repository { get { return base.Repository as RelOrdersRepository; } }

        public BOMRepOrdersService(RelOrdersRepository repository) : base(repository)
        {
        }
    }
}