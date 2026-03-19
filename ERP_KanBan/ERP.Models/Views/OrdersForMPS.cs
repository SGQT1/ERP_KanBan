using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class OrdersForMPS
    {
        public decimal Id { get; set; }
        public string OrderNo { get; set; }
        public decimal CustomerId { get; set; }
        public decimal ArticleId { get; set; }
        public decimal StyleId { get; set; }
        public DateTime ETD { get; set; }
        public decimal CompanyId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public DateTime CSD { get; set; }
        public decimal OrderQty { get; set; }
        public string ShoeName { get; set; }
        public DateTime? LCSD { get; set; }
        public string ArticleNo { get; set; }
        public string StyleNo { get; set; }
        public decimal? BrandCodeId { get; set; }
        public string Brand { get; set; }
        public string Customer { get; set; }
        public string ArticleSizeCountryCode{ get; set; }
        public int Status { get; set; }
        public decimal? MPSOrdersId { get; set; }
        public decimal? MpsArticleId { get; set; }
        public decimal? ProcessSetId { get; set; }
        public decimal? IncreaseRate { get; set; }
        public int BaseOn { get; set; }
        public int ProcessType { get; set; }
    }
}
