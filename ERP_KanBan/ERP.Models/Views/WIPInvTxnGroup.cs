using ERP.Models.System.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class WIPInvTxnGroup: BaseEntity
    {
        public WIPInvTxn WIPInvTxn { get; set; }
        public IEnumerable<WIPInvTxnIO> WIPInvTxnIO { get; set; }
    }
}
