using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class MPSOrdersGroup
    {
        public MPSOrders MPSOrders { get; set; }
        public IEnumerable<MPSOrdersItem> MPSOrdersItem { get; set; }
    }
}
