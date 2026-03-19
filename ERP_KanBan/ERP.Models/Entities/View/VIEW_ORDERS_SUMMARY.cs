using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_ORDERS_SUMMARY
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string LocaleNo { get; set; }
        public string CustomerOrderNo { get; set; }
        public string OrderNo { get; set; }
        public decimal CompanyId { get; set; }
        public string CompanyNo { get; set; }
        public decimal CustomerId { get; set; }
        public string EnglishName { get; set; }
        public string ChineseName { get; set; }
        public decimal StyleId { get; set; }
        public decimal ArticleId { get; set; }
        public string ArticleNo { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public decimal OrderQty { get; set; }
        public decimal SaleSubTotal { get; set; }
        public decimal SimpleSaleSubTotal { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ToolingFund { get; set; }
        public decimal ToolingCost { get; set; }
        public string DollarNameTw { get; set; }
        public decimal? OrdersUnitPrice { get; set; }
        public decimal OrdersToolingFund { get; set; }
        public string OrdersDollarNameTw { get; set; }
        public string CSD { get; set; }
        public string CSDYM { get; set; }
        public string CSDDD { get; set; }
        public string ETD { get; set; }
        public string RSD { get; set; }
        public string ETDYM { get; set; }
        public string ETDDD { get; set; }
        public decimal? LastId { get; set; }
        public string LastNo { get; set; }
        public decimal? OutsoleId { get; set; }
        public string OutsoleNo { get; set; }
        public int Status { get; set; }
        public decimal BrandCodeId { get; set; }
        public string BrandTw { get; set; }
        public int OrderType { get; set; }
        public int ProductType { get; set; }
        public int? TransitType { get; set; }
        public string OrderTypeTw { get; set; }
        public string OrderTypeEn { get; set; }
        public string ProductTypeTw { get; set; }
        public string ProductTypeEn { get; set; }
        public string TransitTypeDescTw { get; set; }
        public string TransitTypeDescEn { get; set; }
        public string StatusTw { get; set; }
        public int SpecialPackingStatus { get; set; }
        public decimal ARCustomerId { get; set; }
        public string AREnglishName { get; set; }
        public string ARChineseName { get; set; }
        public string SpecialNote { get; set; }
        public string ColorDesc { get; set; }
        public string OrderDate { get; set; }
        public string ShippingDate { get; set; }
        public string SimpleShippingDate { get; set; }
        public string MappingSizeCountryNameTw { get; set; }
        public string SizeCountryNameTw { get; set; }
        public string BarcodeNameTw { get; set; }
        public string ExportPortNameTw { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string CustomerStyleNo { get; set; }
        public decimal? ReferUnitPrice { get; set; }
        public string DeliveryTerms { get; set; }
        public int? PayType { get; set; }
        public int PackingType { get; set; }
        public string PackingTypeDesc { get; set; }
        public string InsockLabel { get; set; }
        public string SafeCode { get; set; }
        public string PackingDescTW { get; set; }
        public string PackingDescEng { get; set; }
        public string Mark { get; set; }
        public string SideMark { get; set; }
        public string Mark1Desc { get; set; }
        public string Mark2Desc { get; set; }
        public string Mark3Desc { get; set; }
        public string Mark4Desc { get; set; }
        public string Mark5Desc { get; set; }
        public decimal MixedBoxes1 { get; set; }
        public decimal MixedBoxes2 { get; set; }
        public decimal MixedBoxes3 { get; set; }
        public decimal MixedBoxes4 { get; set; }
        public decimal MixedBoxes5 { get; set; }
        public string OutsoleColorDescTW { get; set; }
        public string KnifeNo { get; set; }
        public decimal? ARLocaleId { get; set; }
        public decimal? ParentOrdersId { get; set; }
        public decimal? RefOrdersLocaleId { get; set; }
        public string LCSD { get; set; }
        public string LCSDYM { get; set; }
        public string LCSDDD { get; set; }
        public string GBSPOReferenceNo { get; set; }
    }
}
