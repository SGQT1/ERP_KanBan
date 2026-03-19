
using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class BOMRepOrdersBKService : EntityService<REP_ORDERSBK>
    {
        protected new RelOrdersBKRepository Repository { get { return base.Repository as RelOrdersBKRepository; } }

        public BOMRepOrdersBKService(RelOrdersBKRepository repository) : base(repository)
        {
        }
    }
}