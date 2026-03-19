using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_ORDERSPACK_MIX2
    {
        public decimal LocaleId { get; set; }
        public decimal? RefLocaleId { get; set; }
        public decimal RefOrdersId { get; set; }
        public string Edition { get; set; }
        public int GroupBy { get; set; }
        public int CTNS { get; set; }
        public decimal? TTLPRS { get; set; }
        public int? MinCNo { get; set; }
        public int? MaxCNo { get; set; }
    }
}
