using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class BondMaterialChinaContrastService : EntityService<BondMaterialChinaContrast>
    {
        protected new BondMaterialChinaContrastRepository Repository { get { return base.Repository as BondMaterialChinaContrastRepository; } }

        public BondMaterialChinaContrastService(BondMaterialChinaContrastRepository repository) : base(repository)
        {
        }
    }
}