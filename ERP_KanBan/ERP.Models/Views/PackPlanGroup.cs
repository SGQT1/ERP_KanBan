using System;
using System.Collections.Generic;
using System.Text;

using ERP.Models.Views;

namespace ERP.Models.Views
{
    public class PackPlanGroup
    {
        public PackPlan PackPlan { get; set; }
        public IEnumerable<PackPlanItem> PackPlanItem { get; set; }
        public IEnumerable<PackSizeItem> PackSizeItem { get; set; }
        public PackPlanSummary PackPlanSummary { get; set; }
    }
}