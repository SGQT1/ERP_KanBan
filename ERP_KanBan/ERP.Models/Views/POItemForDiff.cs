using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class POItemForDiff
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal POId { get; set; }
        public decimal OrdersId { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public int? POType { get; set; }
        public decimal MaterialId { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal DollarCodeId { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitCodeId { get; set; }
        public int PayCodeId { get; set; }
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
        public string Remark { get; set; }
        public decimal? CompanyId { get; set; }
        public decimal VendorId { get; set; }
        public string VendorNameTw { get; set; }
        public int? IsAllowPartial { get; set; }
        public DateTime? VendorETD { get; set; }
        public DateTime? POItemVendorETD { get; set; }
        public string Material { get; set; }
        public string Currency { get; set; }
        public string Unit { get; set; }
        public string RefOrderNo { get; set; }
        public string RefPONo { get; set; }
        public string ReceivedBarcode { get; set; }
        public string PCLUnitNameTw { get; set; }
        public decimal? PCLUnitCodeId { get; set; }
        public DateTime? PODate { get; set; }
        public decimal? PurBatchItemId { get; set; }
        public string PONo { get; set; }
        public int? AlternateType { get; set; }
        public int? IsShowSizeRun { get; set; }
        
        public string MaterialEng { get; set; }
        public decimal PlanQty { get; set; }
        public decimal? PurQty { get; set; }
        public decimal? PayQty { get; set; }
        public DateTime? LCSD { get; set; }
        public string ShoeName { get; set; }
        public string Part { get; set; }

        public string BatchNo { get; set; }
        public int SeqId { get; set; }
        public string POTeam { get; set; }
        public decimal? ReceivedLogId { get; set; }
        public decimal? ReceivedQty { get; set; }
        public string NewPONo { get; set; }
        public decimal? CategoryCodeId { get; set; }
        public decimal BOMQty { get; set; }
        public int Diff { get; set; }
        public string ParentMaterial { get; set; }
        public decimal? BatchId { get; set; }
        public decimal? Amount { get; set; }
    }
}
