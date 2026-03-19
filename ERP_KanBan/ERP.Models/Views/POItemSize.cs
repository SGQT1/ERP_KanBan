using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class POItemSize
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal POItemId { get; set; }
        public string DisplaySize { get; set; }
        public decimal Qty { get; set; }
        public decimal? SeqNo { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal PreQty { get; set; }
        public decimal? OrdersId { get; set; }
        public decimal? MaterialId { get; set; }
        public decimal? ParentMaterialId { get; set; }

        public string ArticleDisplaySize { get; set; }
        public string KnifeDisplaySize { get; set; }
        public string OutsoleDisplaySize { get; set; }
        public string LastDisplaySize { get; set; }
        public string ShellDisplaySize { get; set; }
        public string Other1Desc { get; set; }
        public string Other2SizeDesc { get; set; }
        public decimal? ArticleInnerSize { get; set; }
    }

    public class POItemSizeUsage
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal POItemId { get; set; }
        public decimal MaterialId { get; set; }
        public decimal? ParentMaterialId { get; set; }
        public decimal? OrdersId { get; set; }

        public int AlternateType { get; set; }
        public decimal? ArticleSize { get; set; }
        public string ArticleSizeSuffix { get; set; }
        public decimal? ArticleInnerSize { get; set; }
        public decimal? Usage { get; set; }
        public decimal? Qty { get; set; }
    }
}
