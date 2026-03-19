using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class BOMRepPCLService : EntityService<REP_PCL>
    {
        protected new RepPCLRepository Repository { get { return base.Repository as RepPCLRepository; } }

        public BOMRepPCLService(RepPCLRepository repository) : base(repository)
        {
        }
    }
}