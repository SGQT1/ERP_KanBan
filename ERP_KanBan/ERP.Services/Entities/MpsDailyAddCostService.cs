using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsDailyAddCostService : EntityService<MpsDailyAddCost>
    {
        protected new MpsDailyAddCostRepository Repository { get { return base.Repository as MpsDailyAddCostRepository; } }
        public MpsDailyAddCostService(MpsDailyAddCostRepository repository) : base(repository)
        {
        }
    }
}

