using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class MaterialStockService : EntityService<MaterialStock>
    {
        protected new MaterialStockRepository Repository { get { return base.Repository as MaterialStockRepository; } }

        public MaterialStockService(MaterialStockRepository repository) : base(repository)
        {
        }
    }
}