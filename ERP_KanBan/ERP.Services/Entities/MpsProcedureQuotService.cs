using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsProcedureQuotService : EntityService<MpsProcedureQuot>
    {
        protected new MpsProcedureQuotRepository Repository { get { return base.Repository as MpsProcedureQuotRepository; } }
        public MpsProcedureQuotService(MpsProcedureQuotRepository repository) : base(repository)
        {
        }
    }
}
