using System;
using System.Collections.Generic;
using System.Text;

using ERP.Models.Views;

namespace ERP.Models.Views
{
    public class POGroup
    {
        public PO PO { get; set; }
        public IEnumerable<POItem> POItem { get; set; }
    }
}