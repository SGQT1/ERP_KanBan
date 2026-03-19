using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class SOM
    {
        public decimal Id { get; set; }
        public decimal ParentId { get; set; }
        public decimal ChildId { get; set; }
        public int SeqNo { get; set; }
        public decimal? Qty { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string ItemGroupCode { get; set; }
        public string ParentGroupCode { get; set; }

        public virtual Company Locale { get; set; }
        public virtual Material Material { get; set; }
        public virtual Material MaterialNavigation { get; set; }
    }
}
