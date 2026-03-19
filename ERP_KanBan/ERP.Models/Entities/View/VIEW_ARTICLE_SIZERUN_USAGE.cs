using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_ARTICLE_SIZERUN_USAGE
    {
        public decimal ArticleId { get; set; }
        public decimal ArticleSize { get; set; }
        public string ArticleSizeSuffix { get; set; }
        public double ArticleInnerSize { get; set; }
        public decimal KnifeSize { get; set; }
        public string KnifeSizeSuffix { get; set; }
        public double KnifeInnerSize { get; set; }
        public decimal OutsoleSize { get; set; }
        public string OutsoleSizeSuffix { get; set; }
        public double OutsoleInnerSize { get; set; }
        public decimal LastSize { get; set; }
        public string LastSizeSuffix { get; set; }
        public double LastInnerSize { get; set; }
        public decimal ShellSize { get; set; }
        public string ShellSizeSuffix { get; set; }
        public double ShellInnerSize { get; set; }
        public decimal Other1Size { get; set; }
        public string Other1SizeSuffix { get; set; }
        public double Other1InnerSize { get; set; }
        public string Other1Desc { get; set; }
        public decimal Other2Size { get; set; }
        public string Other2SizeSuffix { get; set; }
        public double Other2InnerSize { get; set; }
        public string Other2SizeDesc { get; set; }
        public string ArticleDisplaySize { get; set; }
        public string ShellDisplaySize { get; set; }
        public decimal? ArticleSizeRunId { get; set; }
        public decimal? ArticlePartId { get; set; }
        public decimal? UnitUsage { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? PartId { get; set; }
        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string PartNameEn { get; set; }
    }
}
