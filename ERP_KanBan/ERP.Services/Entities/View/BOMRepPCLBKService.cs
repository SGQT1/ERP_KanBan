using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class BOMRepPCLBKService : EntityService<REP_PCLBK>
    {
        protected new RepPCLBKRepository Repository { get { return base.Repository as RepPCLBKRepository; } }

        public BOMRepPCLBKService(RepPCLBKRepository repository) : base(repository)
        {
        }
    }
}