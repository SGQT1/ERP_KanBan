using ERP.Models.System.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class VendorGroup: BaseEntity
    {
        public Vendor Vendor { get; set; }
        public IEnumerable<VendorItem> VendorItem { get; set; }
        public VendorUseFor VendorUseFor { get; set; }
    }
    public class VendorUseFor {
        public int POCount { get; set;}
        public int QuotationCount { get; set;}
        public int ProjectPOItemCount { get; set;}
    }
}
