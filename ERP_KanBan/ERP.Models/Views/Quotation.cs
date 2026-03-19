using System;
using System.Collections.Generic;

namespace ERP.Models.Views {
    public class Quotation {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public int? CompanyId { get; set; }
        public string ArticleNo { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public int ProductTypeId { get; set; }
        public int? ShipmentTypeId { get; set; }
        public decimal? LastId { get; set; }
        public decimal BrandCodeId { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal DollarCodeId { get; set; }
        public int Confirmed { get; set; }
        public DateTime QuoteDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal? SizeCountryCodeId { get; set; }
        public decimal SizeBeginning { get; set; }
        public string SizeBeginningSuffix { get; set; }
        public decimal? SizeBeginningInner { get; set; }
        public decimal SizeEndding { get; set; }
        public string SizeEnddingSuffix { get; set; }
        public decimal? SizeEnddingInner { get; set; }
        public decimal FactoryPriceIntel { get; set; }
        public decimal? InvoicePriceIntel { get; set; }
        public decimal? FactoryPriceSub { get; set; }
        public decimal? InvoicePriceSub { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public string ReferFileURL { get; set; }
        public decimal ProductClass { get; set; }
        public decimal TargetOutput { get; set; }
        public int? StopOrder { get; set; }
        public int IsLimitedQty { get; set; } //MOQ Type
        public decimal LimitedQty { get; set; } //MOQ
        public decimal CLB { get; set; }
        public int IsForSeason { get; set; }
        public string Season { get; set; }
        public decimal CBSId { get; set; }
        public decimal? ToolFundIntel { get; set; }
        public decimal? ToolFundSub { get; set; }
        public string MidsoleNo { get; set; }
        public decimal? OutsoleId { get; set; }
        public decimal? MidsolePrice { get; set; }
        public decimal? OutsolePrice { get; set; }
        public decimal? ToolingOtherPrice { get; set; }
        public decimal? ToolingTotalPrice { get; set; }
        public string OutsoleNo { get; set; }
        public string LastNo { get; set; }
        
        public string CompanyNo { get; set; }
        public string Brand { get; set; }
         public string ArticleSize { get; set; }
        public string Dollar { get; set; }
        public string ShipmentType { get; set; }
        public string ProductType { get; set; }
        public string DisplaySizeBeginning { get; set; }
        public string DisplaySizeEndding { get; set; }
    }
}