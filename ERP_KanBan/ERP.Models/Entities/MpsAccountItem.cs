using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsAccountItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsAccountId { get; set; }
        public decimal RefLocaleId { get; set; }
        public decimal MpsReceivedLogId { get; set; }
        public decimal MpsProcedurePOId { get; set; }
        public decimal ChargeQty { get; set; }
        public string PurUnitName { get; set; }
        public decimal UnitPrice { get; set; }
        public string DollarNameTw { get; set; }
        public decimal? AdjustAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal SubAmount { get; set; }
        public string Remark { get; set; }
        public string OrderNo { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual MpsAccount MpsAccount { get; set; }
    }
}
