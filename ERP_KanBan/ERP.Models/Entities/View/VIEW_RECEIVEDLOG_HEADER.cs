using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_RECEIVEDLOG_HEADER
    {
        public decimal LocaleId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string ShortNameTw { get; set; }
        public string NameTw { get; set; }
        public string NameEn { get; set; }
        public string TelNo1 { get; set; }
        public string FaxNo1 { get; set; }
        public string UnifiedInvoiceNo { get; set; }
        public decimal ShippingListVendorId { get; set; }
        public decimal TransferInLocaleId { get; set; }
    }
}
