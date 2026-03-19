using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class MaterialQuotService : EntityService<MaterialQuot>
    {
        protected new MaterialQuotRepository Repository { get { return base.Repository as MaterialQuotRepository; } }

        public MaterialQuotService(MaterialQuotRepository repository) : base(repository)
        {
        }
    }
}