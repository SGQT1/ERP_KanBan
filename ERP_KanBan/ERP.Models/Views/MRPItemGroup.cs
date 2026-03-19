using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class MRPItemGroup
    {
        public Orders Orders { get; set; }
        public IEnumerable<MRPItem> MRPItem { get; set; }
        public IEnumerable<MRPItemOrders> MRPItemOrders { get; set; }
    }
}
