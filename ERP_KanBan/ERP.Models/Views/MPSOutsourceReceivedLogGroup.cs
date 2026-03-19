using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MPSOutsourceReceivedLogGroup
    {
        public MPSReceivedLog MPSReceivedLog { get; set; }
        public IEnumerable<MPSReceivedLogSizeItem> MPSReceivedLogSizeItem { get; set; }
        public MPSProcedurePO MPSProcedurePO { get; set; }
        public IEnumerable<MPSProcedurePOSize> MPSProcedurePOSize { get; set; }
    }
}
