using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsDailyAddDuty
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? MpsDailyAddId { get; set; }
        public decimal DutyProcessUnitId { get; set; }
        public int DutyReason { get; set; }
        public decimal DutyQty { get; set; }
        public decimal DutyCostRate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual MpsDailyAdd MpsDailyAdd { get; set; }
    }
}
