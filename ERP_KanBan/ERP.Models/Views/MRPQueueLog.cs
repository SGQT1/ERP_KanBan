using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MRPQueueLog
    {
        public int? Type { get; set; }
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string BatchNo { get; set; }
        public decimal OrdersId { get; set; }
        public DateTime SubmitTime { get; set; }
        public string Launcher { get; set; }
        public string ReportEmail { get; set; }
        public DateTime NotBeforeTime { get; set; }
        public DateTime NotAfterTime { get; set; }
        public decimal Priority { get; set; }
        public int NotRetryTimes { get; set; }
        public int HasRetriedTimes { get; set; }
        public int HasNotifyTimes { get; set; }
        public DateTime? ProcessTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public int? ProcessStatus { get; set; }
        public string ProcessLog { get; set; }
        
        public decimal? CompanyId { get; set; }
        public DateTime? CSD { get; set; }
        public DateTime? LCSD { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? OWD { get; set; }
        public string? OrderNo { get; set; }
        public decimal? ArticleId { get; set; }
        public decimal? StyleId { get; set; }
        public int? Version { get; set; }
        public string? ShoeName { get; set; }
        public string? ArticleNo { get; set; }
        public string? StyleNo { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal? BrandCodeId { get; set; }
        public string? Season { get; set; }
        public string? Customer { get; set; }
        public string? Company { get; set; }
    }
}
