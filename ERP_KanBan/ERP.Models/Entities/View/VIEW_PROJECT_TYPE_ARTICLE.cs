using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_PROJECT_TYPE_ARTICLE
    {
        public decimal ProjectType { get; set; }
        public string ArticleNo { get; set; }
        public decimal LocaleId { get; set; }
        public decimal Id { get; set; }
    }
}
