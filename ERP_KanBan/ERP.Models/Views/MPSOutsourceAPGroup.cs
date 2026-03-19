using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MPSOutsourceAPGroup
    {
        public MPSAP MPSAP { get; set; }
        public IEnumerable<MPSReceivedLogForAP> MPSReceivedLogForAP { get; set; }
    }
}
