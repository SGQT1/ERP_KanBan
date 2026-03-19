using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsDailyAddCost
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsDailyAddId { get; set; }
        public string MaterialNameTw { get; set; }
        public string UnitNameTw { get; set; }
        public decimal PlanUsage { get; set; }
        public decimal IOUsage { get; set; }
        public decimal UnitPrice { get; set; }
        public string DollarNameTw { get; set; }
        public decimal ExchangeRate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual MpsDailyAdd MpsDailyAdd { get; set; }
    }
}
