using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class APMonthItemDiscountService : EntityService<APMonthItemDiscount>
    {
        protected new APMonthItemDiscountRepository Repository { get { return base.Repository as APMonthItemDiscountRepository; } }

        public APMonthItemDiscountService(APMonthItemDiscountRepository repository) : base(repository)
        {
        }
    }
}