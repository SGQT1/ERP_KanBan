using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MaterialStockAdd
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MaterialStockId { get; set; }
        public decimal? StockInQty { get; set; }
        public decimal? StockOutQty { get; set; }
        public decimal? SotckOutCostQty { get; set; }
        public decimal? PurPrice { get; set; }
        public decimal? PurDollarCodeId { get; set; }
        public string PurDollarNameTw { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string ModifyUserName { get; set; }
    }
}
