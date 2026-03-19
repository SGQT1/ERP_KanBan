using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MaterialStock
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string MaterialNameEng { get; set; }
        public decimal WarehouseId { get; set; }
        public string WarehouseNo { get; set; }
        public string OrderNo { get; set; }
        public decimal PCLUnitCodeId { get; set; }
        public string PCLUnitNameTw { get; set; }
        public string PCLUnitNameEn { get; set; }
        public decimal TransRate { get; set; }
        public decimal PurUnitCodeId { get; set; }
        public string PurUnitNameTw { get; set; }
        public string PurUnitNameEn { get; set; }
        public decimal PCLPlanQty { get; set; }
        public decimal PCLQty { get; set; }
        public decimal PurQty { get; set; }
        public decimal PCLAllocationQty { get; set; }
        public decimal PurAllocationQty { get; set; }
        public decimal Amount { get; set; }
        public decimal StockDollarCodeId { get; set; }
        public string StockDollarNameTw { get; set; }
        public string StockDollarNameEn { get; set; }
        public decimal ParentMaterialId { get; set; }
        public string ParentMaterialNameTw { get; set; }
        public string ParentMaterialNameEn { get; set; }
        public decimal LastStockIOId { get; set; }
        public decimal TotalUsageCost { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public DateTime? IODate { get; set; }
        public decimal? PurUnitPrice { get; set; }
        public decimal? AvgUnitPrice { get; set; }
        // public decimal? PurDollarCodeId { get; set; }
        // public string PurDollarNameTw { get; set; }
        // public string PurDollarNameEn { get; set; }
        // public decimal? ExchangeRate { get; set; }
        public decimal? QuotUnitPrice { get; set; }
        public string QuotDollarNameTw { get; set; }
        public decimal? RealStockINQty { get; set; }
        public decimal? RealStockOutQty { get; set; }
        public decimal? RealStockQty { get; set; }
        public decimal? RealStockOutCostQty { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public DateTime? CSD { get; set; }
        public string RefOrderNo { get; set; }
    }
}
