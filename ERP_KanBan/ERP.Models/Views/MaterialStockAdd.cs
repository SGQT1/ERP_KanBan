using System;
using System.Collections.Generic;

namespace ERP.Models.Views
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
