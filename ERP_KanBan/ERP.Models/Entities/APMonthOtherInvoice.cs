using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class APMonthOtherInvoice
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public decimal APMonthOtherId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal InvoiceTax { get; set; }
        public decimal InvoiceAmountTTL { get; set; }
        public decimal BankingRate { get; set; }
        public string Remark { get; set; }
        public int IsDraft { get; set; }
        public string VoucherNo { get; set; }

        public virtual APMonthOther APMonthOther { get; set; }
    }
}
