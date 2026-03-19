using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class MaterialSamplingMethodService : EntityService<MaterialSamplingMethod>
    {
        protected new MaterialSamplingMethodRepository Repository { get { return base.Repository as MaterialSamplingMethodRepository; } }

        public MaterialSamplingMethodService(MaterialSamplingMethodRepository repository) : base(repository)
        {
        }
    }
}