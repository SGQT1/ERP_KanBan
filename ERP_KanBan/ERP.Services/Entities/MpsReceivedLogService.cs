using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsReceivedLogService : EntityService<MpsReceivedLog>
    {
        protected new MpsReceivedLogRepository Repository { get { return base.Repository as MpsReceivedLogRepository; } }
        public MpsReceivedLogService(MpsReceivedLogRepository repository) : base(repository)
        {
        }
    }
}
