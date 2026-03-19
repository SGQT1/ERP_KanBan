using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class JITProductionLineWorkHour
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string LinesNo { get; set; }
        public string LinesName { get; set; }
        public DateTime WorkDate { get; set; }
        public decimal DayWorkHour { get; set; }
        public decimal NightWorkHour { get; set; }
        public decimal Workers { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
