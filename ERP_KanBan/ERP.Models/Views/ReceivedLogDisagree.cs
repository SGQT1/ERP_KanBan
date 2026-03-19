using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class ReceivedLogDisagree
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal RefLocaleId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public decimal ShippingListVendorId { get; set; }
        public string ShippingListVendorName { get; set; }
        public decimal POItemId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal SubTotalPrice { get; set; }
        public string Remark { get; set; }
        public decimal WarehouseId { get; set; }
        public decimal StockQty { get; set; }
        public string OrderNo { get; set; }
        public decimal TransferInId { get; set; }
        public decimal TransferInLocaleId { get; set; }
        public decimal TransferQty { get; set; }
        public string RefPONo { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialNameTw { get; set; }
        public decimal? ParentMaterialId { get; set; }
        public string ParentMaterialNameTw { get; set; }
        public decimal PCLUnitCodeId { get; set; }
        public string PCLUnitNameTw { get; set; }
        public decimal PurUnitCodeId { get; set; }
        public string PurUnitNameTw { get; set; }
        public decimal PayQty { get; set; }
        public decimal FreeQty { get; set; }
        public decimal TotalQty { get; set; }
        public decimal PurDollarCodeId { get; set; }
        public string PurDollarNameTw { get; set; }
        public decimal? StockDollarCodeId { get; set; }
        public string StockDollarNameTw { get; set; }
        public string ReceivedBarcode { get; set; }
        public string CloseMonth { get; set; }
        public int? POType { get; set; }
        public string MaterialNameEng { get; set; }
        public string WarehouseNo { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal? PurUnitPrice { get; set; }
        public DateTime POLastUpdateTime { get; set; }
    }
}
