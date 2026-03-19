using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class REPTest
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid? msrepl_tran_version { get; set; }
        public string F1 { get; set; }
        public string F2 { get; set; }
        public int? F3 { get; set; }
    }
}
