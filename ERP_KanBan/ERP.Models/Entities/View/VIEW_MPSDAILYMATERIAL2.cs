using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_MPSDAILYMATERIAL2
    {
        public string DailyNo { get; set; }
        public decimal OrgUnitId { get; set; }
        public DateTime DailyDate { get; set; }
        public string OrderNo { get; set; }
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
