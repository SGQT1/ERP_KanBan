using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class APMonthService : EntityService<APMonth>
    {
        protected new APMonthRepository Repository { get { return base.Repository as APMonthRepository; } }

        public APMonthService(APMonthRepository repository) : base(repository)
        {
        }
    }
}