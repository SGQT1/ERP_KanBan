using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class KnifeGroup
    {
        public Knife Knife { get; set; }
        public IEnumerable<KnifeItem> KnifeItem { get; set; }
    }
}
