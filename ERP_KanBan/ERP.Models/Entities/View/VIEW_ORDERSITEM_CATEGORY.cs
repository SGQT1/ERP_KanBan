using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_ORDERSITEM_CATEGORY
    {
        public string CompanyNo { get; set; }
        public string CSDYM { get; set; }
        public string ETDYM { get; set; }
        public string CategoryNameTw { get; set; }
        public string OrderNo { get; set; }
        public decimal? OrdersId { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? MappingSizeCountryCodeId { get; set; }
        public string MappingSizeCountryNameTw { get; set; }
        public decimal ArticleSize { get; set; }
        public string ArticleSizeSuffix { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal Qty { get; set; }
        public decimal? TransferUnitPrice { get; set; }
        public decimal TransferQty { get; set; }
        public string DisplaySize { get; set; }
        public decimal MixedQty1 { get; set; }
        public decimal MixedQty2 { get; set; }
        public decimal MixedQty3 { get; set; }
        public decimal MixedQty4 { get; set; }
        public decimal MixedQty5 { get; set; }
        public decimal? SizeCountryCodeId { get; set; }
        public string SizeCountryNameTw { get; set; }
        public decimal StyleId { get; set; }
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
        public string ShellDisplaySize { get; set; }
        public decimal Other1Size { get; set; }
        public double ShellInnerSize { get; set; }
        public string Other1SizeSuffix { get; set; }
        public double Other1InnerSize { get; set; }
        public string Other1Desc { get; set; }
        public decimal Other2Size { get; set; }
        public string Other2SizeSuffix { get; set; }
        public double Other2InnerSize { get; set; }
        public string Other2SizeDesc { get; set; }
        public string ArticleDisplaySize { get; set; }
        public decimal UKArticleSize { get; set; }
        public string UKArticleSizeSuffix { get; set; }
    }
}
