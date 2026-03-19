using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_PROJECT_CBS
    {
        public decimal ProjectId { get; set; }
        public decimal ProjectType { get; set; }
        public string ArticleNo { get; set; }
        public string ColorCode { get; set; }
        public string ColorDesc { get; set; }
        public decimal ShoeSize { get; set; }
        public string Suffix { get; set; }
        public decimal InnerSize { get; set; }
        public decimal ProjectItemId { get; set; }
        public decimal? Id { get; set; }
        public string LastNo { get; set; }
        public string Factory { get; set; }
        public decimal? ExchangeRate { get; set; }
        public DateTime? MadeDate { get; set; }
        public string SizeRunDesc { get; set; }
        public string SampleSizeDesc { get; set; }
        public string MoldAmortiseOnDesc { get; set; }
        public string MoldAmortizationDesc { get; set; }
        public decimal? Labor { get; set; }
        public decimal? Overhead { get; set; }
        public decimal? Profit { get; set; }
        public decimal? TTL { get; set; }
        public decimal? TotalMaterialCost { get; set; }
        public decimal? StandardQuote { get; set; }
        public decimal? OurQuote { get; set; }
        public decimal? OurLabor { get; set; }
        public decimal? OurOverhead { get; set; }
        public decimal? OurProfit { get; set; }
        public decimal? OurTTL { get; set; }
        public decimal? OurTotalMaterialCost { get; set; }
        public decimal? OurStandardQuote { get; set; }
        public string TargetPriceDesc { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public decimal CustomerId { get; set; }
        public decimal? BrandCodeId { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? CBSLocaleId { get; set; }
        public int Status { get; set; }
        public decimal SizeCountryCodeId { get; set; }
        public decimal? SurfaceRate { get; set; }
        public decimal? DollarCodeId { get; set; }
        public string ShoeName { get; set; }
    }
}
