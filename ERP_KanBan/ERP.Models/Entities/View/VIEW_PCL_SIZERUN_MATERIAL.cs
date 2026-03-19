using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_PCL_SIZERUN_MATERIAL
    {
        public int? AlternateType { get; set; }
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public int EnableMaterial { get; set; }
        public string OrderNo { get; set; }
        public decimal ArticleId { get; set; }
        public decimal StyleId { get; set; }
        public int OrdersVersion { get; set; }
        public decimal OrderQty { get; set; }
        public decimal OrdersItemId { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public string ArticleSizeText { get; set; }
        public decimal OrdersItemQty { get; set; }
        public int StyleVersion { get; set; }
        public string ArticleSizeInSizeRun { get; set; }
        public decimal? KnifeSize { get; set; }
        public string KnifeSizeSuffix { get; set; }
        public double? KnifeInnerSize { get; set; }
        public string KnifeDisplaySize { get; set; }
        public decimal? OutsoleSize { get; set; }
        public string OutsoleSizeSuffix { get; set; }
        public double? OutsoleInnerSize { get; set; }
        public string OutsoleDisplaySize { get; set; }
        public decimal? LastSize { get; set; }
        public string LastSizeSuffix { get; set; }
        public double? LastInnerSize { get; set; }
        public string LastDisplaySize { get; set; }
        public decimal? ShellSize { get; set; }
        public string ShellSizeSuffix { get; set; }
        public double? ShellInnerSize { get; set; }
        public decimal? Other1Size { get; set; }
        public string Other1SizeSuffix { get; set; }
        public double? Other1InnerSize { get; set; }
        public string Other1Desc { get; set; }
        public decimal? Other2Size { get; set; }
        public string Other2SizeSuffix { get; set; }
        public double? Other2InnerSize { get; set; }
        public string Other2SizeDesc { get; set; }
        public string ShellDisplaySize { get; set; }
        public decimal ArticlePartId { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public int? SemiGoods { get; set; }
        public decimal? ParentId { get; set; }
        public decimal? ChildId { get; set; }
        public string MaterialNameTwChild { get; set; }
        public string MaterialNameEnChild { get; set; }
        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string PartNameEn { get; set; }
        public decimal? PartId { get; set; }
        public string UnitCodeNameTw { get; set; }
        public string UnitCodeNameEn { get; set; }
        public decimal? UnitCodeId { get; set; }
        public decimal ArticleSize { get; set; }
        public string ArticleSizeSuffix { get; set; }
        public string DisplaySize { get; set; }
        public decimal? ArticleSizeRunId { get; set; }
        public decimal? UnitUsage { get; set; }
        public double? InnerSizeCalc { get; set; }
    }
}
