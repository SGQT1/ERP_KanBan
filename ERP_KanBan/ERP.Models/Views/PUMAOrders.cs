using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class PUMAOrders
    {        
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string CustomerName { get; set; }
        public string StyleNo { get; set; }
        public string OrderNo { get; set; }
        public DateTime? CSD { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? LCSD { get; set; }
        public string Season { get; set; }
    }
}
