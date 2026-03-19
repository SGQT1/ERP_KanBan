using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class StyleSizeRunUsage
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal ArticleSizeRunId { get; set; }
        public decimal StyleId { get; set; }
        public decimal OrdersId { get; set; }
        public decimal MaterialId { get; set; }
        public decimal UnitUsage { get; set; }
        public decimal UnitCodeId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public string MaterialName { get; set; }
        public string UnitCode { get; set; }
        
        public decimal? ArticleSize { get; set; }
        public string? ArticleSizeSuffix { get; set; }
        public double?  ArticleInnerSize { get; set; }  
        public string? ArticleDisplaySize { get; set; } 
        public decimal? StyleItemId { get; set; }
        public string? StyleNo { get; set; }
    }
}
