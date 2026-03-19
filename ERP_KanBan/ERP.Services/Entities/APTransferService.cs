using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class APTransferService : EntityService<APTransfer>
    {
        protected new APTransferRepository Repository { get { return base.Repository as APTransferRepository; } }

        public APTransferService(APTransferRepository repository) : base(repository)
        {
        }
    }
}