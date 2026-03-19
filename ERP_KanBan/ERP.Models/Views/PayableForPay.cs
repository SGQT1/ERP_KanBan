using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class PayableForPay
    {
        public decimal APMonthId { get; set; }
        public string CloseMonth { get; set; }
        public string Vendor { get; set; }
        public string PayDollar { get; set; }
        public decimal? PaymentLocaleId { get; set; } //付款地
        // public string PaymentLocale { get; set; }
        public decimal? PurLocaleId { get; set; } //下單地
        // public string PurLocale { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? CompanyId { get; set; } // Orders Company
        // public string Company { get; set; }

        public string WarehouseNo { get; set; }

        public decimal? AP { get; set; }
        public decimal? PreAPTTL { get; set; }
        public decimal? APTTL { get; set; }
        public decimal? APGetPre { get; set; }
        public decimal? APGet { get; set; }
        public decimal? TaxRate { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Discount { get; set; }
        public decimal PayRate { get; set; }
        public string CloseMonthFrom { get; set; }
        public string CloseMonthTo { get; set; }
        public int APType { get; set; }
        public int IsDiscount { get; set; }
    }
}
