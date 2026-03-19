using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ShippingPaperInv
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal SaleId { get; set; }
        public string MarkInfo { get; set; }
        public string BasicInfo { get; set; }
        public string AmountInfo { get; set; }
        public string OtherInfo { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
