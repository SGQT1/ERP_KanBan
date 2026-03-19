using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class REP_PCL
    {
        public decimal OrdersId { get; set; }
        public string DisplaySize { get; set; }
        public string MappingSize { get; set; }
        public decimal Qty { get; set; }
        public string other1size { get; set; }
        public string other2size { get; set; }
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ShellDisplaySize { get; set; }
        public decimal? SizeCountryCodeId { get; set; }
        public decimal? Expr1 { get; set; }
        public string KnifeDisplaySize { get; set; }
        public string OutsoleDisplaySize { get; set; }
        public string LastDisplaySize { get; set; }
        public decimal ArticleInnerSize { get; set; }
    }
}
