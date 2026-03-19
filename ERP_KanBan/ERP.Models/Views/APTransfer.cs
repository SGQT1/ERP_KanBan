using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class APTransfer
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string YYYYMM { get; set; }
        public decimal? PaymentLocaleId { get; set; }
        public string PaymentLocale { get; set; }
        public decimal? PurLocaleId { get; set; }
        public string PurLocale { get; set; }
        public string Locale { get; set; }
        // public string Vendor { get; set; }
    }
}
