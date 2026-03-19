using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsProcedureGroupService : EntityService<MpsProcedureGroup>
    {
        protected new MpsProcedureGroupRepository Repository { get { return base.Repository as MpsProcedureGroupRepository; } }
        public MpsProcedureGroupService(MpsProcedureGroupRepository repository) : base(repository)
        {
        }
    }
}
