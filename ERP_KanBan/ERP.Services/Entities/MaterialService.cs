using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class MaterialService : EntityService<Material>
    {
        protected new MaterialRepository Repository { get { return base.Repository as MaterialRepository; } }

        public MaterialService(MaterialRepository repository) : base(repository)
        {
        }
    }
}