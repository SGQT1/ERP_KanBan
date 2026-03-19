using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsLiveItem
    {
        public MpsLiveItem()
        {
            MpsDaily = new HashSet<MpsDaily>();
            MpsLiveItemSize = new HashSet<MpsLiveItemSize>();
        }

        public decimal Id { get; set; }
        public decimal MpsLiveId { get; set; }
        public DateTime PlanDate { get; set; }
        public decimal PlanQty { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual MpsLive MpsLive { get; set; }
        public virtual ICollection<MpsDaily> MpsDaily { get; set; }
        public virtual ICollection<MpsLiveItemSize> MpsLiveItemSize { get; set; }
    }
}
