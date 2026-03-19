using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsProcessSet
    {
        public MpsProcessSet()
        {
            MpsBOP = new HashSet<MpsBOP>();
        }

        public decimal Id { get; set; }
        public string ProcessSetName { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<MpsBOP> MpsBOP { get; set; }
    }
}
