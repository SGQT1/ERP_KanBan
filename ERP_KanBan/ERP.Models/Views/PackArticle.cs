using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class PackArticle
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ArticleNo { get; set; }
        public string ShoeName { get; set; }
        public decimal SizeCountryCodeId{ get; set; }
        public string SizeCountryNameTw { get; set; }
        public decimal WeightUnitCodeId { get; set; }
        public string WeightUnitNameTw { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public decimal ArticleId { get; set; }
        public decimal BrandCodeId { get; set; }
    }
}
