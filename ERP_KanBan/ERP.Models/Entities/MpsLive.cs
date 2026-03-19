using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsLive
    {
        public MpsLive()
        {
            MpsLiveItem = new HashSet<MpsLiveItem>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal MpsOrdersId { get; set; }
        public decimal ProcessId { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual MpsOrders MpsOrders { get; set; }
        public virtual MpsProcess MpsProcess { get; set; }
        public virtual ICollection<MpsLiveItem> MpsLiveItem { get; set; }
    }
}
