using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class StyleService : EntityService<Style>
    {
        protected new StyleRepository Repository { get { return base.Repository as StyleRepository; } }

        public StyleService(StyleRepository repository) : base(repository)
        {
        }
    }
}