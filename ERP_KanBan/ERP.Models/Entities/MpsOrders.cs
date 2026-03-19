using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsOrders
    {
        public MpsOrders()
        {
            MpsLive = new HashSet<MpsLive>();
            MpsOrdersItem = new HashSet<MpsOrdersItem>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string OrderNo { get; set; }
        public decimal OrderQty { get; set; }
        public decimal Qty { get; set; }
        public decimal MpsArticleId { get; set; }
        public decimal ProcessSetId { get; set; }
        public string StyleNo { get; set; }
        public decimal SizeCountryCodeId { get; set; }
        public decimal IncreaseRate { get; set; }
        public DateTime ETD { get; set; }
        public DateTime CSD { get; set; }
        public int BaseOn { get; set; }
        public string CustomerNameTw { get; set; }
        public int ProcessType { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<MpsLive> MpsLive { get; set; }
        public virtual ICollection<MpsOrdersItem> MpsOrdersItem { get; set; }
    }
}
