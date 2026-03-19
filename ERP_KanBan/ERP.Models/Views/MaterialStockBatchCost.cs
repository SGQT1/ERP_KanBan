using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MaterialStockBatchCost
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

        public bool Closed { get; set; }
        public string Brand { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public string Customer { get; set; }
        public string OrderQty { get; set; }
        public DateTime ETD { get; set; }
        public DateTime CSD { get; set; }
        public DateTime CloseDate { get; set; }
        public decimal OrdersId { get; set; }
    }
}
