using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class StyleSizeRunUsageService : EntityService<StyleSizeRunUsage>
    {
        protected new StyleSizeRunUsageRepository Repository { get { return base.Repository as StyleSizeRunUsageRepository; } }

        public StyleSizeRunUsageService(StyleSizeRunUsageRepository repository) : base(repository)
        {
        }
    }
}