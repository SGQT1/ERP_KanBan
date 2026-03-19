using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_PCL_SIZERUN_STYLE
    {
        public decimal Id { get; set; }
        public string OrderNo { get; set; }
        public decimal ArticleId { get; set; }
        public decimal StyleId { get; set; }
        public decimal OrdersItemId { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? MaterialId { get; set; }
        public decimal? UnitUsage { get; set; }
        public decimal? UnitCodeId { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public decimal? ParentId { get; set; }
        public int? SemiGoods { get; set; }
        public decimal? ChildId { get; set; }
        public string MaterialNameTwChild { get; set; }
        public string MaterialNameEnChild { get; set; }
        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string PartNameEn { get; set; }
        public decimal? OrdersId { get; set; }
        public decimal OrdersItemQty { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? SubUsage { get; set; }
        public decimal PartId { get; set; }
        public string UnitCodeNameTw { get; set; }
        public string UnitCodeNameEn { get; set; }
        public int StyleVersion { get; set; }
    }
}
