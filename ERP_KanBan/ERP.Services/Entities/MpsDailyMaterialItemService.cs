using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsDailyMaterialItemService : EntityService<MpsDailyMaterialItem>
    {
        protected new MpsDailyMaterialItemRepository Repository { get { return base.Repository as MpsDailyMaterialItemRepository; } }
        public MpsDailyMaterialItemService(MpsDailyMaterialItemRepository repository) : base(repository)
        {
        }
    }
}

