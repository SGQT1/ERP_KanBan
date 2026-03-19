using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class BOM
    {
        public decimal OrderId { get; set; }
        public string OrderNo { get; set; }
        public decimal OrderQty { get; set; }
        public decimal ArticleId { get; set; }
        public decimal StyleId { get; set; }
        public int BOMType { get; set; }
        public int OrderType { get; set; }
        public int ProductType { get; set; }
        public decimal CompanyId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public int Status { get; set; }
        public int OrdersVersion { get; set; }
        public string ArticleNo { get; set; }
        public string StyleNo { get; set; }
        public decimal? BrandCodeId { get; set; }
        public string RefBrand { get; set; }
        public string Season { get; set; }
    }
}
