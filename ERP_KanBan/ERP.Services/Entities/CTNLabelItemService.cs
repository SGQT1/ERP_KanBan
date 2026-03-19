using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System.Linq;

namespace ERP.Services.Entities
{
    public class CTNLabelItemService : EntityService<CTNLabelItem>
    {
        protected new CTNLabelItemRepository Repository { get { return base.Repository as CTNLabelItemRepository; } }

        public CTNLabelItemService(CTNLabelItemRepository repository) : base(repository)
        {
        }
    }
}