using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_MPSDAILY
    {
        public decimal LocaleId { get; set; }
        public decimal Id { get; set; }
        public string DailyNo { get; set; }
        public string PreDailyNo { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime DailyDate { get; set; }
        public DateTime? FinishedDate { get; set; }
        public string OrderNo { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public decimal UnitCodeId { get; set; }
        public string UnitNameTw { get; set; }
        public string UnitNameEn { get; set; }
        public decimal OrgUnitId { get; set; }
        public string OrgUnitNameTw { get; set; }
        public string OrgUnitNameEn { get; set; }
        public string OrgUnitNameCn { get; set; }
        public string OrgUnitNameVn { get; set; }
        public decimal Qty { get; set; }
        public int DailyMode { get; set; }
        public int DoDaily { get; set; }
        public DateTime? DoDate { get; set; }
        public decimal MpsLiveItemId { get; set; }
        public decimal Multi { get; set; }
        public string Remark { get; set; }
        public string DailyModeNameTw { get; set; }
        public int DailyType { get; set; }
        public string DailyTypeNameTw { get; set; }
        public int? DailyTimes { get; set; }
        public string MaterialNameCn { get; set; }
        public string MaterialNameVn { get; set; }
        public string UnitNameCn { get; set; }
        public string UnitNameVn { get; set; }
        public string StyleNo { get; set; }
        public string ArticleNo { get; set; }
        public string ShoeName { get; set; }
        public string OutsoleNo { get; set; }
        public string LastNo { get; set; }
        public string KnifeNo { get; set; }
        public string ShellNo { get; set; }
        public int IsForOrders { get; set; }
        public decimal? TotalUsage { get; set; }
    }
}
