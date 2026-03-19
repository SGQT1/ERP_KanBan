using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class ReceivedLogGroup
    {
        public ReceivedLog ReceivedLog { get; set; }
        public IEnumerable<ReceivedLogSizeItem> ReceivedLogSizeItem { get; set; }
        public POItem POItem { get; set; }
        public IEnumerable<MaterialStockItemPO> MaterialStockItemPO { get; set; }
        public SaveReceivedLogOption SaveReceivedLogOption { get; set; }
        public int? ReceivedType { get; set; }
    }

    public partial class SaveReceivedLogOption {
        public int? ReceivedType { get; set; }
        public bool SaveQCResult { get; set; }
        public bool SaveStockIn { get; set; }
        public bool SaveStockOut { get; set; }
    }
}
