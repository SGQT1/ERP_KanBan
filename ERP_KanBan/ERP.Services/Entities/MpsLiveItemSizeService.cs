using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsLiveItemSizeService : EntityService<MpsLiveItemSize>
    {
        protected new MpsLiveItemSizeRepository Repository { get { return base.Repository as MpsLiveItemSizeRepository; } }
        public MpsLiveItemSizeService(MpsLiveItemSizeRepository repository) : base(repository)
        {
        }
    }
}

