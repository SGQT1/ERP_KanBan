using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsDailyAddPrintLogService : EntityService<MpsDailyAddPrintLog>
    {
        protected new MPSDailyAddPrintLogRepository Repository { get { return base.Repository as MPSDailyAddPrintLogRepository; } }
        public MpsDailyAddPrintLogService(MPSDailyAddPrintLogRepository repository) : base(repository)
        {
        }
    }
}

