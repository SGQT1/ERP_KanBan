using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class REP_PO_B
    {
        public string CompanyNo { get; set; }
        public string CompanyNameTw { get; set; }
        public string CompanyNameEn { get; set; }
        public string VendorNameTw { get; set; }
        public string VendorNameEn { get; set; }
        public string VendorTelNo1 { get; set; }
        public string VendorFaxNo1 { get; set; }
        public string VendorCompanyAddress { get; set; }
        public decimal POId { get; set; }
        public decimal LocaleId { get; set; }
        public decimal VendorId { get; set; }
        public int IsShowSizeRun { get; set; }
        public string BatchNo { get; set; }
        public int IsAllowPartial { get; set; }
        public string PayCodeDesc { get; set; }
        public DateTime PODate { get; set; }
        public DateTime VendorETD { get; set; }
        public string PaymentCompanyShortName { get; set; }
        public string PaymentCompanyShortNameEn { get; set; }
        public decimal PaymentLocaleId { get; set; }
        public decimal ReceivingLocaleId { get; set; }
        public string Remark { get; set; }
        public string MaterialName { get; set; }
        public string MaterialNameEng { get; set; }
        public string PONo { get; set; }
        public string IsAllowPartialDesc { get; set; }
        public decimal Qty { get; set; }
        public decimal MaterialId { get; set; }
        public decimal POItemId { get; set; }
        public string BarcodeNo { get; set; }
        public string ReceivingCompanyNameTw { get; set; }
        public string ReceivingCompanyNameEn { get; set; }
        public string ReceivingCompanyShortName { get; set; }
        public string ReceivingCompanyShortNameEn { get; set; }
        public decimal? UnitPrice { get; set; }
        public string PaymentCompanyNameTw { get; set; }
        public string PaymentCompanyNameEn { get; set; }
        public string PhotoURLDescTw { get; set; }
        public string PhotoURL { get; set; }
        public string POItemRemark { get; set; }
        public decimal UnitCodeId { get; set; }
        public string UnitNameTw { get; set; }
        public string UnitNameEn { get; set; }
        public string DollarNameTw { get; set; }
        public string DollarNameEn { get; set; }
        public decimal? Id { get; set; }
        public string OrderNo { get; set; }
        public decimal? CompanyId { get; set; }
        public DateTime? OrdersETD { get; set; }
        public DateTime? CSD { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public int? FilterId { get; set; }
        public string CompanyShortNameTw { get; set; }
        public string CompanyShortNameEn { get; set; }
        public string Mark { get; set; }
        public string SideMark { get; set; }
        public int Status { get; set; }
        public int? POType { get; set; }
        public string CustomerNameTw { get; set; }
        public DateTime POItemVendorETD { get; set; }
        public string ReferenceCodeNo { get; set; }
    }
}
