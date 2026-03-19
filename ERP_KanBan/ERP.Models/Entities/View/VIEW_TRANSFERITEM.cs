using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_TRANSFERITEM
    {
        public decimal TransferItemId { get; set; }
        public decimal LocaleId { get; set; }
        public decimal TransferId { get; set; }
        public decimal ReceivedLogId { get; set; }
        public string Mark { get; set; }
        public decimal TargetCompanyId { get; set; }
        public int TransferType { get; set; }
        public string MaterialNameTwCust { get; set; }
        public string MaterialNameEnCust { get; set; }
        public string DollarCodeNameTwCust { get; set; }
        public decimal TransferQtyCust { get; set; }
        public decimal UnitPriceCust { get; set; }
        public decimal TaxRateCust { get; set; }
        public decimal AmountCust { get; set; }
        public decimal TransferQty { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string ModifyUserName { get; set; }
        public string Remark { get; set; }
        public int SubCount { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal NetWeight { get; set; }
        public string WeiUnitCodeNameTwCust { get; set; }
        public string UnitCodeNameTwCust { get; set; }
        public string ShipmentNo { get; set; }
        public string ContainerNo { get; set; }
        public DateTime OBDate { get; set; }
        public decimal? UnitPricePercent { get; set; }
        public decimal? PaymentLocaleId { get; set; }
        public DateTime ShippingDate { get; set; }
        public string Vessel { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string ShippingPortName { get; set; }
        public string TargetPortName { get; set; }
        public decimal TargetReceivedLogId { get; set; }
        public DateTime TargetReceivedDate { get; set; }
        public decimal TargetReceivedQty { get; set; }
    }
}
