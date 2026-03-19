using System;
using System.Collections.Generic;

namespace ERP.Models.Views.View
{
    public class BOMPCL
    {
        public decimal OrdersId { get; set; }
        public string DisplaySize { get; set; }
        public string MappingSize { get; set; }
        public decimal Qty { get; set; }
        public string Other1Size { get; set; }
        public string Other2Size { get; set; }
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ShellDisplaySize { get; set; }
        public decimal? OrderSizeCountryCodeId { get; set; }
        public decimal? ArticleSizeCountryCodeId { get; set; }
        public string KnifeDisplaySize { get; set; }
        public string OutsoleDisplaySize { get; set; }
        public string LastDisplaySize { get; set; }
        public decimal ArticleInnerSize { get; set; }
    }
}