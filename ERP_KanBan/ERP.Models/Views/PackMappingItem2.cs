using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class PackMappingItem2
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal PackMappingId { get; set; }
        public int Type { get; set; }
        public decimal BeginArticleSize { get; set; }
        public string BeginArticleSizeSuffix { get; set; }
        public decimal BeginArticleInnerSize { get; set; }
        public decimal EndArticleSize { get; set; }
        public string EndArticleSizeSuffix { get; set; }
        public decimal EndArticleInnerSize { get; set; }
        public string Spec { get; set; }
        public string SpecCLB { get; set; }

        //未來要增加的欄位
        public string CTNL {get;set;}
        public string CTNW {get;set;}
        public string CTNH {get;set;}
    }
}
