using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_STYLEITEM_PARTPAGE
    {
        public decimal LocaleId { get; set; }
        public string StyleNo { get; set; }
        public string ArticleDisplaySize { get; set; }
        public double ArticleInnerSize { get; set; }
        public string KnifeDisplaySize { get; set; }
        public double KnifeInnerSize { get; set; }
        public string OutsoleDisplaySize { get; set; }
        public double OutsoleInnerSize { get; set; }
        public string LastDisplaySize { get; set; }
        public double LastInnerSize { get; set; }
        public string ShellDisplaySize { get; set; }
        public double ShellInnerSize { get; set; }
        public string Other1Desc { get; set; }
        public double Other1InnerSize { get; set; }
        public string Other2SizeDesc { get; set; }
        public double Other2InnerSize { get; set; }
        public int Page { get; set; }
    }
}
