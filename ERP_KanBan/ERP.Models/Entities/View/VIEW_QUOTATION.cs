using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_QUOTATION
    {
        public string BrandTw { get; set; }
        public string DollarNameTw { get; set; }
        public decimal Id { get; set; }
        public DateTime QuoteDate { get; set; }
        public decimal CBSId { get; set; }
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
        public decimal? ToolFundIntel { get; set; }
        public decimal? ToolFundSub { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string Remark { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public string MidsoleNo { get; set; }
        public decimal? OutsoleId { get; set; }
        public decimal? MidsolePrice { get; set; }
        public decimal? OutsolePrice { get; set; }
        public decimal? ToolingOtherPrice { get; set; }
        public decimal? ToolingTotalPrice { get; set; }
        public decimal BrandCodeId { get; set; }
        public string ArticleNo { get; set; }
        public decimal DollarCodeId { get; set; }
        public int Confirmed { get; set; }
        public string ReferFileURL { get; set; }
        public string ShoeName { get; set; }
        public decimal? LastId { get; set; }
        public string StyleNo { get; set; }
        public decimal ProductClass { get; set; }
        public int ProductType { get; set; }
        public decimal TargetOutput { get; set; }
        public int? StopOrder { get; set; }
        public int? CompanyId { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
