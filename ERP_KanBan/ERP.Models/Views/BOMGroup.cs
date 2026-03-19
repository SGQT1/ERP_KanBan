using System;
using System.Collections.Generic;
using System.Text;
using ERP.Models.Views.View;

namespace ERP.Models.Views
{
    public class BOMGroup
    {
        public BOMOrders BOMOrders { get;set; }
        public IEnumerable<BOMPCL> BOMPCL { get;set; }
        public IEnumerable<BOMItem> BOMItems { get;set; }
    }
}