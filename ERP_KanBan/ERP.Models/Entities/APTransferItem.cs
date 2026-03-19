using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class APTransferItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public decimal APTransferId { get; set; }
        public int IsTransfer { get; set; }
        public string VendorNameTw { get; set; }
        public decimal? APQty { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal SaleAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal TTL { get; set; }
        public int IsIntergrate { get; set; }
        public int IsFromInvoice { get; set; }
        public string PurDollarNameTw { get; set; }
    }
}
