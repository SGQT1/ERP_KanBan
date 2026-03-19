using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSProcedureQuot
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsProcedureGroupId { get; set; }
        public decimal MpsProcedureVendorId { get; set; }
        public string StyleNo { get; set; }
        public DateTime QuotDate { get; set; }
        public string VendorQuotNo { get; set; }
        public string PurUnitNameTw { get; set; }
        public decimal UnitPrice { get; set; }
        public string DollarNameTw { get; set; }
        public decimal PayCodeId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string VendorShortNameTw { get; set; }
        public string ReferenceNo { get; set; }
        public int? ProcessMethod { get; set; }
        public string ContractNo { get; set; }
        public int Enable { get; set; }
        public string CustomUnitNameTw { get; set; }
        public decimal? CustomTransRate { get; set; }
        public int Confirmed { get; set; }
        public string MpsProcedureGroupName { get; set; }
        public string VendorNameLocal { get; set; }
        public string VendorNameTw { get; set; }
        public decimal PaymentCodeId { get; set; }
    }
}
