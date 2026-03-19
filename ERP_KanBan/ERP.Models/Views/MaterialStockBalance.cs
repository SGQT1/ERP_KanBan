using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MaterialStockBalance
    {
        public decimal LocaleId { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string MaterialNameEng { get; set; }
        public decimal WarehouseId { get; set; }
        public string WarehouseNo { get; set; }
        public string PCLUnitNameTw { get; set; }
        public decimal? PCLUnitCodeId { get; set; }
        public decimal? PCLQty { get; set; }
        public decimal? RealPCLQty { get; set; }
        public decimal Amount { get; set; }
        
        public decimal? PurTotalQty { get; set; }
        public decimal? PurTotalAmount { get; set; }
        public decimal AvgPrice { get; set; }
        public string Dollar { get; set; }
        public decimal? DollarCodeId { get; set; }
        public DateTime? MaxIODate { get; set; }
        public decimal? NewAmount { get; set; }
        public decimal? NewAvgPrice { get; set; }
        public string NewDollar { get; set; }
        public decimal? NewDollarCodeId { get; set; }
        public decimal? ExchangeRate { get; set; }
    }
}
