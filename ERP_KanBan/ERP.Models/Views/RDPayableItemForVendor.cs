using System;

namespace ERP.Models.Views
{
    public partial class RDPayableItemForVendor
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal ProjectPOId { get; set; }
        public int SeqNo { get; set; }
        public decimal RefLocaleId { get; set; }
        public string WorkOrderNo { get; set; }
        public string StyleNo { get; set; }
        public decimal DevPairs { get; set; }
        public decimal PlanPairs { get; set; }
        public string MaterialNameTw { get; set; }
        public decimal VendorId { get; set; }
        public decimal PlanQty { get; set; }
        public string UnitNameTw { get; set; }
        public int PayCodeId { get; set; }
        public decimal PaymentLocaleId { get; set; }
        public string Remark { get; set; }
        public DateTime FirstProjectPODate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public DateTime VendorETD { get; set; }
        public decimal? ReceivedLocaleId { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public decimal? ReceivedQty { get; set; }
        public int? ReceivedConfirmed { get; set; }
        public string RvdUserName { get; set; }
        public DateTime? RvdUpdateTime { get; set; }
        public decimal QuotUnitPrice { get; set; }
        public decimal PayUnitPrice { get; set; }
        public decimal ExtraAmount { get; set; }
        public string DollarNameTw { get; set; }
        public decimal Amount { get; set; }
        public decimal? PayQty { get; set; }
        public decimal Discount { get; set; }
        public decimal? APAmount { get; set; }
        public int? DoAP { get; set; }
        public string APMonth { get; set; }
        public string ShoeName { get; set; }
        public string CFMUserName { get; set; }
        public DateTime? CFMTime { get; set; }
        public int IsCFM { get; set; }
        public decimal DiscountRate { get; set; }

        public DateTime? ProjectPODate { get; set; }
        public int? Type { get; set; }
        public string ProjectPONo { get; set; }
        public string VendorNameTw { get; set; }
        public string Brand { get; set; }
        public string CloseMonth { get; set; }
        
    }
}
