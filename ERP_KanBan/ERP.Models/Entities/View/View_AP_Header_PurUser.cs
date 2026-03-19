using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class View_AP_Header_PurUser
    {
        public decimal LocaleId { get; set; }
        public string APNo { get; set; }
        public string YYYYMM { get; set; }
        public string VendorNameTw { get; set; }
        public int IsClose { get; set; }
        public string DollarCodeName { get; set; }
        public string ReceiveAddress { get; set; }
        public int IsTaxCombined { get; set; }
        public decimal? TaxRate { get; set; }
        public string PaymentCodeName { get; set; }
        public string PurUserName { get; set; }
        public decimal Id { get; set; }
        public decimal BankingRate { get; set; }
    }
}
