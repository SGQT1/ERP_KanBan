using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_VENDOR
    {
        public decimal Id { get; set; }
        public decimal? TypeCodeId { get; set; }
        public string ShortNameTw { get; set; }
        public string NameTw { get; set; }
        public string NameEn { get; set; }
        public string OwnerName { get; set; }
        public string TelNo1 { get; set; }
        public string TelNo2 { get; set; }
        public string FaxNo1 { get; set; }
        public string FaxNo2 { get; set; }
        public decimal? CountryCodeId { get; set; }
        public decimal? AreaCodeId { get; set; }
        public string UnifiedInvoiceNo { get; set; }
        public string CompanyAddressZip { get; set; }
        public string CompanyAddress { get; set; }
        public string BillAddressZip { get; set; }
        public string BillAddress { get; set; }
        public string Contact { get; set; }
        public string ContactMobileNo { get; set; }
        public string ContactEmail { get; set; }
        public string FirstTradeDate { get; set; }
        public string LastTradeDate { get; set; }
        public decimal? TaxCodeId { get; set; }
        public decimal? DollarCodeId { get; set; }
        public decimal? PaymentCodeId { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public int? PaymentPoint { get; set; }
        public string CheckTitle { get; set; }
        public string CheckAddressZip { get; set; }
        public string CheckAddress { get; set; }
        public int DayOfMonth { get; set; }
        public int CloseOff { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string PaymentCodeNameTw { get; set; }
        public string PaymentCodeNameEn { get; set; }
        public string TaxCodeNameTw { get; set; }
        public string TaxCodeNameEn { get; set; }
        public string TypeCodeNameTw { get; set; }
        public string TypeCodeNameEn { get; set; }
    }
}
