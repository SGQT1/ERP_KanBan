using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_PO_RECEIVEDLOGEXCH
    {
        public decimal LocaleId { get; set; }
        public decimal ReceivedId { get; set; }
        public decimal POLocaleId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string ShippingListNo { get; set; }
        public decimal ShippingListVendorId { get; set; }
        public string ShipListVendorNameTw { get; set; }
        public string ShipListVendorNameEn { get; set; }
        public string ShipListVenShortNameTw { get; set; }
        public decimal POItemId { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public string PONo { get; set; }
        public decimal VendorId { get; set; }
        public string VendorNameTw { get; set; }
        public string VendorNameEn { get; set; }
        public decimal PurUnitCodeId { get; set; }
        public string PurUnitNameTw { get; set; }
        public string PurUnitNameEn { get; set; }
        public decimal Qty { get; set; }
        public int PayCodeId { get; set; }
        public decimal PaymentLocaleId { get; set; }
        public decimal PaymentCodeId { get; set; }
        public string PaymentNameTw { get; set; }
        public string PaymentNameEn { get; set; }
        public int PaymentPoint { get; set; }
        public decimal DollarCodeId { get; set; }
        public string DollarNameTw { get; set; }
        public string DollarNameEn { get; set; }
        public decimal PayDollarCodeId { get; set; }
        public string PayDollarNameTw { get; set; }
        public string PayDollarNameEn { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal SubTotalPrice { get; set; }
        public string Remark { get; set; }
        public decimal APId { get; set; }
        public DateTime? QCDate { get; set; }
        public decimal IQCFlag { get; set; }
        public decimal? IQCGetQty { get; set; }
        public decimal? IQCTestQty { get; set; }
        public decimal IQCPassQty { get; set; }
        public decimal? IQCRejectionQty { get; set; }
        public int? IQCResult { get; set; }
        public int? SamplingMethod { get; set; }
        public string OrderNo { get; set; }
        public int IsAccounting { get; set; }
        public string TaiwanInvoiceNo { get; set; }
        public int DayOfMonth { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public DateTime VendorETD { get; set; }
        public decimal FilterLocaleId { get; set; }
    }
}
