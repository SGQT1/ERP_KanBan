using System;
using System.Collections.Generic;
using System.Text;

using ERP.Models.Views;

namespace ERP.Models.Views
{
    public class PurPlanGroup
    {
        public PurBatch PurBatch { get; set; }
        public IEnumerable<PurOrdersItem> PurOrdersItem { get; set; }
        public IEnumerable<PurBatchItem> PurBatchItem { get; set; }
    }
}