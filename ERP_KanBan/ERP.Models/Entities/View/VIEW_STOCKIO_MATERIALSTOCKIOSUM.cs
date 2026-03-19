using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_STOCKIO_MATERIALSTOCKIOSUM
    {
        public decimal MaterialId { get; set; }
        public string MaterialNameTw { get; set; }
        public decimal LocaleId { get; set; }
        public string IOYM { get; set; }
        public string WarehouseNo { get; set; }
        public decimal? TPrePCLQty { get; set; }
        public decimal? TInQty { get; set; }
        public decimal? TOutQty { get; set; }
        public decimal? TNextPCLQty { get; set; }
        public string PCLUnitNameTw { get; set; }
        public decimal? TPreAmount { get; set; }
        public decimal? TInAmount { get; set; }
        public decimal? TOutAmount { get; set; }
        public decimal? TNextAmount { get; set; }
        public string StockDollarNameTw { get; set; }
    }
}
