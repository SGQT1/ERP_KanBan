using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class TransferItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal TransferId { get; set; }
        public decimal ReceivedLogId { get; set; }
        public decimal TransferQty { get; set; }
        public decimal TargetCompanyId { get; set; }
        public string MaterialNameTwCust { get; set; }
        public string MaterialNameEnCust { get; set; }
        public decimal TransferQtyCust { get; set; }
        public string UnitCodeNameTwCust { get; set; }
        public string DollarCodeNameTwCust { get; set; }
        public decimal UnitPriceCust { get; set; }
        public decimal TaxRateCust { get; set; }
        public decimal AmountCust { get; set; }
        public string WeiUnitCodeNameTwCust { get; set; }
        public decimal NetWeight { get; set; }
        public decimal GrossWeight { get; set; }
        public int SubCount { get; set; }
        public string Mark { get; set; }
        public string Remark { get; set; }
        public int TransferType { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public string? ContainerNo { get; set; }
        public string? ShipmentNo { get; set; }
        public DateTime? ShippingDate { get; set; }
        public string? Vendor { get; set; } 
        public string? OrderNo { get; set; } 
        public string? StyleNo { get; set; }
        public DateTime? CSD { get; set; }
        public string? PurUnitNameTw { get; set; }
        public decimal? POItemId { get; set; }
        public string? PONo { get; set; }
        public decimal? MaterialId { get; set; }
        public decimal? ParentMaterialId { get; set; }
        public string? ParentMaterialNameTw { get; set; }
        public decimal? UnitCodeId { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? MaterialNameTw { get; set; }
        public string? MaterialNameEn { get; set; }
        public decimal? HasReceivedLogId { get; set; }
        public string TargetCompany { get; set; }
        public int? SamplingMethod { get; set; }
        public decimal? ReceivedLogLocaleId { get; set; }
        public decimal? POItemLocaleId { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public decimal? ReceivedQty { get; set; }
        public decimal? IQCGetQty { get; set; }
        public decimal? StockQty { get; set; }
        public decimal? RefLocaleId { get; set; }
        public string? ReceivedBarcode { get; set; }
        public string? Recipient { get; set; }
        public decimal? PaymentLocaleId { get; set; }
        public DateTime? OBDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string? ShippingPortName { get; set; }
        public string? TargetPortName { get; set; }

        public decimal? PayQty { get; set; }
        public decimal? FreeQty { get; set; }

        public decimal? TargetReceivedLogId { get; set; }
        public decimal? TargetReceivedQty { get; set; }
        public DateTime? TargetReceivedDate { get; set; }

        public decimal? PurLocaleId { get; set; }
        public decimal? ReceivingLocaleId { get; set; }

        public decimal? RefMaterialId { get; set; }
        public decimal? RefParentMaterialId { get; set; }
    }
}
