using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_STYLEITEM_PARTSIZE
    {
        public decimal ArticleId { get; set; }
        public string StyleNo { get; set; }
        public decimal? SizeCountryCodeId { get; set; }
        public string SizeCountryNameTw { get; set; }
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
        public string ArticleDisplaySize { get; set; }
        public string ShellDisplaySize { get; set; }
        public decimal? StyleId { get; set; }
        public decimal? ArticlePartId { get; set; }
        public decimal? MaterialId { get; set; }
        public string Remark { get; set; }
        public decimal? LocaleId { get; set; }
        public int? EnableMaterial { get; set; }
        public int Division { get; set; }
        public string DivisionOther { get; set; }
        public decimal PartId { get; set; }
        public decimal UnitCodeId { get; set; }
        public decimal StandardUsage { get; set; }
        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string PartNameEn { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public decimal? CategoryCodeId { get; set; }
        public string CategoryNameTw { get; set; }
        public string CategoryNameEn { get; set; }
        public int AlternateType { get; set; }
        public string UnitNameTw { get; set; }
        public string UnitNameEn { get; set; }
        public string KnifeNo { get; set; }
        public int? PieceOfPair { get; set; }
        public string AlternateTypeSize { get; set; }
        public string MaterialNameVN { get; set; }
        public string PartNameVN { get; set; }
        public string MaterialNameCN { get; set; }
        public string PartNameCN { get; set; }
        public int doMRP { get; set; }
    }
}
