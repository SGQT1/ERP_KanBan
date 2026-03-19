using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Services.Entities
{
    public class ReceivedLogService : EntityService<ReceivedLog>
    {
        protected new ReceivedLogRepository Repository { get { return base.Repository as ReceivedLogRepository; } }

        public ReceivedLogService(ReceivedLogRepository repository) : base(repository)
        {
        }
    }
}
