using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System.Linq;

namespace ERP.Services.Entities
{
    public class CTNLabelStockOutService : EntityService<CTNLabelStockout>
    {
        protected new CTNLabelStockOutRepository Repository { get { return base.Repository as CTNLabelStockOutRepository; } }

        public CTNLabelStockOutService(CTNLabelStockOutRepository repository) : base(repository)
        {
        }
    }
}