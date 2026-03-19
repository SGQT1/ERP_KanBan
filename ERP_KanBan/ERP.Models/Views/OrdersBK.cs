using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class OrdersBK
    {
        public decimal Id { get; set; }
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
        public decimal? OrderSizeCountryCodeId { get; set; }
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
        //public decimal MixedBoxes1 { get; set; }
        //public decimal MixedBoxes2 { get; set; }
        //public decimal MixedBoxes3 { get; set; }
        //public decimal MixedBoxes4 { get; set; }
        //public decimal MixedBoxes5 { get; set; }
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
        public string ArticleNo { get; set; }
        public string StyleNo { get; set; }
        public decimal? BrandCodeId { get; set; }
        public string Season { get; set; }

        public string RefLocale { get; set; }
        public string RefCustomer { get; set; }
        public string RefARCustomer { get; set; }
        public string RefArticleNo { get; set; }
        public string RefStyleNo { get; set; }
        public string RefColorDesc { get; set; }
        public string RefLast {get;set;}
        public string RefOutsole {get;set;}
        public string RefOutsoleColorDesc {get;set;}
        public string RefOrderType { get; set; }
        public string RefProductType { get; set; }
        public string RefCompany { get; set; }
        public string RefOrderSizeCountry { get; set; }
        public string RefBarcode { get; set; }
        public string RefOrdersStatus { get; set; }
        public string RefPort { get; set; }
        public string RefPackingType { get; set; }
        public string RefDollar { get; set; }
        public string RefdoMRP { get; set; }
        public string RefPayType { get; set; }
        public string RefDeliveryTerm { get; set; }
        public string RefTransitType { get; set; }
        public string RefApproved { get; set; }
        public string RefARLocale { get; set; }
        public string RefOrdersLocale { get; set; }
        public string RefBrand { get; set; }
        public string RefShoeName { get; set; }
        public decimal? ArticleSizeCountryCodeId { get; set; }
        public string RefArticleSizeCountry{ get; set; }
    }
}
