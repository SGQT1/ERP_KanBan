using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsProcedureGroupItemService : EntityService<MpsProcedureGroupItem>
    {
        protected new MpsProcedureGroupItemRepository Repository { get { return base.Repository as MpsProcedureGroupItemRepository; } }
        public MpsProcedureGroupItemService(MpsProcedureGroupItemRepository repository) : base(repository)
        {
        }
    }
}
