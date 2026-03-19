using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class OrdersPackInvoice
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string OrderNo { get; set; }
        public string Edition { get; set; }
        public string InvoiceNo { get; set; }
        public decimal PLQty { get; set; }
        public int DoPL { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
