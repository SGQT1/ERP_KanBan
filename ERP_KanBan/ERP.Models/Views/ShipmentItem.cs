using System;
using System.Collections.Generic;

namespace ERP.Models.Views {
    public class ShipmentItem {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal SaleId { get; set; }
        public decimal OrdersItemId { get; set; }
        public decimal ArticleSize { get; set; }
        public string ArticleSizeSuffix { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public string DisplaySize { get; set; }
        public decimal SaleQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? ToolingFund { get; set; }
        public decimal? ToolingCost { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public string RefOrderNo { get; set; }
        public decimal RefOrderId { get; set; }
        public decimal RefLocaleId { get; set; }
        public string RefStyleNo { get;set; }
        public string RefArticleNo { get;set; }
    }
}