using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsAccount
    {
        public MpsAccount()
        {
            MpsAccountItem = new HashSet<MpsAccountItem>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string PayMonth { get; set; }
        public string Vendor { get; set; }
        public string DollarNameTw { get; set; }
        public decimal Amount { get; set; }
        public decimal? AdjustAmount { get; set; }
        public decimal AmountTTL { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<MpsAccountItem> MpsAccountItem { get; set; }
    }
}
