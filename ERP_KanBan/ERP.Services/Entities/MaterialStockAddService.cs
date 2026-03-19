using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class MaterialStockAddService : EntityService<MaterialStockAdd>
    {
        protected new MaterialStockAddRepository Repository { get { return base.Repository as MaterialStockAddRepository; } }

        public MaterialStockAddService(MaterialStockAddRepository repository) : base(repository)
        {
        }
    }
}