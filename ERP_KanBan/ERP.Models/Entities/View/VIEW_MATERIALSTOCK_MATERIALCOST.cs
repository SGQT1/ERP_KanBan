using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_MATERIALSTOCK_MATERIALCOST
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string OrderNo { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string PCLUnitNameTw { get; set; }
        public decimal TotalPCLIOQty { get; set; }
        public decimal? TotalPCLQty { get; set; }
        public string StockDollarNameTw { get; set; }
        public decimal? TotalUsageCost { get; set; }
    }
}
