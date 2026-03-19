using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class BondMaterialChinaService : EntityService<BondMaterialChina>
    {
        protected new BondMaterialChinaRepository Repository { get { return base.Repository as BondMaterialChinaRepository; } }

        public BondMaterialChinaService(BondMaterialChinaRepository repository) : base(repository)
        {
        }
    }
}