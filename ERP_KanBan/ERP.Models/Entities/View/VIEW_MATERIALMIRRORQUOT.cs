using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_MATERIALMIRRORQUOT
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public int Enable { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialName { get; set; }
        public decimal CategoryCodeId { get; set; }
        public string CategoryNameTw { get; set; }
        public decimal VendorId { get; set; }
        public string VendorNameTw { get; set; }
        public string VendorShortNameTw { get; set; }
        public DateTime QuotDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int QuotType { get; set; }
        public decimal UnitCodeId { get; set; }
        public string UnitNameTw { get; set; }
        public decimal DollarCodeId { get; set; }
        public string DollarNameTw { get; set; }
        public decimal UnitPrice { get; set; }
        public int SamplingMethod { get; set; }
        public decimal PayCodeId { get; set; }
        public int ProcessMethod { get; set; }
        public string ContractNo { get; set; }
        public decimal PaymentCodeId { get; set; }
        public string ReferenceNo { get; set; }
        public string VendorQuotNo { get; set; }
        public int? PaymentPoint { get; set; }
        public decimal MinOrderQty { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal CustomUnitCodeId { get; set; }
        public decimal CustomTransRate { get; set; }
        public int Confirmed { get; set; }
        public string ProcessMethodNameTw { get; set; }
    }
}
