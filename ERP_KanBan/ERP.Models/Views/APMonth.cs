using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class APMonth
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string APNo { get; set; }
        public string YYYYMM { get; set; }
        public string VendorNameTw { get; set; }
        public decimal AP { get; set; }
        public decimal Tax { get; set; }
        public int IsClose { get; set; }
        public decimal PreAPTTL { get; set; }
        public decimal? APTTL { get; set; }
        public decimal APGet { get; set; }
        public string DollarCodeName { get; set; }
        public decimal BankingRate { get; set; }
        public string Remark { get; set; }
        public string ISONo { get; set; }
        public string ReceiveAddress { get; set; }
        public string TelNo { get; set; }
        public string PaymentCodeName { get; set; }
        public decimal Discount { get; set; }
        public decimal? TaxRate { get; set; }
        public int IsTaxCombined { get; set; }
        public string CloseUserName { get; set; }
        public DateTime? CloseTime { get; set; }
        public decimal APGetPre { get; set; }
        public decimal? PaymentLocaleId { get; set; }
        public int? APType { get; set; }
    }
}
