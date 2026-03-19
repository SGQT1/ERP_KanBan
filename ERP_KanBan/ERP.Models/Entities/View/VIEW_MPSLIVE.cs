using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_MPSLIVE
    {
        public decimal Id { get; set; }
        public decimal MpsArticleId { get; set; }
        public DateTime ETD { get; set; }
        public DateTime CSD { get; set; }
        public decimal LocaleId { get; set; }
        public decimal OrderQty { get; set; }
        public decimal Qty { get; set; }
        public decimal IncreaseRate { get; set; }
        public int BaseOn { get; set; }
        public int ProcessType { get; set; }
        public string ArticleNo { get; set; }
        public string StyleNo { get; set; }
        public string CustomerNameTw { get; set; }
        public string LastNo { get; set; }
        public decimal DayCapacity { get; set; }
        public decimal LastTurnover { get; set; }
        public string OrderNo { get; set; }
        public decimal ProcessSetId { get; set; }
        public int? MpsResult { get; set; }
        public decimal CompanyId { get; set; }
        public decimal MpsLiveId { get; set; }
        public decimal ProcessId { get; set; }
        public decimal MpsOrdersId { get; set; }
        public string ProcessNo { get; set; }
        public string ProcessNameTw { get; set; }
        public int RelateCapacity { get; set; }
        public int RelateMaterial { get; set; }
        public int LeadInDays { get; set; }
        public int? DoUsage { get; set; }
        public decimal? SizeCountryCodeId { get; set; }
    }
}
