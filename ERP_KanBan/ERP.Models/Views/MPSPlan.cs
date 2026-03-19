using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSPlan
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal MPSOrdersId { get; set; }
        public decimal ProcessId { get; set; }
        public string Process { get; set; }

        public string OrderNo { get; set; }
        public decimal OrderQty { get; set; }
        public decimal PlanQty { get; set; }
        public decimal MPSArticleId { get; set; }
        public decimal ProcessSetId { get; set; }
        public string ProcessSet { get; set; }
        public string StyleNo { get; set; }
        public DateTime ETD { get; set; }
        public DateTime CSD { get; set; }
    }
}
