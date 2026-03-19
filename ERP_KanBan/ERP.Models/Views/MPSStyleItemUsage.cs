using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSStyleItemUsage
    {
        public decimal Id { get; set; }
        public decimal MpsStyleItemId { get; set; }
        public decimal ArticleSize { get; set; }
        public string ArticleSizeSuffix { get; set; }
        public double ArticleInnerSize { get; set; }
        public decimal PreUnitUsage { get; set; }
        public decimal UnitUsage { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsStyleId { get; set; }
        public decimal? MRPItemId { get; set; }
        public decimal? StyleItemId { get; set; }
    }
}
