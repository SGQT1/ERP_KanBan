using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsReceivedLogSizeItemService : EntityService<MpsReceivedLogSizeItem>
    {
        protected new MpsReceivedLogSizeItemRepository Repository { get { return base.Repository as MpsReceivedLogSizeItemRepository; } }
        public MpsReceivedLogSizeItemService(MpsReceivedLogSizeItemRepository repository) : base(repository)
        {
        }
    }
}
