using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views.Report
{
    public partial class MPSBatch
    {
        public string Report { get; set; }
        public string Company { get; set; }
        public string PlanId { get; set; }
        public string PlanNo { get; set; }
        public string StyleNo { get; set; }
        public string ArticleNo { get; set; }
        public string PrintTime { get; set; }
        public string PrintBy { get; set; }
        public string PrintCount { get; set; }
        public string OrgUnit { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public List<MPSBatchDaily> MPSBatchDaily { get; set; }
    }
    public partial class MPSBatchDaily
    {
        public string MPSDailyId { get; set; }
        public string DailyNo { get; set; }
        public string OrderNo { get; set; }
        public string PlanQty { get; set; }
        public string CSD { get; set; }
        public string LCSD { get; set; }
        public string PlanDate { get; set; }
        public string Part { get; set; }
        public string Barcode { get; set; }
        public List<MPSBatchDailyItem> MPSBatchDailyItem { get; set; }
    }
    public partial class MPSBatchDailyItem
    {
        public string Size { get; set; }
        public decimal Usage { get; set; }
        public decimal InnerSize { get; set; }
    }
}
