using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class OrdersItem
    {
        public OrdersItem()
        {
            SaleItem = new HashSet<SaleItem>();
        }

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
        public decimal MixedQty1 { get; set; }
        public decimal MixedQty2 { get; set; }
        public decimal MixedQty3 { get; set; }
        public decimal MixedQty4 { get; set; }
        public decimal MixedQty5 { get; set; }
        public decimal? TransferUnitPrice { get; set; }
        public decimal TransferQty { get; set; }
        public decimal? ToolingFund { get; set; }
        public decimal? ToolingCost { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual PCL PCL { get; set; }
        public virtual ICollection<SaleItem> SaleItem { get; set; }
    }
}
