using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class InvoiceItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }
        public int FormType { get; set; }
        public int Items { get; set; }
        public decimal SubAmount { get; set; }
        public decimal TaxRate { get; set; }
        public decimal SubTax { get; set; }
        public decimal SubTotalAmount { get; set; }
        public int AccountType { get; set; }
        public int Deduct { get; set; }
        public int InvoiceType { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual Invoice Invoice { get; set; }
    }
}
