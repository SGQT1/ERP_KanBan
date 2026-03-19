using System;

namespace ERP.Models.Views
{
    public partial class RDPOItemLog
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public decimal? ProjectPOId { get; set; }
        public decimal ProjectPOItemId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public decimal? VendorId { get; set; }
        public string MaterialName { get; set; }
        public string UnitNameTw { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal? PayUnitPrice { get; set; }
        public string DollarNameTw { get; set; }
        public string DeliveryOrderNo { get; set; }
        public string WarehouseNo { get; set; }
        public string LocationDesc { get; set; }
        public decimal? BankingRate { get; set; }
        public string APMonth { get; set; }
        public string Barcode { get; set; }
    }
}
