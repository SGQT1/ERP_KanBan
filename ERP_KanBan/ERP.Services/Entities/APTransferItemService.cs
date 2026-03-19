using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class APTransferItemService : EntityService<APTransferItem>
    {
        protected new APTransferItemRepository Repository { get { return base.Repository as APTransferItemRepository; } }

        public APTransferItemService(APTransferItemRepository repository) : base(repository)
        {
        }
    }
}