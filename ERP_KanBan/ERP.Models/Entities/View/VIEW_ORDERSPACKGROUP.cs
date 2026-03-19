using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_ORDERSPACKGROUP
    {
        public decimal LocaleId { get; set; }
        public decimal? RefLocaleId { get; set; }
        public decimal RefOrdersId { get; set; }
        public string Edition { get; set; }
        public int GroupBy { get; set; }
        public decimal MEAS { get; set; }
        public int? TTLCTNS { get; set; }
        public decimal? TTLPRS { get; set; }
        public decimal? MinItemInnerSize { get; set; }
        public decimal? MaxItemInnerSize { get; set; }
        public string MinRefDisplaySize { get; set; }
        public string MaxRefDisplaySize { get; set; }
    }
}
