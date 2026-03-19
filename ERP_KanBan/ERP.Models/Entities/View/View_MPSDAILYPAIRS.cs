using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class View_MPSDAILYPAIRS
    {
        public string DailyNo { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? SumSubMultiUsage { get; set; }
    }
}
