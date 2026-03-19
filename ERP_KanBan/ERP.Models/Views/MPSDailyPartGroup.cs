using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MPSDailyPartGroup
    {
        public IEnumerable<MPSDailyPlan> MPSDailyPlan { get; set; }
        public IEnumerable<MPSDailyPlanItem> MPSDailyPlanItem { get; set; }
    }
    public class MPSDailyBuildGroup
    {
        public decimal MPSDailyPlanId { get; set; }
        public decimal LocaleId { get; set; }
        public IEnumerable<MPSDailyPlanItem> MPSDailyPlanItem { get; set;}
    }
}
