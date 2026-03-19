using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsStyleItemService : EntityService<MpsStyleItem>
    {
        protected new MpsStyleItemRepository Repository { get { return base.Repository as MpsStyleItemRepository; } }
        public MpsStyleItemService(MpsStyleItemRepository repository) : base(repository)
        {
        }
    }
}

