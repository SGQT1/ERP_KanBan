using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class PCLSizeRun
    {
        public decimal? OrdersId { get; set; }
        public decimal LocaleId { get; set; }
        public decimal CompanyId { get; set; }
        public string CompanyNo { get; set; }
        public decimal CustomerId { get; set; }
        public string Customer { get; set; }
        public string CustomerOrderNo { get; set; }
        public string GBSPOReferenceNo { get; set; }
        public decimal OrderQty { get; set; }
        public string OrderNo { get; set; }
        public int ProductType { get; set; }
        public int OrderType { get; set; }
        public DateTime? OWD { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CSD { get; set; }
        public DateTime? LCSD { get; set; }
        public decimal? BrandCodeId { get; set; }
        public string Brand { get; set; }
        public string ArticleNo { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public string OutsoleNo { get; set; }
        public string LastNo { get; set; }
        public string ArticleSizeCountry { get; set; }
        public string OrderSizeCountry { get; set; }
        
        public string LastSizeRun { get; set; }
        public string LastHead { get; set; }

        public string ArticleSizeRun { get; set; }
        public string ArticleHead { get; set; }

        public string KnifeSizeRun { get; set; }
        public string KnifeHead { get; set; }

        public string OutsoleSizeRun { get; set; }
        public string OutsoleHead { get; set; }

        public string ShellSizeRun { get; set; }
        public string ShellHead { get; set; }

        public string Other1SizeRun { get; set; }
        public string Other1Head { get; set; }

        public string Other2SizeRun { get; set; }
        public string Other2Head { get; set; }
    }
}
