using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_MPSDAILYMATERIAL
    {
        public decimal LocaleId { get; set; }
        public decimal MpsDailyId { get; set; }
        public decimal MpsStyleItemId { get; set; }
        public decimal? MpsDailyMaterialId { get; set; }
        public decimal? MpsOrdersItemId { get; set; }
        public decimal SubQty { get; set; }
        public decimal UnitUsage { get; set; }
        public decimal? SubUsage { get; set; }
        public decimal Multi { get; set; }
        public decimal? SubMultiUsage { get; set; }
        public string DisplaySize { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public string PartNameTw { get; set; }
        public string PartNameEn { get; set; }
        public int PieceOfPair { get; set; }
        public int AlternateType { get; set; }
        public string RefKnifeNo { get; set; }
        public string PreDailyNo { get; set; }
        public string OrderNo { get; set; }
        public string DailyNo { get; set; }
        public string PartNameCn { get; set; }
        public string PartNameVn { get; set; }
        public DateTime DailyDate { get; set; }
        public decimal MpsLiveId { get; set; }
        public decimal MpsLiveItemId { get; set; }
    }
}
