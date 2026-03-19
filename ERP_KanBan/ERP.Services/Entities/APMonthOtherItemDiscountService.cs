using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class APMonthOtherItemDiscountService : EntityService<APMonthOtherItemDiscount>
    {
        protected new APMonthOtherItemDiscountRepository Repository { get { return base.Repository as APMonthOtherItemDiscountRepository; } }

        public APMonthOtherItemDiscountService(APMonthOtherItemDiscountRepository repository) : base(repository)
        {
        }
    }
}