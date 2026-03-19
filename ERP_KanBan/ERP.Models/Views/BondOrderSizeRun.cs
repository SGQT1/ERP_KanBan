using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class BondOrderSizeRun
    {
        public decimal? OrdersId { get; set; }
        public decimal LocaleId { get; set; }
        public string CompanyNo { get; set; }
        public decimal OrderQty { get; set; }
        public string OrderNo { get; set; }
        public DateTime CSD { get; set; }
        public DateTime? LCSD { get; set; }

        public string Brand { get; set; }
        public string ArticleNo { get; set; }
        public string StyleNo { get; set; }
        public string BondProductName { get; set;}

        public string ArticleSizeRun { get; set; }
        public string ArticleHead { get; set; }
    }
}
