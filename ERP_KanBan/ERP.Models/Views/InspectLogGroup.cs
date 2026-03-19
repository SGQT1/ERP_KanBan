using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class InspectLogGroup
    {
        public InspectLog InspectLog { get; set;}
        public ReceivedLog ReceivedLog { get; set; }
        public IEnumerable<InspectLogSizeItem> InspectLogSizeItem { get; set; }
        public POItem POItem { get; set; }
        public SaveReceivedLogOption SaveReceivedLogOption { get; set; }
    }
}
