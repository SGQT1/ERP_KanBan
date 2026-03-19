using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_ORDERSITEM_SUMMARY2
    {
        public string OrderNo { get; set; }
        public decimal? OrdersId { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? MappingSizeCountryCodeId { get; set; }
        public string MappingSizeCountryNameTw { get; set; }
        public decimal ArticleSize { get; set; }
        public string ArticleSizeSuffix { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public decimal Qty { get; set; }
        public decimal? UnitPrice { get; set; }
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
        public decimal UKArticleSize { get; set; }
        public string UKArticleSizeSuffix { get; set; }
    }
}
