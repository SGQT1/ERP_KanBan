using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class QueueDoMRPLogNew
    {
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
        public DateTime ProcessTime { get; set; }
        public DateTime FinishTime { get; set; }
        public DateTime NotifyTime { get; set; }
        public int? ProcessStatus { get; set; }
        public string ProcessLog { get; set; }
    }
}
