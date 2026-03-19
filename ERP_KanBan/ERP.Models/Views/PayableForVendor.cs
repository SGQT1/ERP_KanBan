using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class PayableForVendor
    {
        public string Vendor { get; set; }
        public decimal VendorId { get; set; }
        public decimal ReceivedLocaleId { get; set; } //收貨地
        // public string ReceivedLocale { get; set; }
        public decimal PaymentLocaleId { get; set; } //付款地
        // public string PaymentLocale { get; set; }
        public decimal PurLocaleId { get; set; } //下單地
        // public string PurLocale { get; set; }
        public decimal POLocaleId { get; set; } // PO LocaelId
        public decimal? CompanyId { get; set; } // Orders Company
        // public string Company { get; set; }
        public decimal SubAmount { get; set; }
        public decimal PayCodeId { get; set; }
        public string PayType { get; set; }
        public decimal PayDollarCodeId { get; set; }
        public string PayDollar { get; set; }
        public decimal DayOfMonth { get; set; }
        public DateTime? ReceivedDateFrom { get; set; }
        public DateTime? ReceivedDateTo { get; set; }
        public string CloseMonthFrom { get; set; }
        public string CloseMonthTo { get; set; }
    }
}
