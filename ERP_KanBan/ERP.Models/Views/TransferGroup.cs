using ERP.Models.System.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class TransferGroup: BaseEntity
    {
        public Transfer Transfer { get; set; }
        public IEnumerable<TransferItem> TransferItem { get; set; }
    }
}
