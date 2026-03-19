using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class PackInvoice
    {
        public decimal? Id { get; set; }
        public decimal? LocaleId { get; set; }
        public string OrderNo { get; set; }
        public string Edition { get; set; }
        public string InvoiceNo { get; set; }
        public decimal? PLQty { get; set; }
        public int? DoPL { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public DateTime? InvoiceDate { get; set; }

        public decimal PackPlanId { get; set; }
        public decimal PackPlanLocaleId { get; set; }
        public DateTime? CSD { get; set; }
        public decimal? PackingQty { get; set; }
        public decimal? PackingCTNS { get; set; }
        public decimal? PackingNW { get; set; }
        public decimal? PackingGW { get; set; }
        public decimal? PackingMEAS { get; set; }
        public decimal? PackingCBM { get; set; }
        public string StyleNo { get; set; }
        public string Customer { get; set; }
        public string CustomerOrderNo { get; set; }
        public bool HasInvoice { get; set; }
    }
}
