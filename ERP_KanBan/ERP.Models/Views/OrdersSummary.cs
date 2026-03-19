using System;

namespace ERP.Models.Views {
    public class OrdersSummary {
        public decimal CompanyId { get; set; }
        public string Company { get; set; }
        public string BrandCode { get; set; }
        public string ArticleNo { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public decimal OrderQty { get; set; }
    }
}