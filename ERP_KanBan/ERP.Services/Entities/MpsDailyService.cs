using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsDailyService : EntityService<MpsDaily>
    {
        protected new MpsDailyRepository Repository { get { return base.Repository as MpsDailyRepository; } }
        public MpsDailyService(MpsDailyRepository repository) : base(repository)
        {
        }
    }
}

