using System;

namespace ERP.Models.Views
{
    // 拖外廠商
    public class MPSProcedureVendor
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string VendorNo { get; set; }
        public string ShortNameTw { get; set; }
        public string NameTw { get; set; }
        public string NameEn { get; set; }
        public string NameLocal { get; set; }
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
        public string Contact { get; set; }
        public string ContactMobileNo { get; set; }
        public string ContactEmail { get; set; }
        public decimal? PaymentCodeId { get; set; }
        public int? DayOfMonth { get; set; }
        public int? PaymentPoint { get; set; }
        public string Remark { get; set; }
        public string CheckTitle { get; set; }
        public string CheckAddressZip { get; set; }
        public string CheckAddress { get; set; }
        public int CloseOff { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
