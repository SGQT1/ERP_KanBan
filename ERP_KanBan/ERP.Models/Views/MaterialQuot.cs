using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MaterialQuot
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MaterialId { get; set; }
        public decimal VendorId { get; set; }
        public DateTime QuotDate { get; set; }
        public string VendorQuotNo { get; set; }
        public decimal UnitCodeId { get; set; }
        public string UnitCode { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DollarCodeId { get; set; }
        public string DollarCode { get; set; }
        public decimal PayCodeId { get; set; }
        public string PayCode { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal MinOrderQty { get; set; }
        public int QuotType { get; set; }
        public string QuotationType { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string VendorShortNameTw { get; set; }
        public string ReferenceNo { get; set; }
        public int? ProcessMethod { get; set; }
        public string ContractNo { get; set; }
        public int Enable { get; set; }
        public decimal CustomUnitCodeId { get; set; }
        public decimal CustomTransRate { get; set; }
        public int Confirmed { get; set; }
        public string CategoryNameTw { get; set; }
        public string MaterialName { get; set; }
        public string Locale { get; set; }
        
    }
}
