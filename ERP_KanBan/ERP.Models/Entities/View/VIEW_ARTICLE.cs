using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_ARTICLE
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ArticleNo { get; set; }
        public string ArticleName { get; set; }
        public decimal BrandCodeId { get; set; }
        public decimal? OutsoleId { get; set; }
        public decimal? ShellId { get; set; }
        public decimal? KnifeId { get; set; }
        public decimal? LastId { get; set; }
        public string BrandNameTw { get; set; }
        public string KnifeNo { get; set; }
        public string LastNo { get; set; }
        public string OutsoleNo { get; set; }
        public string ShellNo { get; set; }
    }
}
