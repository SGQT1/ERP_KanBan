using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ProjectItemControl
    {
        public decimal Id { get; set; }
        public decimal ProjectId { get; set; }
        public decimal? ShoeSize { get; set; }
        public string Suffix { get; set; }
        public string ItemControlName { get; set; }
        public DateTime ScheduleDate { get; set; }
        public DateTime? ActualDate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual Project Project { get; set; }
    }
}
