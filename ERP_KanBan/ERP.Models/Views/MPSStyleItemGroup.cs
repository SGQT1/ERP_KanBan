using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSStyleItemGroup
    {
        public ERP.Models.Views.MPSStyle MPSStyle { get; set; }
        public ERP.Models.Views.MPSStyleItem MPSStyleItem { get; set; }
        public IEnumerable<ERP.Models.Views.MPSStyleItemUsage> MPSStyleItemUsage { get; set; }
    }
}
