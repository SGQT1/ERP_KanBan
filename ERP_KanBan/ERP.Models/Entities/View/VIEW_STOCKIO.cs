using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_STOCKIO
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public DateTime IODate { get; set; }
        public int SourceType { get; set; }
        public decimal MaterialId { get; set; }
        public decimal WarehouseId { get; set; }
        public string OrderNo { get; set; }
        public decimal PCLUnitCodeId { get; set; }
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
        public string WarehouseNo { get; set; }
        public string MaterialNameTw { get; set; }
        public string PCLUnitNameTw { get; set; }
        public string StockDollarNameTw { get; set; }
        public string IOYM { get; set; }
        public string MaterialNameEn { get; set; }
        public decimal MinPrePCLQty { get; set; }
        public decimal MinPreAmount { get; set; }
    }
}
