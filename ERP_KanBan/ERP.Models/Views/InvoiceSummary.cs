using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class InvoiceSummary
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
    }
}
