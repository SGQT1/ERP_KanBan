using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class APMonthItemService : EntityService<APMonthItem>
    {
        protected new APMonthItemRepository Repository { get { return base.Repository as APMonthItemRepository; } }

        public APMonthItemService(APMonthItemRepository repository) : base(repository)
        {
        }
    }
}