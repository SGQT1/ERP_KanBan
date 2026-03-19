using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class OrdersSizeRun
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
        public string MESFormat { get; set; }

        public string LastSizeRun { get; set; }
        public string LastHead { get; set; }

        public string ArticleSizeRun { get; set; }
        public string ArticleHead { get; set; }
        // public string SJ010 { get; set; }
        // public string SJ015 { get; set; }
        // public string SJ020 { get; set; }
        // public string SJ025 { get; set; }
        // public string SJ030 { get; set; }
        // public string SJ035 { get; set; }
        // public string SJ040 { get; set; }
        // public string SJ045 { get; set; }
        // public string SJ050 { get; set; }
        // public string SJ055 { get; set; }
        // public string SJ060 { get; set; }
        // public string SJ065 { get; set; }
        // public string SJ070 { get; set; }
        // public string SJ075 { get; set; }
        // public string SJ080 { get; set; }
        // public string SJ085 { get; set; }
        // public string SJ090 { get; set; }
        // public string SJ095 { get; set; }
        // public string SJ100 { get; set; }
        // public string SJ105 { get; set; }
        // public string SJ110 { get; set; }
        // public string SJ115 { get; set; }
        // public string SJ120 { get; set; }
        // public string SJ125 { get; set; }
        // public string SJ130 { get; set; }
        // public string SJ135 { get; set; }
        // public string SJ140 { get; set; }
        // public string SJ145 { get; set; }
        // public string SJ150 { get; set; }
        // public string SJ155 { get; set; }
        // public string S010 { get; set; }
        // public string S015 { get; set; }
        // public string S020 { get; set; }
        // public string S025 { get; set; }
        // public string S030 { get; set; }
        // public string S035 { get; set; }
        // public string S040 { get; set; }
        // public string S045 { get; set; }
        // public string S050 { get; set; }
        // public string S055 { get; set; }
        // public string S060 { get; set; }
        // public string S065 { get; set; }
        // public string S070 { get; set; }
        // public string S075 { get; set; }
        // public string S080 { get; set; }
        // public string S085 { get; set; }
        // public string S090 { get; set; }
        // public string S095 { get; set; }
        // public string S100 { get; set; }
        // public string S105 { get; set; }
        // public string S110 { get; set; }
        // public string S115 { get; set; }
        // public string S120 { get; set; }
        // public string S125 { get; set; }
        // public string S130 { get; set; }
        // public string S135 { get; set; }
        // public string S140 { get; set; }
        // public string S145 { get; set; }
        // public string S150 { get; set; }
        // public string S155 { get; set; }
        // public string S160 { get; set; }
        // public string S165 { get; set; }
        // public string S170 { get; set; }
        // public string S175 { get; set; }
        // public string S180 { get; set; }
        // public string S185 { get; set; }
        // public string S190 { get; set; }
        // public string S195 { get; set; }
        // public string S200 { get; set; }
        // public string S205 { get; set; }
        // public string S210 { get; set; }
        // public string S215 { get; set; }
        // public string S220 { get; set; }
        // public string S225 { get; set; }
        // public string S230 { get; set; }
        // public string S235 { get; set; }
        // public string S240 { get; set; }
        // public string S245 { get; set; }
        // public string S250 { get; set; }
        // public string S255 { get; set; }
        // public string S260 { get; set; }
        // public string S265 { get; set; }
        // public string S270 { get; set; }
        // public string S275 { get; set; }
        // public string S280 { get; set; }
        // public string S285 { get; set; }
        // public string S290 { get; set; }
        // public string S295 { get; set; }
        // public string S300 { get; set; }
        // public string S305 { get; set; }
        // public string S310 { get; set; }
        // public string S315 { get; set; }
        // public string S320 { get; set; }
        // public string S325 { get; set; }
        // public string S330 { get; set; }
        // public string S335 { get; set; }
        // public string S340 { get; set; }
        // public string S345 { get; set; }
        // public string S350 { get; set; }
        // public string S355 { get; set; }
        // public string S360 { get; set; }
        // public string S365 { get; set; }
        // public string S370 { get; set; }
        // public string S375 { get; set; }
        // public string S380 { get; set; }
        // public string S385 { get; set; }
        // public string S390 { get; set; }
        // public string S395 { get; set; }
        // public string S400 { get; set; }
        // public string S405 { get; set; }
        // public string S410 { get; set; }
        // public string S415 { get; set; }
        // public string S420 { get; set; }
        // public string S425 { get; set; }
        // public string S430 { get; set; }
        // public string S435 { get; set; }
        // public string S440 { get; set; }
        // public string S445 { get; set; }
        // public string S450 { get; set; }
        // public string S455 { get; set; }
        // public string S460 { get; set; }
        // public string S465 { get; set; }
        // public string S470 { get; set; }
        // public string S475 { get; set; }
        // public string S480 { get; set; }
        // public string S485 { get; set; }
        // public string S490 { get; set; }
        // public string S495 { get; set; }
        // public string S500 { get; set; }
        // public string S505 { get; set; }

    }
}
