using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class PurBatch
    {
        public PurBatch()
        {
            PurBatchItem = new HashSet<PurBatchItem>();
            PurOrdersItem = new HashSet<PurOrdersItem>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string BatchNo { get; set; }
        public DateTime BatchDate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal RefLocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<PurBatchItem> PurBatchItem { get; set; }
        public virtual ICollection<PurOrdersItem> PurOrdersItem { get; set; }
    }
}
