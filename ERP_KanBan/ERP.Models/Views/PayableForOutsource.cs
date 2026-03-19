using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class PayableForOutsource
    {
        public string Vendor { get; set; }
        public decimal VendorId { get; set; }
        public decimal SubAmount { get; set; }
        public decimal? PayDollarCodeId { get; set; }
        public string PayDollar { get; set; }
        public string CloseMonth { get; set; }
        public string CloseMonthFrom { get; set; }
        public string CloseMonthTo { get; set; }
        public decimal PayQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? Discount { get; set; }
        public decimal? APTTL { get; set; }
        public decimal? PlanPayTTL { get; set; }
        public decimal PayRate { get; set; }

        // public decimal DayOfMonth { get; set; }
        // public DateTime? ReceivedDateFrom { get; set; }
        // public DateTime? ReceivedDateTo { get; set; }
        public decimal? ReceivedLocaleId { get; set; } //收貨地
        // public string ReceivedLocale { get; set; }
        public decimal? PaymentLocaleId { get; set; } //付款地
        // public string PaymentLocale { get; set; }
        public decimal? PurLocaleId { get; set; } //下單地
        // public string PurLocale { get; set; }
        public decimal? POLocaleId { get; set; } // PO LocaelId
        public decimal? CompanyId { get; set; } // Orders Company
        // public decimal PayCodeId { get; set; }
        // public string PayType { get; set; }
        // public string Company { get; set; }
    }
}
