using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MaterialStockTransferItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public DateTime IODate { get; set; }
        public int SourceType { get; set; }
        public decimal MaterialId { get; set; }
        public decimal WarehouseId { get; set; }
        public string OrderNo { get; set; }
        public decimal PCLIOQty { get; set; }
        public decimal PurUnitPrice { get; set; }
        public decimal PurDollarCodeId { get; set; }
        public decimal BankingRate { get; set; }
        public decimal StockDollarCodeId { get; set; }
        public string Remark { get; set; }
        public string RefNo { get; set; }
        public decimal MaterialStockId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string MaterialName { get; set; }
        public string MaterialNameEng { get; set; }
        public string WarehouseNo { get; set; }
        public string PurCurrency { get; set; }
        public string StockCurrency { get; set; }

        public decimal MaterialStockIdIn { get; set; }
        public decimal IdIn { get; set; }
        public decimal LocaleIdIn { get; set; }
        public DateTime IODateIn { get; set; }
        public int SourceTypeIn { get; set; }
        public decimal MaterialIdIn { get; set; }
        public decimal WarehouseIdIn { get; set; }
        public string OrderNoIn { get; set; }
        public decimal PCLIOQtyIn { get; set; }
        public string RefNoIn { get; set; }
        public string RemarkIn { get; set; }

    }
}
