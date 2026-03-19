using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class BondProductChinaContrastService : EntityService<BondProductChinaContrast>
    {
        protected new BondProductChinaContrastRepository Repository { get { return base.Repository as BondProductChinaContrastRepository; } }

        public BondProductChinaContrastService(BondProductChinaContrastRepository repository) : base(repository)
        {
        }
    }
}