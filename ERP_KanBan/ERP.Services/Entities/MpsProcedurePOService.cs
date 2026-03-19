using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsProcedurePOService : EntityService<MpsProcedurePO>
    {
        protected new MpsProcedurePORepository Repository { get { return base.Repository as MpsProcedurePORepository; } }
        public MpsProcedurePOService(MpsProcedurePORepository repository) : base(repository)
        {
        }
    }
}
