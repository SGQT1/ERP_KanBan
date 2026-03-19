using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSPlanItem
    {
        public decimal Id { get; set; }
        public decimal MPSLiveId { get; set; }
        public DateTime PlanDate { get; set; }
        public decimal PlanQty { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public int HasSize { get; set; }
        public decimal? SeqId { get; set; }
    }
}
