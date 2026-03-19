using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_RECEIVEDLOG_ITEM
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string OrderNo { get; set; }
        public decimal POItemId { get; set; }
        public decimal RefLocaleId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public decimal ShippingListVendorId { get; set; }
        public string VendorShortNameTw { get; set; }
        public string VendorNameTw { get; set; }
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
        public DateTime VendorETD { get; set; }
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
    }
}
