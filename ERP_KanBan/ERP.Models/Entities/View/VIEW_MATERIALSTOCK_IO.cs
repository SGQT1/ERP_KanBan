using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_MATERIALSTOCK_IO
    {
        public decimal LocaleId { get; set; }
        public string YM { get; set; }
        public string WarehouseNo { get; set; }
        public decimal MaterialStockId { get; set; }
        public decimal MaterialId { get; set; }
        public string OrderNo { get; set; }
        public string MaterialName { get; set; }
        public string MaterialNameEng { get; set; }
        public string PCLUnitNameTw { get; set; }
        public string StockDollarNameTw { get; set; }
        public int IOType { get; set; }
        public decimal? IOQty { get; set; }
        public decimal? IOAmount { get; set; }
    }
}
