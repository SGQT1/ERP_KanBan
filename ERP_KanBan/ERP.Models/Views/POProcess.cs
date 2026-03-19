using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class POProcess
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal BatchId { get; set; }
        public decimal OrdersId { get; set; }
        public decimal MaterialId { get; set; }
        public decimal PlanUnitCodeId { get; set; }
        public decimal PlanQty { get; set; }
        public decimal RefQuotId { get; set; }
        public decimal? VendorId { get; set; }
        public decimal? LastVendorId { get; set; }
        public decimal PurUnitPrice { get; set; }
        public decimal DollarCodeId { get; set; }
        public decimal PayCodeId { get; set; }
        public decimal PurUnitCodeId { get; set; }
        public decimal PurQty { get; set; }
        public decimal PurLocaleId { get; set; }
        public decimal ReceivingLocaleId { get; set; }
        public decimal PaymentLocaleId { get; set; }
        public decimal? POItemId { get; set; }
        public decimal OnHandQty { get; set; }
        public decimal? ParentMaterialId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal PayDollarCodeId { get; set; }
        public decimal RefLocaleId { get; set; }
        public decimal RefItemId { get; set; }
        public int? AlternateType { get; set; }
        public int? POStatus { get; set; }
        
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public string BatchNo { get; set; }
        public string OrderNo { get; set; }
        public string Vendor { get; set;}
        public string LastVendor { get; set;}
        public string Seq { get; set;}
        public string PONo { get; set;}
        public decimal? POQty { get; set; }
        public decimal? NewPurQty { get; set; }
        public decimal? CategoryCodeId { get; set; }
    }

    public partial class PurchaseStatus
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal OrdersId { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public decimal MaterialId { get; set; }
        public decimal? ParentMaterialId { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public decimal PlanUnitCodeId { get; set; }
        public decimal PlanQty { get; set; }
        
        public int? AlternateType { get; set; }
        public string Seq { get; set;}
        public decimal? CategoryCodeId { get; set; }
        public decimal? PurQty { get; set; }
        public decimal? POQty { get; set; }
        public decimal OnHandQty { get; set; }  // 主副料，靠ParentMaterialId判斷

        public string LastVendor { get; set;}        
        public decimal? LastVendorId { get; set; }
        public decimal? POItemId { get; set; }

        public decimal? PurUnitPrice { get; set; }
        public decimal? DollarCodeId { get; set; }
        public decimal? PurUnitCodeId { get; set; }
        
    }
}
