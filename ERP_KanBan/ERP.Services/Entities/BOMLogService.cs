using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class BOMLogService : EntityService<BOMLog>
    {
        protected new BOMLogRepository Repository { get { return base.Repository as BOMLogRepository; } }

        public BOMLogService(BOMLogRepository repository) : base(repository)
        {
        }
    }
}