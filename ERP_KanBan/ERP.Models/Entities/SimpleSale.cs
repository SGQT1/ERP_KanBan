using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class SimpleSale
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal OrdersId { get; set; }
        public decimal RefLocaleId { get; set; }
        public decimal SaleId { get; set; }
        public DateTime? SaleDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public DateTime? OBDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal SaleQty { get; set; }
        public decimal? ShippingId { get; set; }
        public string InvoiceNo { get; set; }
        public string ARCustomerNameTw { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
