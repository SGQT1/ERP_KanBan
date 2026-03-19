using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class CBSIn
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal CBSId { get; set; }
        public decimal OurLabor { get; set; }
        public decimal OurOverhead { get; set; }
        public decimal OurProfit { get; set; }
        public decimal OurTTL { get; set; }
        public decimal OurTotalMaterialCost { get; set; }
        public decimal? OurSurfaceRate { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
