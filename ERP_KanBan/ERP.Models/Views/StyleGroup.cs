using ERP.Models.System.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class StyleGroup
    {
        public Models.Views.Style Style { get; set; }
        public IEnumerable<Models.Views.StyleItem> StyleItem { get; set; }
        public StyleUseFor StyleUseFor { get; set; }
    }
    public class StyleUseFor {
        public int OrdersCount { get; set;}
    }
}
