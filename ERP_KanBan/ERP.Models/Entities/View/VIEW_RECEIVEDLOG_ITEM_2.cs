using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_RECEIVEDLOG_ITEM_2
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string OrderNo { get; set; }
        public decimal POItemId { get; set; }
        public decimal RefLocaleId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public decimal ShippingListVendorId { get; set; }
        public string ShippingListVendorShortNameTw { get; set; }
        public string ShippingListVendorNameTw { get; set; }
        public string ShippingListVendorNameEn { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal SubTotalPrice { get; set; }
        public decimal StockQty { get; set; }
        public decimal TransferQty { get; set; }
        public decimal POExchPOItemId { get; set; }
        public string POExchOrderNo { get; set; }
        public decimal POExchLocaleId { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public string PONo { get; set; }
        public string PurUnitNameTw { get; set; }
        public string PurUnitNameEn { get; set; }
        public string PCLUnitNameTw { get; set; }
        public string PCLUnitNameEn { get; set; }
        public int Status { get; set; }
        public DateTime? VendorETD { get; set; }
        public DateTime FactoryETD { get; set; }
        public string DollarNameTw { get; set; }
        public string DollarNameEn { get; set; }
        public decimal CompanyId { get; set; }
        public decimal OrdersId { get; set; }
        public decimal TransferItemId { get; set; }
        public string StyleNo { get; set; }
        public string Brand { get; set; }
        public DateTime CSD { get; set; }
        public DateTime ETD { get; set; }
        public decimal TransferInId { get; set; }
        public decimal TransferInLocaleId { get; set; }
        public decimal WarehouseId { get; set; }
        public string ShippingListNo { get; set; }
        public int IsAccounting { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public decimal IQCGetQty { get; set; }
        public int IQCResult { get; set; }
        public decimal IQCTestQty { get; set; }
        public decimal IQCPassQty { get; set; }
        public decimal IQCRejectionQty { get; set; }
        public string IQCMen { get; set; }
        public string IQCRemark { get; set; }
        public int SamplingMethod { get; set; }
        public string TaiwanInvoiceNo { get; set; }
        public decimal WeightUnitCodeId { get; set; }
        public decimal NetWeight { get; set; }
        public decimal GrossWeight { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal APId { get; set; }
        public DateTime? QCDate { get; set; }
        public decimal IQCFlag { get; set; }
        public decimal PurQty { get; set; }
        public decimal PurLocaleId { get; set; }
        public decimal ReceivingLocaleId { get; set; }
        public decimal PaymentLocaleId { get; set; }
        public decimal? ParentMaterialId { get; set; }
        public int? IsAllowPartial { get; set; }
        public int IsOverQty { get; set; }
        public int? IsShowSizeRun { get; set; }
        public decimal POId { get; set; }
        public decimal MaterialId { get; set; }
        public decimal VendorId { get; set; }
        public string VendorShortNameTw { get; set; }
        public string VendorNameTw { get; set; }
        public string VendorNameEn { get; set; }
        public decimal PurUnitCodeId { get; set; }
        public decimal PCLUnitCodeId { get; set; }
        public int PayCodeId { get; set; }
        public decimal? PaymentCodeId { get; set; }
        public string PaymentNameTw { get; set; }
        public string PaymentNameEn { get; set; }
        public string ParentMaterialNameTw { get; set; }
        public string ParentMaterialNameEn { get; set; }
        public decimal PurUnitPrice { get; set; }
        public decimal DollarCodeId { get; set; }
        public decimal PayDollarCodeId { get; set; }
        public string PayDollarNameTw { get; set; }
        public string PayDollarNameEn { get; set; }
        public decimal? OnHandQty { get; set; }
        public decimal PlanQty { get; set; }
        public int? PaymentPoint { get; set; }
        public string ReceivedWeightUnitNameTw { get; set; }
        public decimal WeightEachUnit { get; set; }
        public string MaterialWeightUnitNameTw { get; set; }
    }
}
