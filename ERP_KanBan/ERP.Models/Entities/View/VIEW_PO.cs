using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_PO
    {
        public decimal POId { get; set; }
        public decimal LocaleId { get; set; }
        public decimal VendorId { get; set; }
        public int IsShowSizeRun { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string PhotoURLDescTw { get; set; }
        public string PhotoURL { get; set; }
        public decimal OrdersId { get; set; }
        public decimal MaterialId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DollarCodeId { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitCodeId { get; set; }
        public int PayCodeId { get; set; }
        public decimal PayDollarCodeId { get; set; }
        public decimal PurLocaleId { get; set; }
        public decimal ReceivingLocaleId { get; set; }
        public string POItemRemark { get; set; }
        public decimal PaymentLocaleId { get; set; }
        public string PONo { get; set; }
        public decimal POItemId { get; set; }
        public DateTime VendorETD { get; set; }
        public DateTime FactoryETD { get; set; }
        public int Status { get; set; }
        public DateTime PODate { get; set; }
        public string BatchNo { get; set; }
        public int IsAllowPartial { get; set; }
        public string BarcodeNo { get; set; }
        public string PayCodeDesc { get; set; }
        public string Remark { get; set; }
        public string IsAllowPartialDesc { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public int? POType { get; set; }
    }
}
