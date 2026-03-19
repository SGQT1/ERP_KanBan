using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class PackMappingItem1
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal PackMappingId { get; set; }
        public decimal ArticleSize { get; set; }
        public string ArticleSizeSuffix { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public decimal GWOfCTN { get; set; }
        public decimal NWOfCTN { get; set; }
        public decimal MEAS { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public decimal GWOfCTNCLB { get; set; }
        public decimal MEASCLB { get; set; }
        public decimal? PairsOfCTN { get; set; }

        public virtual PackMapping PackMapping { get; set; }
    }
}
