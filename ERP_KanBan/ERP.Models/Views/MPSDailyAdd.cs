using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSDailyAdd
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string DailyNo { get; set; }
        public string PreDailyNo { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime DailyDate { get; set; }
        public DateTime? FinishedDate { get; set; }
        public decimal ProcessId { get; set; }
        public decimal ProcessUnitId { get; set; }
        public string OrderNo { get; set; }
        public decimal MpsStyleId { get; set; }
        public decimal OrderQty { get; set; }
        public decimal? Qty { get; set; }
        public string SizeCountryNameTw { get; set; }
        public int DailyMode { get; set; }
        public int DailyType { get; set; }
        public int DoDaily { get; set; }
        public DateTime? DoDate { get; set; }
        public decimal Multi { get; set; }
        public string Remark { get; set; }
        public int SeqId { get; set; }
        public decimal? MaterialCost { get; set; }
        public string DollarNameTw { get; set; }
        public int? CostBalance { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public int IsForOrders { get; set; }
        public decimal? MPSDailyId { get; set; }
        public string Part { get; set; }
        public decimal? TotalUsage { get; set; }
        public string ArticleNo { get; set;}
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public DateTime CSD { get; set; }
        public decimal? CompanyId { get; set; }
        public string CompanyNo { get; set; }

        public string ProcessUnitTw { get; set; }
        public decimal? MPSProcessId { get; set; }
        public string MPSProcessNameTw { get; set; }
        public string MPSProcessNameEn { get; set; }
        public decimal? SizeCountryCodeId { get; set; }
    }
}
