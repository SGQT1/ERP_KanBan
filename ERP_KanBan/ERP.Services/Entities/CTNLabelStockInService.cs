using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System.Linq;

namespace ERP.Services.Entities
{
    public class CTNLabelStockInService : EntityService<CTNLabelStockIn>
    {
        protected new CTNLabelStockInRepository Repository { get { return base.Repository as CTNLabelStockInRepository; } }

        public CTNLabelStockInService(CTNLabelStockInRepository repository) : base(repository)
        {
        }
    }
}