using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSAP
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsReceivedLogId { get; set; }
        public decimal MpsProcedurePOId { get; set; }
        public string PayMonth { get; set; }
        public string Vendor { get; set; }
        public decimal PaymentLocaleId { get; set; }
        public decimal ChargeQty { get; set; }
        public string PurUnitName { get; set; }
        public decimal UnitPrice { get; set; }
        public string DollarNameTw { get; set; }
        public decimal? AdjustAmount { get; set; }
        public decimal SubAmount { get; set; }
        public string Remark { get; set; }
        public string OrderNo { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
