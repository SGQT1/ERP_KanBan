using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MRPItemService : EntityService<MRPItem>
    {
        protected new MRPItemRepository Repository { get { return base.Repository as MRPItemRepository; } }
        public MRPItemService(MRPItemRepository repository) : base(repository)
        {
        }
    }
}
