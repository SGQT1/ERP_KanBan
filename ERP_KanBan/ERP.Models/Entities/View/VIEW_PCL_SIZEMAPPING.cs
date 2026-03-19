using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_PCL_SIZEMAPPING
    {
        public string DisplaySize { get; set; }
        public decimal Qty { get; set; }
        public string knifeSize { get; set; }
        public string OutsoleSize { get; set; }
        public string LastSize { get; set; }
        public string ShellDisplaySize { get; set; }
        public string Other1Size { get; set; }
        public string Other2Size { get; set; }
        public string MappingSize { get; set; }
        public string ArticleSize { get; set; }
        public decimal? SourceCountryCodeId { get; set; }
        public decimal TargetCountryCodeId { get; set; }
        public decimal OrdersId { get; set; }
        public decimal LocaleId { get; set; }
    }
}
