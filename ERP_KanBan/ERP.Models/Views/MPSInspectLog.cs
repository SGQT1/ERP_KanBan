using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSInspectLog
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
        public int DoPay { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string Remark { get; set; }
        public string PayMonth { get; set; }
        public decimal AddFreeQty { get; set; }
        public decimal DiscountRate { get; set; }
        public string WarehouseNo { get; set; }
        public decimal? DayOfMonth { get; set; }
    }
}
