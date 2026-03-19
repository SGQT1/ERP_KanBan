using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsDailyPrintLogService : EntityService<MpsDailyPrintLog>
    {
        protected new MpsDailyPrintLogRepository Repository { get { return base.Repository as MpsDailyPrintLogRepository; } }
        public MpsDailyPrintLogService(MpsDailyPrintLogRepository repository) : base(repository)
        {
        }
    }
}

