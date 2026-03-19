using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsDailyAddService : EntityService<MpsDailyAdd>
    {
        protected new MpsDailyAddRepository Repository { get { return base.Repository as MpsDailyAddRepository; } }
        public MpsDailyAddService(MpsDailyAddRepository repository) : base(repository)
        {
        }
    }
}

