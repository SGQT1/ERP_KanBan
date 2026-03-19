using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_PROJECT_BRAND
    {
        public string ArticleNo { get; set; }
        public decimal? BrandCodeId { get; set; }
        public decimal LocaleId { get; set; }
        public string CodeNo { get; set; }
        public string NameTW { get; set; }
        public string ChineseName { get; set; }
    }
}
