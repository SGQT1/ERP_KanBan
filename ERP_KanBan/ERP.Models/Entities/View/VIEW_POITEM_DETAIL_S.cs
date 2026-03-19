using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_POITEM_DETAIL_S
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
        public string VendorNameEn { get; set; }
        public DateTime? VendorETD { get; set; }
        public decimal OrdersId { get; set; }
        public string OrderNo { get; set; }
        public DateTime? CSD { get; set; }
        public DateTime? ETD { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string MaterialNameEng { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DollarCodeId { get; set; }
        public string DollarNameTW { get; set; }
        public string DollarNameEn { get; set; }
        public decimal PurQty { get; set; }
        public int PayCodeId { get; set; }
        public decimal PurUnitCodeId { get; set; }
        public string PurUnitNameTw { get; set; }
        public string PurUnitNameEn { get; set; }
        public decimal PurLocaleId { get; set; }
        public decimal ReceivingLocaleId { get; set; }
        public decimal PaymentLocaleId { get; set; }
        public decimal? PaymentCodeId { get; set; }
        public string PaymentNameTw { get; set; }
        public string PaymentNameEn { get; set; }
        public int? PaymentPoint { get; set; }
        public decimal? ParentMaterialId { get; set; }
        public string ParentMaterialName { get; set; }
        public string ParentMaterialNameEng { get; set; }
        public int IsOverQty { get; set; }
        public int SamplingMethod { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal PayDollarCodeId { get; set; }
        public string PayDollarNameTw { get; set; }
        public string PayDollarNameEn { get; set; }
        public int Status { get; set; }
        public DateTime FactoryETD { get; set; }
        public string POItemRemark { get; set; }
        public decimal PlanUnitCodeId { get; set; }
        public string PlanUnitNameTw { get; set; }
        public string PlanUnitNameEn { get; set; }
        public decimal PlanQty { get; set; }
        public int? IsAllowPartial { get; set; }
        public int? IsShowSizeRun { get; set; }
        public decimal OnHandQty { get; set; }
        public decimal? CompanyId { get; set; }
        public decimal RefLocaleId { get; set; }
        public DateTime? PODate { get; set; }
        public string ModifyUserName { get; set; }
    }
}
