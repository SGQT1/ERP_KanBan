using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSProcedurePO
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public int Type { get; set; }
        public decimal MpsProcedureVendorId { get; set; }
        public string OrderNo { get; set; }
        public int SeqId { get; set; }
        public string PONo { get; set; }
        public string StyleNo { get; set; }
        public decimal MpsProcedureGroupId { get; set; }
        public string MpsProcedureGroupNameTw { get; set; }
        public DateTime PODate { get; set; }
        public decimal UnitPrice { get; set; }
        public string DollarNameTw { get; set; }
        public decimal Qty { get; set; }
        public string PurUnitName { get; set; }
        public DateTime VendorETD { get; set; }
        public int PayCodeId { get; set; }
        public decimal ReceivingLocaleId { get; set; }
        public decimal PaymentLocaleId { get; set; }
        public decimal PaymentCodeId { get; set; }
        public int? PaymentPoint { get; set; }
        public int IsOverQty { get; set; }
        public int IsAllowPartial { get; set; }
        public int? IsShowSizeRun { get; set; }
        public int SamplingMethod { get; set; }
        public int Status { get; set; }
        public string SpecDesc { get; set; }
        public string Remark { get; set; }
        public string PhotoURL { get; set; }
        public string PhotoURLDescTw { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal PayQty { get; set; }
        public string WarehouseNo { get; set; }
        public int? PriceType { get; set; }
        public string MPSVendor { get; set; }
        public string Vendor { get; set; }
        public string MPSVendorShortName { get; set; }
        public decimal? Amount { get; set; }
        public decimal? DayOfMonth { get; set; }
        public decimal? TotalOutsouceQty { get; set; }
        public decimal? OrderQty { get; set; }
        public int? PayType { get; set; }
    }
}
