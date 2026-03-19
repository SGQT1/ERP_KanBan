using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsProcess
    {
        public MpsProcess()
        {
            MpsBOPMpsProcess = new HashSet<MpsBOP>();
            MpsBOPMpsProcessNavigation = new HashSet<MpsBOP>();
            MpsLive = new HashSet<MpsLive>();
            MpsProcessOrg = new HashSet<MpsProcessOrg>();
        }

        public decimal Id { get; set; }
        public string ProcessNo { get; set; }
        public string ProcessNameTw { get; set; }
        public string ProcessNameEn { get; set; }
        public int LeadInDays { get; set; }
        public decimal SortKey { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int RelateCapacity { get; set; }
        public int RelateMaterial { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<MpsBOP> MpsBOPMpsProcess { get; set; }
        public virtual ICollection<MpsBOP> MpsBOPMpsProcessNavigation { get; set; }
        public virtual ICollection<MpsLive> MpsLive { get; set; }
        public virtual ICollection<MpsProcessOrg> MpsProcessOrg { get; set; }
    }
}
