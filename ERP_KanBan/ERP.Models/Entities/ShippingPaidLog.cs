using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ShippingPaidLog
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime PaidDate { get; set; }
        public string PayDollarCodeDesc { get; set; }
        public decimal? ARPaid { get; set; }
        public decimal? AROff { get; set; }
        public string DiffDesc { get; set; }
        public int IsCFM { get; set; }
        public string Confirmer { get; set; }
        public DateTime? ConfirmDate { get; set; }
    }
}
