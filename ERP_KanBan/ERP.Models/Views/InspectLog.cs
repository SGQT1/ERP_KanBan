using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class InspectLog
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal RefLocaleId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string ShippingListNo { get; set; }
        public decimal ShippingListVendorId { get; set; }
        public string ShippingListVendorName { get; set; }
        public decimal POItemId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal SubTotalPrice { get; set; }
        public string Remark { get; set; }
        public decimal APId { get; set; }
        public DateTime? QCDate { get; set; }
        public decimal IQCFlag { get; set; }
        public decimal IQCGetQty { get; set; }
        public decimal? IQCTestQty { get; set; }
        public decimal IQCPassQty { get; set; }
        public decimal IQCRejectionQty { get; set; }
        public int IQCResult { get; set; }
        public string IQCMen { get; set; }
        public string IQCRemark { get; set; }
        public int SamplingMethod { get; set; }
        public decimal WarehouseId { get; set; }
        public decimal StockQty { get; set; }
        public string OrderNo { get; set; }
        public int IsAccounting { get; set; }
        public decimal TransferInId { get; set; }
        public decimal TransferInLocaleId { get; set; }
        public string TaiwanInvoiceNo { get; set; }
        public decimal TransferQty { get; set; }
        public decimal WeightUnitCodeId { get; set; }
        public decimal NetWeight { get; set; }
        public decimal GrossWeight { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int? ReceivedCount { get; set; }
        public decimal ReceivedLogId { get; set; }
        public string RefPONo { get; set; }
        public int Type { get; set; }
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
        public string TransferInLocale { get; set; }
        public string CloseMonth { get; set; }
        public int? POType { get; set; }
        public int? ReceivedType { get; set; }

        public string MaterialNameEng { get; set; }
        public string WarehouseNo { get; set; }
        public decimal PlanQty { get; set; }
    }
}
