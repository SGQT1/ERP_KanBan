using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class WIPInvTxnIOService : EntityService<WIPInvTxnIO>
    {
        protected new WIPInvTxnIORepository Repository { get { return base.Repository as WIPInvTxnIORepository; } }
        public WIPInvTxnIOService(WIPInvTxnIORepository repository) : base(repository)
        {
        }
    }
}