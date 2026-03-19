using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class POItemForBOM
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal POId { get; set; }
        public decimal OrdersId { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public int? POTypeId { get; set; }
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

        public string Material { get; set; }
        public string MaterialEng { get; set; }
        public string Currency { get; set; }
        public string Unit { get; set; }

        public bool IsSub { get; set; }
        public string PONo { get; set; }
        public decimal PlanQty { get; set; }
        public decimal OnHandQty { get; set; }
        public string PlanUnitNameTw { get; set; }
        public string Vendor { get; set; }
        public string VendorEng { get; set; }
        public DateTime VendorETD { get; set; }
        public string PurLocale { get; set; }
        public string ReceivingLocale { get; set; }

        public DateTime PODate { get; set; }
        public string Brand { get; set; }
        public string POType { get; set; }
        public string POTypeEng { get; set; }
        public string Purchaser { get; set; }
        public string CustomerOrderNo { get; set; }
        public DateTime? CSD { get; set; }
        public DateTime? LCSD { get; set; }
        public string? ReceivedBarcode { get; set; }
    }
}
