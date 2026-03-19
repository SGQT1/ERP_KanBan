using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class StockIORemoved
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal StockIOId { get; set; }
        public DateTime IODate { get; set; }
        public int SourceType { get; set; }
        public string MaterialNameTw { get; set; }
        public string WarehouseNo { get; set; }
        public string OrderNo { get; set; }
        public string PCLUnitNameTw { get; set; }
        public decimal TransRate { get; set; }
        public string PurUnitNameTw { get; set; }
        public decimal PCLIOQty { get; set; }
        public decimal PurIOQty { get; set; }
        public decimal? ReceivedLogId { get; set; }
        public decimal PurUnitPrice { get; set; }
        public string PurDollarNameTw { get; set; }
        public decimal BankingRate { get; set; }
        public string StockDollarNameTw { get; set; }
        public string Remark { get; set; }
        public string RefNo { get; set; }
        public string OrgUnitNameTw { get; set; }
        public string MPSProcessNameTw { get; set; }
        public string RefUserName { get; set; }
        public decimal MaterialStockId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
