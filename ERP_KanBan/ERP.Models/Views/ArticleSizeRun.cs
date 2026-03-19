using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class ArticleSizeRun
    {
        public decimal Id { get; set; }
        public decimal ArticleId { get; set; }
        public decimal ArticleSize { get; set; }
        public string ArticleSizeSuffix { get; set; }
        public double ArticleInnerSize { get; set; }
        public decimal KnifeSize { get; set; }
        public string KnifeSizeSuffix { get; set; }
        public double KnifeInnerSize { get; set; }
        public string KnifeDisplaySize { get; set; }
        public decimal OutsoleSize { get; set; }
        public string OutsoleSizeSuffix { get; set; }
        public double OutsoleInnerSize { get; set; }
        public string OutsoleDisplaySize { get; set; }
        public decimal LastSize { get; set; }
        public string LastSizeSuffix { get; set; }
        public double LastInnerSize { get; set; }
        public string LastDisplaySize { get; set; }
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
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public string ArticleDisplaySize { get; set; }
        public string ShellDisplaySize { get; set; }

        // public string ArticleNo { get; set; }
        // public string ArticleName { get; set; }
        // public decimal BrandCodeId { get; set; }
        // public decimal? RefStyleId { get; set; }
        // public decimal? RefStyleNo { get; set; }
    }
}
