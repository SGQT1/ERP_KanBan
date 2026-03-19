using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsProcedurePOItemService : EntityService<MpsProcedurePOItem>
    {
        protected new MpsProcedurePOItemRepository Repository { get { return base.Repository as MpsProcedurePOItemRepository; } }
        public MpsProcedurePOItemService(MpsProcedurePOItemRepository repository) : base(repository)
        {
        }
    }
}
