using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class NoticeGroup
    {
        public NoticeGroup()
        {
            NoticeGroupItem = new HashSet<NoticeGroupItem>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string GroupName { get; set; }
        public string SMTPServer { get; set; }
        public int Type { get; set; }
        public int Notify { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<NoticeGroupItem> NoticeGroupItem { get; set; }
    }
}
