using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class ArticleSizeRunUsage
    {
        public decimal Id { get; set; }
        public decimal ArticleSizeRunId { get; set; }
        public decimal ArticlePartId { get; set; }
        public decimal UnitUsage { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }

        public decimal ArticleId { get; set; }
        public string PartNameTw { get; set; }
        public decimal ArticleSize { get; set; }
        public string ArticleSizeSuffix { get; set; }
        public double ArticleInnerSize { get; set; }  
        public string ArticleDisplaySize { get; set; } 
        public decimal? StyleId { get; set; }
        public decimal? StyleItemId { get; set; }
        public decimal? PartId { get; set; }
    }
}
