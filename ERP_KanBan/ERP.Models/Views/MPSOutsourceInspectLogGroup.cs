using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MPSOutsourceInspectLogGroup
    {
        public MPSInspectLog MPSInspectLog { get; set; }
        public IEnumerable<MPSInspectLogSizeItem> MPSInspectLogSizeItem { get; set; }
        public MPSProcedurePO MPSProcedurePO { get; set; }
        public IEnumerable<MPSProcedurePOSize> MPSProcedurePOSize { get; set; }
    }
}
