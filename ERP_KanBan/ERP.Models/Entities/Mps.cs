using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class Mps
    {
        public Mps()
        {
            MpsItem = new HashSet<MpsItem>();
        }

        public decimal Id { get; set; }
        public DateTime MadeDate { get; set; }
        public decimal MpsOrdersItemId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsOrdersId { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<MpsItem> MpsItem { get; set; }
    }
}
