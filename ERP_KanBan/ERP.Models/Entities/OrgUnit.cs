using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class OrgUnit
    {
        public OrgUnit()
        {
            MpsProcessOrg = new HashSet<MpsProcessOrg>();
            Staff = new HashSet<Staff>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string UnitNo { get; set; }
        public string UnitNameTw { get; set; }
        public string UnitNameEn { get; set; }
        public DateTime FoundingDate { get; set; }
        public decimal ManagerStaffId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public decimal ParentId { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<MpsProcessOrg> MpsProcessOrg { get; set; }
        public virtual ICollection<Staff> Staff { get; set; }
    }
}
