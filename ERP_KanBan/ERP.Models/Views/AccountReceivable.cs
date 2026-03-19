using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class AccountReceivable
    {
        public decimal LocaleId { get; set; }
        public string InvoiceNo { get; set; }
        public decimal? CustomerId { get; set; }
        public string Customer { get; set; } // CustomerNameTw
        public decimal PayDollarCodeId { get; set; }
        public string PayDollarCodeDesc { get; set; }
        public decimal ARTotal { get; set; }
        public decimal? ARR { get; set; }  //from payment
        public decimal? ARF { get; set; }  //from payment
        public decimal? ARReceived { get; set; } // from invoice
        public decimal? Balance { get; set; }
        public string Remark { get; set; }
        public decimal? CompanyId { get; set; }
        public string Company { get; set; }
        public decimal? BrandId { get; set; }
        public string Brand { get; set; }
        public DateTime? PaidDate { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public int ARStatus { get; set; }
        public string DiffDesc { get; set; }
    }
}