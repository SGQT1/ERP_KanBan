using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class SaleItem
    {
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
        public Guid msrepl_tran_version { get; set; }

        public virtual OrdersItem OrdersItem { get; set; }
        public virtual Sale Sale { get; set; }
    }
}
