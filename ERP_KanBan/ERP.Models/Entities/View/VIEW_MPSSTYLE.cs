using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_MPSSTYLE
    {
        public decimal Id { get; set; }
        public decimal MpsArticleId { get; set; }
        public string StyleNo { get; set; }
        public decimal LocaleId { get; set; }
        public decimal SizeCountryCodeId { get; set; }
        public int DoUsage { get; set; }
        public string RefOrderNo { get; set; }
        public string BrandTw { get; set; }
        public string ArticleNo { get; set; }
        public string ShoeName { get; set; }
        public string OutsoleNo { get; set; }
        public string LastNo { get; set; }
        public string KnifeNo { get; set; }
        public string ShellNo { get; set; }
        public decimal DayCapacity { get; set; }
        public decimal LastTurnover { get; set; }
    }
}
