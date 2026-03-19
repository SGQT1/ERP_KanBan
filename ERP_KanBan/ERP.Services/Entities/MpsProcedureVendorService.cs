using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsProcedureVendorService : EntityService<MpsProcedureVendor>
    {
        protected new MpsProcedureVendorRepository Repository { get { return base.Repository as MpsProcedureVendorRepository; } }
        public MpsProcedureVendorService(MpsProcedureVendorRepository repository) : base(repository)
        {
        }
    }
}
