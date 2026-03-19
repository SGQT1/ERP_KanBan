using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class View_OUTSOLETC
    {
        public int TCType { get; set; }
        public string OutsoleNo { get; set; }
        public string VendorShortNameTw { get; set; }
        public decimal? TCQty { get; set; }
        public decimal? TC { get; set; }
        public decimal? TCTTL { get; set; }
        public decimal? CBDTC { get; set; }
        public string DollarName { get; set; }
        public decimal? PlusQty { get; set; }
        public decimal? OwnerCompanyId { get; set; }
        public decimal LocaleId { get; set; }
    }
}
