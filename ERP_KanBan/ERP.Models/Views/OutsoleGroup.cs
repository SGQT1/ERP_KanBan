using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class OutsoleGroup
    {
        public Outsole Outsole { get; set; }
        public IEnumerable<OutsoleItem> OutsoleItem { get; set; }
    }
}
