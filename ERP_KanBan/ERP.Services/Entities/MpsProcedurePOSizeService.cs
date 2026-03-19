using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsProcedurePOSizeService : EntityService<MpsProcedurePOSize>
    {
        protected new MpsProcedurePOSizeRepository Repository { get { return base.Repository as MpsProcedurePOSizeRepository; } }
        public MpsProcedurePOSizeService(MpsProcedurePOSizeRepository repository) : base(repository)
        {
        }
    }
}
