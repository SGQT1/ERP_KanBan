using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class PackMappingItem2
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
        public Guid msrepl_tran_version { get; set; }
        public string SpecCLB { get; set; }
        public string CTNL { get; set; }
        public string CTNW { get; set; }
        public string CTNH { get; set; }

        public virtual PackMapping PackMapping { get; set; }
    }
}
