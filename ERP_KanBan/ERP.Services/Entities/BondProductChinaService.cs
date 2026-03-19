using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class BondProductChinaService : EntityService<BondProductChina>
    {
        protected new BondProductChinaRepository Repository { get { return base.Repository as BondProductChinaRepository; } }

        public BondProductChinaService(BondProductChinaRepository repository) : base(repository)
        {
        }
    }
}