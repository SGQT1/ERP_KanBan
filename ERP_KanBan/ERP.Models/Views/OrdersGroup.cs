using ERP.Models.System.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class OrdersGroup: BaseEntity
    {
        public Orders Orders { get; set; }
        public IEnumerable<OrdersTD> OrdersTD { get; set; }
        public IEnumerable<OrdersItem> OrdersItem { get; set; }
        public IEnumerable<OrdersTransfer> OrdersTransfer { get; set; }
        // public IEnumerable<MRPQueueLog> MRPQueueLog { get; set; }
    }
}
