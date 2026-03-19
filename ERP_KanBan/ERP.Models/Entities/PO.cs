using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class PO
    {
        public PO()
        {
            POItem = new HashSet<POItem>();
            ProcessPO = new HashSet<ProcessPO>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public DateTime PODate { get; set; }
        public string BatchNo { get; set; }
        public int SeqId { get; set; }
        public decimal VendorId { get; set; }
        public int IsShowSizeRun { get; set; }
        public int ShowAlternateType { get; set; }
        public DateTime VendorETD { get; set; }
        public int IsAllowPartial { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string PhotoURLDescTw { get; set; }
        public string PhotoURL { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string POTeam { get; set; }

        public virtual ICollection<POItem> POItem { get; set; }
        public virtual ICollection<ProcessPO> ProcessPO { get; set; }
    }
}
