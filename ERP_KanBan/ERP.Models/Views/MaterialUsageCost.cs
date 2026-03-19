using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class MaterialUsageCost
    {
        public decimal OrdersId { get; set; }
        public string OrderNo { get; set; }
        public decimal CustomerId { get; set; }
        public decimal ArticleId { get; set; }
        public decimal StyleId { get; set; }
        public int OrderType { get; set; }
        public int ProductType { get; set; }
        public DateTime ETD { get; set; }
        public decimal CompanyId { get; set; }
        public decimal LocaleId { get; set; }
        public int Status { get; set; }
        public DateTime CSD { get; set; }
        public decimal OrderQty { get; set; }
        public string ShoeName { get; set; }
        public DateTime? LCSD { get; set; }
        public string ArticleNo { get; set; }
        public string StyleNo { get; set; }
        public decimal? BrandCodeId { get; set; }
        public string Season { get; set; }

        public string LocaleNo { get; set; }
        public string CompanyNo { get; set; } 
        public string Customer { get; set; }
        public string ArticleName { get; set; }
        public string Brand { get; set; }
        public string OrdersLocaleNo { get; set; }

        public string RefARCustomer { get; set; }
        public string RefArticleNo { get; set; }
        public string RefStyleNo { get; set; }
        public int RefStyleState { get; set; }
        public string RefOrderType { get; set; }
        public string RefProductType { get; set; }
        public string RefOrdersStatus { get; set; }

        public decimal RefMaterialLocaleId { get; set; }
        public string MaterialName { get; set; }
        public decimal IOMonth { get; set; }
        public int CostType { get; set; }
        public int? IOType { get; set; }
        public string PCLUnitNameTw { get; set; }
        public string StockDollarNameTw { get; set; }
        public decimal IOQty { get; set; }
        public decimal IOAmount { get; set; }
    }
}
