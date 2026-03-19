using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsOrdersItemService : EntityService<MpsOrdersItem>
    {
        protected new MpsOrdersItemRepository Repository { get { return base.Repository as MpsOrdersItemRepository; } }
        public MpsOrdersItemService(MpsOrdersItemRepository repository) : base(repository)
        {
        }
    }
}

