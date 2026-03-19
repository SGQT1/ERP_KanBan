using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceItem = new HashSet<InvoiceItem>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public int YYYY { get; set; }
        public int MM { get; set; }
        public string VendorNameTw { get; set; }
        public string UnifiedInvoiceNo { get; set; }
        public string TelNo { get; set; }
        public string BillAddressZip { get; set; }
        public string BillAddress { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DefaultTaxRate { get; set; }
        public string Remark { get; set; }
        public string ExtDollarNameTw { get; set; }
        public string IntDollarNameTw { get; set; }
        public decimal ExchangeRate { get; set; }
        public int DefaultInvoiceType { get; set; }
        public int DoReport { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<InvoiceItem> InvoiceItem { get; set; }
    }
}
