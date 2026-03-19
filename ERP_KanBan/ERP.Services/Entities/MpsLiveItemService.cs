using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsLiveItemService : EntityService<MpsLiveItem>
    {
        protected new MpsLiveItemRepository Repository { get { return base.Repository as MpsLiveItemRepository; } }
        public MpsLiveItemService(MpsLiveItemRepository repository) : base(repository)
        {
        }
    }
}

