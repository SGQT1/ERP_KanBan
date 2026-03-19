using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_PURBATCHITEM_VENDOR
    {
        public decimal CompanyId { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MaterialId { get; set; }
        public decimal PurLocaleId { get; set; }
        public string PODate { get; set; }
        public decimal VendorId { get; set; }
        public decimal ReceivingLocaleId { get; set; }
        public decimal PaymentLocaleId { get; set; }
        public decimal OrdersId { get; set; }
    }
}
