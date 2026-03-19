using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsProcedureVendorItemService : EntityService<MpsProcedureVendorItem>
    {
        protected new MpsProcedureVendorItemRepository Repository { get { return base.Repository as MpsProcedureVendorItemRepository; } }
        public MpsProcedureVendorItemService(MpsProcedureVendorItemRepository repository) : base(repository)
        {
        }
    }
}
