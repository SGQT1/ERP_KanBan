using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ProjectPO
    {
        public ProjectPO()
        {
            ProjectPOItem = new HashSet<ProjectPOItem>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public DateTime ProjectPODate { get; set; }
        public int Type { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<ProjectPOItem> ProjectPOItem { get; set; }
    }
}
