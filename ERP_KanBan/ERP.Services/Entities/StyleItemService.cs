using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class StyleItemService : EntityService<StyleItem>
    {
        protected new StyleItemRepository Repository { get { return base.Repository as StyleItemRepository; } }

        public StyleItemService(StyleItemRepository repository) : base(repository)
        {
        }
    }
}