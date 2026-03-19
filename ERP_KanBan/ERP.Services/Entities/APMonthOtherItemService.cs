using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class APMonthOtherItemService : EntityService<APMonthOtherItem>
    {
        protected new APMonthOtherItemRepository Repository { get { return base.Repository as APMonthOtherItemRepository; } }

        public APMonthOtherItemService(APMonthOtherItemRepository repository) : base(repository)
        {
        }
    }
}