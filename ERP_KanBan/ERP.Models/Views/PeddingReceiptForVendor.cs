using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class PeddingReceiptForVendor
    {
        public string Vendor { get; set; }
        public decimal VendorId { get; set; }
        public decimal ReceivedLocaleId { get; set; } //收貨地
        public decimal PaymentLocaleId { get; set; } //付款地
        public decimal PurLocaleId { get; set; } //下單地
        public decimal POLocaleId { get; set; } // PO LocaelId
        public decimal? CompanyId { get; set; } // Orders Company
        public DateTime? PODateFrom { get; set; }
        public DateTime? PODateTo { get; set; }
    }
}
