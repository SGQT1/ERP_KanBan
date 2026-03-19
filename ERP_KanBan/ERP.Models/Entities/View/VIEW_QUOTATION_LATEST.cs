using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_QUOTATION_LATEST
    {
        public decimal LocaleId { get; set; }
        public decimal BrandCodeId { get; set; }
        public string ArticleNo { get; set; }
        public string StyleNo { get; set; }
        public int ProductType { get; set; }
        public string MaxEffectiveDate { get; set; }
        public int Type { get; set; }
    }
}
