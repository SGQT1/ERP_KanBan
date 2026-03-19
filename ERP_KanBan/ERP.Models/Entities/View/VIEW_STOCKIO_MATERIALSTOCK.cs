using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_STOCKIO_MATERIALSTOCK
    {
        public decimal MaterialStockId { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialNameTw { get; set; }
        public string IOYM { get; set; }
        public decimal LocaleId { get; set; }
        public string PCLUnitNameTw { get; set; }
        public string StockDollarNameTw { get; set; }
        public string WarehouseNo { get; set; }
        public decimal? MinId { get; set; }
        public decimal? MaxId { get; set; }
        public string LastIOYM { get; set; }
    }
}
