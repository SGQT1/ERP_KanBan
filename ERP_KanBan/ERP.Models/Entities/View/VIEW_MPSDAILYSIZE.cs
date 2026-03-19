using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_MPSDAILYSIZE
    {
        public decimal MpsDailyId { get; set; }
        public decimal LocaleId { get; set; }
        public string DisplaySize { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public decimal? SumSubQty { get; set; }
        public int? Counts { get; set; }
        public decimal? AvgSubQty { get; set; }
        public decimal? SumSubMultiUsage { get; set; }
    }
}
