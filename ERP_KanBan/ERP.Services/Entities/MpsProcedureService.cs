using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsProcedureService : EntityService<MpsProcedure>
    {
        protected new MpsProcedureRepository Repository { get { return base.Repository as MpsProcedureRepository; } }
        public MpsProcedureService(MpsProcedureRepository repository) : base(repository)
        {
        }
    }
}
