using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class LastItemService : EntityService<LastItem>
    {
        protected new LastItemRepository Repository { get { return base.Repository as LastItemRepository; } }

        public LastItemService(LastItemRepository repository) : base(repository)
        {
        }
    }
}