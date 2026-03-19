using System;
using System.Collections.Generic;
using System.Text;

using ERP.Models.Views;

namespace ERP.Models.Views
{
    public class RDPOGroup
    {
        public RDPO RDPO { get; set; }
        public IEnumerable<RDPOItem> RDPOItem { get; set; }
    }
}