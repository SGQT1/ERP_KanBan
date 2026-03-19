using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Style = new HashSet<Style>();
        }

        public decimal Id { get; set; }
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public string ChineseShortName { get; set; }
        public string OwnerName { get; set; }
        public string TelNo1 { get; set; }
        public string TelNo2 { get; set; }
        public string FaxNo1 { get; set; }
        public string FaxNo2 { get; set; }
        public decimal? CountryCodeId { get; set; }
        public decimal? AreaCodeId { get; set; }
        public decimal? BrandCodeId { get; set; }
        public decimal? DollarCodeId { get; set; }
        public string UnifiedInvoiceNo { get; set; }
        public string CompanyAddressZip { get; set; }
        public string CompanyAddress { get; set; }
        public string InvoiceAddressZip { get; set; }
        public string InvoiceAddress { get; set; }
        public string Contact { get; set; }
        public string ContactMobileNo { get; set; }
        public string ContactEmail { get; set; }
        public decimal? CreditAmount { get; set; }
        public decimal? CreditOverRate { get; set; }
        public string FirstTradeDate { get; set; }
        public string LastTradeDate { get; set; }
        public decimal? InvoiceCodeId { get; set; }
        public decimal? TaxCodeId { get; set; }
        public decimal? PaymentCodeId { get; set; }
        public int? PaymentDays { get; set; }
        public string PackingDescTW { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string Mark { get; set; }
        public string SideMark { get; set; }
        public string SafeCode { get; set; }
        public decimal? BarcodeCodeId { get; set; }
        public int? PriceType { get; set; }
        public decimal? ExportPortId { get; set; }
        public string PackingDescEng { get; set; }
        public decimal LocaleId { get; set; }
        public string DefaultPhotoURL1 { get; set; }
        public string DefaultPhotoURL2 { get; set; }
        public string DefaultPhotoURL3 { get; set; }
        public string DefaultPhotoURL4 { get; set; }
        public int? PayType { get; set; }
        public string DeliveryTerms { get; set; }
        public int? DayOfMonth { get; set; }
        public int? ShipmentType { get; set; }
        public decimal? TaxRate { get; set; }
        public int? IsTaxAdded { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public int IsSpecial { get; set; }

        public virtual ICollection<Style> Style { get; set; }
    }
}
