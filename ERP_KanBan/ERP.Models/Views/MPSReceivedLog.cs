using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSReceivedLog
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsProcedurePOId { get; set; }
        public decimal RefLocaleId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal ChargeQty { get; set; }
        public decimal FreeQty { get; set; }
        public decimal QCBackQty { get; set; }
        public int doPay { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string Remark { get; set; }
        public string PayMonth { get; set; }
        public decimal AddFreeQty { get; set; }
        public decimal DiscountRate { get; set; }
        public string WarehouseNo { get; set; }
        public decimal? DayOfMonth { get; set; }

        public decimal MpsProcedureVendorId { get; set; }
        public string MPSVendorShortName { get; set; }
        public string OrderNo { get; set; }
        public string PONo { get; set; }
        public string StyleNo { get; set; }
        public string MpsProcedureGroupNameTw { get; set; }
        public decimal Qty { get; set; }
        public string PurUnitName { get; set; }
        public int Status { get; set; }
        public string MPSVendor { get; set; }
        public DateTime VendorETD { get; set; }
        public decimal PaymentLocaleId { get; set; }
        public decimal SumQty { get; set; }
    }
}
