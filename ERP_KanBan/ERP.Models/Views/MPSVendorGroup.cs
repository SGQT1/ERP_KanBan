using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class MPSOutsourceVendorGroup
    {
        public MPSProcedureVendor MPSProcedureVendor { get; set; }
        public IEnumerable<MPSProcedureVendorItem> MPSProcedureVendorItem { get; set; }
    }
}
