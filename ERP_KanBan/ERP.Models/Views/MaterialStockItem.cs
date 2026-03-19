using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MaterialStockItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public DateTime IODate { get; set; }
        public int SourceType { get; set; }
        public decimal MaterialId { get; set; }
        public decimal WarehouseId { get; set; }
        public string OrderNo { get; set; }
        public decimal PCLUnitCodeId { get; set; }
        public string PCLUnitNameTw { get; set; }
        public decimal TransRate { get; set; }
        public decimal PurUnitCodeId { get; set; }
        public decimal PCLIOQty { get; set; }
        public decimal PurIOQty { get; set; }
        public decimal? ReceivedLogId { get; set; }
        public decimal PurUnitPrice { get; set; }
        public decimal PurDollarCodeId { get; set; }
        public decimal BankingRate { get; set; }
        public decimal StockDollarCodeId { get; set; }
        public string Remark { get; set; }
        public string RefNo { get; set; }
        public decimal? OrgUnitId { get; set; }
        public string OrgUnitNameTw { get; set; }
        public string OrgUnitNameEn { get; set; }
        public decimal? MPSProcessId { get; set; }
        public string MPSProcessNameTw { get; set; }
        public string MPSProcessNameEn { get; set; }
        public string RefUserName { get; set; }
        public decimal MaterialStockId { get; set; }
        public decimal PrePCLQty { get; set; }
        public decimal PreAmount { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string MaterialName { get; set; }
        public string MaterialNameEng { get; set; }
        public string WarehouseNo { get; set; }
        public string PCLUnit { get; set; }
        public string PurUnit { get; set; }
        public string PurCurrency { get; set; }
        public string StockCurrency { get; set; }
        public int? SeqNo { get; set; }
        public string VendorName { get; set; }
        public decimal? VendorId { get; set; }
        public decimal? AvgUnitPrice { get; set; }
        public decimal? NewPurUnitPrice { get; set; }
        public decimal? Amount { get; set; }
        public decimal? NewAmount { get; set; }
        public string PurDollarNameTw { get; set; }
        public decimal? MPSQty { get; set; }    //派工或需求量
        public decimal? StockQty { get; set; }  //庫存數
        public decimal? UsageQty { get; set; }  //已使用數
        public decimal? DailyId { get; set; }  //已使用數
        public string DailyNo { get; set; }    //派工或需求量
        public int? SemiGoods { get; set; }
    }
}
