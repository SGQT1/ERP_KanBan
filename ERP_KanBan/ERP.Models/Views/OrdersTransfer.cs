using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class OrdersTransfer
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal OrdersId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
