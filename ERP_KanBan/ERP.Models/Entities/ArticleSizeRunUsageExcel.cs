using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ArticleSizeRunUsageExcel
    {
        public decimal Id { get; set; }
        public string ArticleNo { get; set; }
        public string PartNo { get; set; }
        public decimal ArticleSize { get; set; }
        public string ArticleSizeSuffix { get; set; }
        public decimal UnitUsage { get; set; }
        public decimal ArticleId { get; set; }
        public decimal PartId { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public decimal ArticleSizeRunId { get; set; }
        public decimal ArticlePartId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
