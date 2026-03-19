using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSDailyPlan
    {
        public decimal Id { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MPSLiveId { get; set; }
        public string OrderNo { get; set; }
        public decimal MPSProcessId { get; set; }
        public string MPSProcess { get; set; }
        public string StyleNo { get; set; }
        public DateTime PlanDate { get; set; }
        public decimal PlanQty { get; set; }

        public decimal HasDaily { get; set; }
        
    }
}
