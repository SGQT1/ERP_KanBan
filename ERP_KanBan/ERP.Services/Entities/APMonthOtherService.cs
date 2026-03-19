using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class APMonthOtherService : EntityService<APMonthOther>
    {
        protected new APMonthOtherRepository Repository { get { return base.Repository as APMonthOtherRepository; } }

        public APMonthOtherService(APMonthOtherRepository repository) : base(repository)
        {
        }
    }
}
