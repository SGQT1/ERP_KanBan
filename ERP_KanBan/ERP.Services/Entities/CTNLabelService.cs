using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System.Linq;

namespace ERP.Services.Entities
{
    public class CTNLabelService : EntityService<CTNLabel>
    {
        protected new CTNLabelRepository Repository { get { return base.Repository as CTNLabelRepository; } }

        public CTNLabelService(CTNLabelRepository repository) : base(repository)
        {
        }
    }
}