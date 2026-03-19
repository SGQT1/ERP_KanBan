using System;

namespace ERP.Models.Views
{
    public class OrdersRecord
    {
        public decimal CompanyId { get; set; }
        public string CompanyNo { get; set; }
        public decimal? BrandCodeId { get; set; }
        public string Brand { get; set; }
        public decimal Records { get; set; }
        public decimal KeyInMonth { get; set; }
    }
}