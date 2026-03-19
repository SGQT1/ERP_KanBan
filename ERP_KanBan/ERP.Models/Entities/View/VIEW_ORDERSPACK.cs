using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_ORDERSPACK
    {
        public decimal LocaleId { get; set; }
        public decimal? RefLocaleId { get; set; }
        public decimal RefOrdersId { get; set; }
        public string Edition { get; set; }
        public decimal ItemInnerSize { get; set; }
        public string RefDisplaySize { get; set; }
        public decimal PairOfCTN { get; set; }
        public int CTNS { get; set; }
        public decimal AdjQty { get; set; }
        public decimal? TTLPrs { get; set; }
        public int GroupBy { get; set; }
        public decimal NWOfCTN { get; set; }
        public decimal? TTLNW { get; set; }
        public decimal GWOfCTN { get; set; }
        public decimal? TTLGW { get; set; }
        public decimal MEAS { get; set; }
        public decimal? TTLMEAS { get; set; }
        public decimal CBM { get; set; }
        public string OrderNo { get; set; }
        public decimal? SizeCountryCodeId { get; set; }
        public int? PackingType { get; set; }
        public string PackingTypeDesc { get; set; }
        public string SizeCountryNameTw { get; set; }
        public string SizeCountryNameEn { get; set; }
        public int? MinCNo { get; set; }
        public int? MaxCNo { get; set; }
        public int? PLPackingType { get; set; }
    }
}
