using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsDailyMaterialItemAddService : EntityService<MpsDailyMaterialItemAdd>
    {
        protected new MpsDailyMaterialItemAddRepository Repository { get { return base.Repository as MpsDailyMaterialItemAddRepository; } }
        public MpsDailyMaterialItemAddService(MpsDailyMaterialItemAddRepository repository) : base(repository)
        {
        }
    }
}

