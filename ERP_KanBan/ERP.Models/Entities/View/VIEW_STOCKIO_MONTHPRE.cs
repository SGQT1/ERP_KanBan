using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_STOCKIO_MONTHPRE
    {
        public decimal LocaleId { get; set; }
        public decimal Id { get; set; }
        public string IOYM { get; set; }
        public decimal MaterialId { get; set; }
        public decimal MaterialStockId { get; set; }
        public string WarehouseNo { get; set; }
        public string OrderNo { get; set; }
        public string MaterialNameTw { get; set; }
        public decimal PrePCLQty { get; set; }
        public string PCLUnitNameTw { get; set; }
        public decimal PreAmount { get; set; }
        public string StockDollarNameTw { get; set; }
    }
}
