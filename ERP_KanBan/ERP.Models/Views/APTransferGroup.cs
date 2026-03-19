using ERP.Models.System.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class APTransferGroup: BaseEntity
    {
        public APTransfer APTransfer { get; set; }
        public IEnumerable<APTransferItem> APTransferItem { get; set; }
    }
}
