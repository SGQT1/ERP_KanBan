using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MonthMaterialStockBatchCost
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string MaterialName { get; set; }
        public string OrderNo { get; set; }
        public decimal IOMonth { get; set; }
        public int CostType { get; set; }
        public int? IOType { get; set; }
        public string PCLUnitNameTw { get; set; }
        public string StockDollarNameTw { get; set; }
        public decimal IOQty { get; set; }
        public decimal IOAmount { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public decimal? PurUnitPrice { get; set; }
        public decimal? PurDollarCodeId { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string PurDollarNameTw { get; set; }
        public string PurDollarNameEn { get; set; }
    }
}
