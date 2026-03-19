using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsBOP
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal ProcessId { get; set; }
        public decimal ChildId { get; set; }
        public decimal ProcessSetId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual Company Locale { get; set; }
        public virtual MpsProcess MpsProcess { get; set; }
        public virtual MpsProcess MpsProcessNavigation { get; set; }
        public virtual MpsProcessSet MpsProcessSet { get; set; }
    }
}
