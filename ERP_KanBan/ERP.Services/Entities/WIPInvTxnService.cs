using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class WIPInvTxnService : EntityService<WIPInvTxn>
    {
        protected new WIPInvTxnRepository Repository { get { return base.Repository as WIPInvTxnRepository; } }
        public WIPInvTxnService(WIPInvTxnRepository repository) : base(repository)
        {
        }
    }
}