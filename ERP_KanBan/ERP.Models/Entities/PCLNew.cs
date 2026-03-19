using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class PCLNew
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal OrdersId { get; set; }
        public decimal OrdersItemId { get; set; }
        public decimal ArticleSize { get; set; }
        public string ArticleSizeSuffix { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public string DisplaySize { get; set; }
        public decimal Qty { get; set; }
        public decimal MappingSize { get; set; }
        public string MappingSizeSuffix { get; set; }
        public decimal MappingInnerSize { get; set; }
        public decimal KnifeSize { get; set; }
        public string KnifeSizeSuffix { get; set; }
        public decimal KnifeInnerSize { get; set; }
        public string KnifeDisplaySize { get; set; }
        public decimal OutsoleSize { get; set; }
        public string OutsoleSizeSuffix { get; set; }
        public decimal OutsoleInnerSize { get; set; }
        public string OutsoleDisplaySize { get; set; }
        public decimal LastSize { get; set; }
        public string LastSizeSuffix { get; set; }
        public decimal LastInnerSize { get; set; }
        public string LastDisplaySize { get; set; }
        public decimal ShellSize { get; set; }
        public string ShellSizeSuffix { get; set; }
        public decimal ShellInnerSize { get; set; }
        public decimal Other1Size { get; set; }
        public string Other1SizeSuffix { get; set; }
        public decimal Other1InnerSize { get; set; }
        public string Other1Desc { get; set; }
        public decimal Other2Size { get; set; }
        public string Other2SizeSuffix { get; set; }
        public decimal Other2InnerSize { get; set; }
        public string Other2SizeDesc { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int OrdersVersion { get; set; }
        public string ShellDisplaySize { get; set; }
    }
}
