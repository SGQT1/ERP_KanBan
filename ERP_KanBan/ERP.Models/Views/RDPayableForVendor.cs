using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class RDPayableForVendor
    {   public decimal LocaleId { get; set; }
        public int Type { get; set; }
        
        public decimal VendorId { get; set; }
        public string VendorNameTw { get; set; }

        public decimal? SubAmount { get; set; }
        public decimal? SubAPAmount { get; set; }
        public decimal? SubDiscount { get; set; }
        public string DollarNameTw { get; set; }
        public decimal PaymentLocaleId { get; set; }

        public DateTime? ReceivedDateFrom { get; set; }
        public DateTime? ReceivedDateTo { get; set; }
        public string CloseMonthFrom { get; set; }
        public string CloseMonthTo { get; set; }

    }
}
