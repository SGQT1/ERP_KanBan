using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MRP
    {
        public decimal Id { get; set; }
        public decimal OrdersId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNo { get; set; }
        public decimal CustomerId { get; set; }
        public decimal ArticleId { get; set; }
        public decimal StyleId { get; set; }
        public int OrderType { get; set; }
        public int ProductType { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? ReferUnitPrice { get; set; }
        public DateTime ETD { get; set; }
        public DateTime? ShippingDate { get; set; }
        public decimal CompanyId { get; set; }
        public decimal? SizeCountryCodeId { get; set; }
        public string PackingDescTW { get; set; }
        public string PackingDescEng { get; set; }
        public string SafeCode { get; set; }
        public decimal? BarcodeCodeId { get; set; }
        public string Mark { get; set; }
        public string SideMark { get; set; }
        public string CustomerOrderNo { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public int Status { get; set; }
        public DateTime CSD { get; set; }
        public decimal OrderQty { get; set; }
        public int PackingType { get; set; }
        public string Mark1Desc { get; set; }
        public string Mark1PhotoURL { get; set; }
        public string Mark2Desc { get; set; }
        public string Mark2PhotoURL { get; set; }
        public string Mark3Desc { get; set; }
        public string Mark3PhotoURL { get; set; }
        public string Mark4Desc { get; set; }
        public string Mark4PhotoURL { get; set; }
        public string Mark5Desc { get; set; }
        public string Mark5PhotoURL { get; set; }
        public decimal MixedBoxes1 { get; set; }
        public decimal MixedBoxes2 { get; set; }
        public decimal MixedBoxes3 { get; set; }
        public decimal MixedBoxes4 { get; set; }
        public decimal MixedBoxes5 { get; set; }
        public decimal? DollarCodeId { get; set; }
        public int doMRP { get; set; }
        public int Version { get; set; }
        public decimal? ProcessSetId { get; set; }
        public decimal ExportPortId { get; set; }
        public string InsockLabel { get; set; }
        public string PackingTypeDesc { get; set; }
        public string CustomerStyleNo { get; set; }
        public string ShoeName { get; set; }
        public string SpecialNote { get; set; }
        public int? PayType { get; set; }
        public string DeliveryTerms { get; set; }
        public int? TransitType { get; set; }
        public decimal? ToolingFund { get; set; }
        public int SpecialPackingStatus { get; set; }
        public decimal ARCustomerId { get; set; }
        public int IsApproved { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? ARLocaleId { get; set; }
        public decimal? ParentOrdersId { get; set; }
        public decimal? RefOrdersLocaleId { get; set; }
        public DateTime? LCSD { get; set; }
        public string GBSPOReferenceNo { get; set; }
        public DateTime? KeyInDate { get; set; }
        public DateTime? OWD { get; set; }
        public DateTime? OWRD { get; set; }
        public DateTime? RSD { get; set; }
        public DateTime? GBSCD { get; set; }
        public DateTime? GBSPUD { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string ArticleNo { get; set; }
        public string StyleNo { get; set; }
        public decimal? BrandCodeId { get; set; }
        public string Season { get; set; }
        public string LocaleNo { get; set; }
        public string CompanyNo { get; set; }
        public string Customer { get; set; }
        public string LastNo { get; set; }
        public string OutsoleNo { get; set; }
        public string ArticleName { get; set; }
        public string Dollar { get; set; }
        public string Brand { get; set; }
        public string OrderSizeCountryCode { get; set; }
        public decimal? ArticleSizeCountryCodeId { get; set; }
        public string ArticleSizeCountryCode { get; set; }
        public string Port { get; set; }
        public string ARLocaleNo { get; set; }
        public string OrdersLocaleNo { get; set; }
        public int MRPVersion { get; set; }
    }
}
