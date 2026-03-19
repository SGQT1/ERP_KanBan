using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class OutsoleItemService : EntityService<OutsoleItem>
    {
        protected new OutsoleItemRepository Repository { get { return base.Repository as OutsoleItemRepository; } }

        public OutsoleItemService(OutsoleItemRepository repository) : base(repository)
        {
        }
    }
}