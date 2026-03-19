using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class PaymentSummary
    {
        public decimal LocaleId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? LastPaidDate { get; set; }
        public decimal? ARPaidTotal { get; set; }
        public decimal? AROffTotal { get; set; }
    }
}
