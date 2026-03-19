using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_MPSORDERSITEMSIZE
    {
        public decimal MpsOrdersItemId { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsOrdersId { get; set; }
        public string OrderNo { get; set; }
        public decimal ArticleSize { get; set; }
        public string ArticleSizeSuffix { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public string DisplaySize { get; set; }
        public decimal OrderQty { get; set; }
        public decimal Qty { get; set; }
        public decimal MpsStyleItemId { get; set; }
        public string PartNameTw { get; set; }
        public string PartNameEn { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public decimal DailyQty { get; set; }
        public decimal? DailyPlusQty { get; set; }
        public decimal? MpsLiveItemId { get; set; }
        public decimal? LiveQty { get; set; }
    }
}
