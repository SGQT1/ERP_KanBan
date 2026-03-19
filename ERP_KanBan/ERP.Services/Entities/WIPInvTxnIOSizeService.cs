using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class WIPInvTxnIOSizeService : EntityService<WIPInvTxnIOSize>
    {
        protected new WIPInvTxnIOSizeRepository Repository { get { return base.Repository as WIPInvTxnIOSizeRepository; } }
        public WIPInvTxnIOSizeService(WIPInvTxnIOSizeRepository repository) : base(repository)
        {
        }
    }
}