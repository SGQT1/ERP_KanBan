using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class MaterialStockSizeService : EntityService<MaterialStockSize>
    {
        protected new MaterialStockSizeRepository Repository { get { return base.Repository as MaterialStockSizeRepository; } }

        public MaterialStockSizeService(MaterialStockSizeRepository repository) : base(repository)
        {
        }
    }
}