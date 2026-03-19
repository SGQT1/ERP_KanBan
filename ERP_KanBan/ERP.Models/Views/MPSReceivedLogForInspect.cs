using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSReceivedLogForInspect
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsProcedureVendorId { get; set; }
        public string OrderNo { get; set; }
        public string PONo { get; set; }
        public string StyleNo { get; set; }
        public string MpsProcedureGroupNameTw { get; set; }
        public decimal Qty { get; set; }
        public string PurUnitName { get; set; }
        public int Status { get; set; }
        public string MPSVendor { get; set; }
        public string WarehouseNo { get; set; }
        
        public decimal? ReceivedId { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public decimal? ReceivedQty { get; set; }
        public decimal? SumQty { get; set; }
        public string MPSVendorShortName { get; set; }
        public decimal? MPSReceivedLogId { get; set; }
        public decimal? DayOfMonth { get; set; }
        public int IsReceived { get; set; }
        public int DoPay { get; set; }
        public decimal QCBackQty { get; set; }
        public decimal ChargeQty { get; set; }
        public decimal FreeQty { get; set; }
    }
}
