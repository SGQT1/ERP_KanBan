using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class MPSProcessOrgGroup
    {
        public ERP.Models.Views.MPSProcess MPSProcess { get; set; }
        public IEnumerable<ERP.Models.Views.MPSProcessOrg> MPSProcessOrg { get; set; }
    }
}
