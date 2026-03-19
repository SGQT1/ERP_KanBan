using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsDailyMaterialAddService : EntityService<MpsDailyMaterialAdd>
    {
        protected new MpsDailyMaterialAddRepository Repository { get { return base.Repository as MpsDailyMaterialAddRepository; } }
        public MpsDailyMaterialAddService(MpsDailyMaterialAddRepository repository) : base(repository)
        {
        }
    }
}

