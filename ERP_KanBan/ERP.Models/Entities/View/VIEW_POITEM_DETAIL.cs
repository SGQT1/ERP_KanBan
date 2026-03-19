using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_POITEM_DETAIL
    {
        public decimal POItemId { get; set; }
        public decimal LocaleId { get; set; }
        public decimal POId { get; set; }
        public string BatchNo { get; set; }
        public int? SeqId { get; set; }
        public string PONo { get; set; }
        public decimal? VendorId { get; set; }
        public string VendorShortNameTw { get; set; }
        public string VendorNameTw { get; set; }
        public DateTime? VendorETD { get; set; }
        public decimal OrdersId { get; set; }
        public string OrderNo { get; set; }
        public DateTime? CSD { get; set; }
        public DateTime? ETD { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DollarCodeId { get; set; }
        public string DollarNameTW { get; set; }
        public decimal PurQty { get; set; }
        public int PayCodeId { get; set; }
        public decimal PurUnitCodeId { get; set; }
        public string PurUnitNameTw { get; set; }
        public decimal PurLocaleId { get; set; }
        public decimal ReceivingLocaleId { get; set; }
        public decimal PaymentLocaleId { get; set; }
        public decimal? PaymentCodeId { get; set; }
        public int? PaymentPoint { get; set; }
        public decimal? ParentMaterialId { get; set; }
        public int IsOverQty { get; set; }
        public int SamplingMethod { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal PayDollarCodeId { get; set; }
        public int Status { get; set; }
        public DateTime FactoryETD { get; set; }
        public string POItemRemark { get; set; }
        public decimal PlanUnitCodeId { get; set; }
        public string PlanUnitNameTw { get; set; }
        public decimal PlanQty { get; set; }
    }
}
