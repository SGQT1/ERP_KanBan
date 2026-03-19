using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class OrdersSub
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal OrdersId { get; set; }
        public decimal CustomerId { get; set; }
        public decimal ARCustomerId { get; set; }
        public string CustomerOrderNo { get; set; }
        public decimal SubQty { get; set; }
        public decimal MappingSizeCountryCodeId { get; set; }
        public string TransitVendorName { get; set; }
        public string TransitNo { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
