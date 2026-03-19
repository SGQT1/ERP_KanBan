using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class OrdersItemBK
    {
        public decimal Id { get; set; }
        public decimal? OrdersId { get; set; }
        public decimal ArticleSize { get; set; }
        public string ArticleSizeSuffix { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public string DisplaySize { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal Qty { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? TransferUnitPrice { get; set; }
        public decimal TransferQty { get; set; }
        public decimal? ToolingFund { get; set; }
        public decimal? ToolingCost { get; set; }

        public decimal OrderSize{ get; set; }
        public string OrderSizeSuffix{ get; set; }
        public string ArticleSizeCountry{ get; set; }
        public string OrderSizeCountry{ get; set; }
    }
}
