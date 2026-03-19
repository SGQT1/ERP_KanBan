using System;
using System.Collections.Generic;
using System.Text;

using ERP.Models.Views;

namespace ERP.Models.Views
{
    public class POItemGroup
    {
        public ERP.Models.Views.PO PO { get; set; }
        public ERP.Models.Views.POItem POItem { get; set; }
        public IEnumerable<ERP.Models.Views.POItemSize> POItemSize { get; set; }
        // public IEnumerable<ERP.Models.Views.ProcessPO> ProcessPO { get; set; }
    }
}