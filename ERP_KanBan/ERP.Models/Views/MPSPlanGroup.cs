using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MPSPlanGroup
    {
        // public MPSOrders MPSOrders { get; set; }
        public MPSPlan MPSPlan { get; set; }
        public IEnumerable<MPSPlan> HasMPSPlan { get; set; }
        public IEnumerable<MPSPlanItem> MPSPlanItem { get; set; }
        public IEnumerable<MPSPlanItemSize> MPSPlanItemSize { get; set; }
        public IEnumerable<MPSOrdersItem> MPSOrdersItem { get; set; }
    }
}
