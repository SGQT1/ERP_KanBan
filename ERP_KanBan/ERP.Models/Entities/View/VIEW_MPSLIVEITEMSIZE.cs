using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_MPSLIVEITEMSIZE
    {
        public decimal MpsLiveId { get; set; }
        public DateTime PlanDate { get; set; }
        public decimal PlanQty { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsLiveItemId { get; set; }
        public decimal MpsOrdersItemId { get; set; }
        public decimal SubQty { get; set; }
        public decimal MpsOrdersId { get; set; }
        public decimal ArticleSize { get; set; }
        public string ArticleSizeSuffix { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public string DisplaySize { get; set; }
        public decimal OrderQty { get; set; }
        public decimal Qty { get; set; }
        public string KnifeDisplaySize { get; set; }
        public double KnifeInnerSize { get; set; }
        public string OutsoleDisplaySize { get; set; }
        public double OutsoleInnerSize { get; set; }
        public string LastDisplaySize { get; set; }
        public double LastInnerSize { get; set; }
        public string ShellDisplaySize { get; set; }
        public double ShellInnerSize { get; set; }
        public string Other1SizeDesc { get; set; }
        public double Other1InnerSize { get; set; }
        public string Other2SizeDesc { get; set; }
        public double Other2InnerSize { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal Id { get; set; }
    }
}
