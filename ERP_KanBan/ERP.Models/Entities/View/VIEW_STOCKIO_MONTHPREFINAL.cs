using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_STOCKIO_MONTHPREFINAL
    {
        public decimal LocaleId { get; set; }
        public string IODate { get; set; }
        public string IOYM { get; set; }
        public string SourceType { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialNameTw { get; set; }
        public string OrderNo { get; set; }
        public int InQty { get; set; }
        public int OutQty { get; set; }
        public decimal? StockQty { get; set; }
        public string PCLUnitNameTw { get; set; }
        public decimal? PreAmount { get; set; }
        public string StockDollarNameTw { get; set; }
        public string Searial { get; set; }
    }
}
