using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class BatchOrdersCostItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string OrderNo { get; set; }
        public decimal SemiGoods { get; set; }
        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string MaterialNameTw { get; set; }
        public string UnitNameTw { get; set; }
        public decimal StandardUsage { get; set; }
        public decimal ActualUsage { get; set; }
        public string PurUnitNameTw { get; set; }
        public string PurDollarNameTw { get; set; }
        public decimal PurUnitPrice { get; set; }
        public decimal TransRate { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string CostDollarNameTw { get; set; }
        public decimal CostUnitPrice { get; set; }
        public decimal StandardCostAmount { get; set; }
        public decimal ActualCostAmount { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int IsNew { get; set; }
        public int? UsageType { get; set; }
        public int? PriceType { get; set; }
        public string Vendor { get; set; }

        public decimal? StandardCostRate { get; set; }
        public decimal? ActualCostRate { get; set; }
    }
}
