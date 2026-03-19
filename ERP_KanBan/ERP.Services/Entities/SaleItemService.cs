using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class SaleItemService : EntityService<SaleItem>
    {
        protected new SaleItemRepository Repository { get { return base.Repository as SaleItemRepository; } }

        public SaleItemService(SaleItemRepository repository) : base(repository)
        {
        }
    }
}