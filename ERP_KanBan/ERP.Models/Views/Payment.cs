using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class Payment
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime PaidDate { get; set; }
        public string PayDollarCodeDesc { get; set; }
        public decimal? ARPaid { get; set; }
        public decimal? AROff { get; set; }
        public string DiffDesc { get; set; }
        public int IsCFM { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }

        public decimal InvoiceId { get; set; }
        public decimal? CustomerId { get; set; }
        public string Customer { get; set; }
        public decimal? CompanyId { get; set; }
        public string Company { get; set; }
        public decimal? BrandId { get; set; }
        public string Brand { get; set; }
        public decimal? ARTotal { get; set; }
        public decimal? ARReceived { get; set; }
        public string Confirmer { get; set; }
        public DateTime? ConfirmDate { get; set; }
    }
}
