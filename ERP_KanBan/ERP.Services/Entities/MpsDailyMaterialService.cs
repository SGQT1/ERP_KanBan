using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsDailyMaterialService : EntityService<MpsDailyMaterial>
    {
        protected new MpsDailyMaterialRepository Repository { get { return base.Repository as MpsDailyMaterialRepository; } }
        public MpsDailyMaterialService(MpsDailyMaterialRepository repository) : base(repository)
        {
        }
    }
}

