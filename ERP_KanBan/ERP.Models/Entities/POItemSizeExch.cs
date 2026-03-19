using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class POItemSizeExch
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal POItemId { get; set; }
        public string DisplaySize { get; set; }
        public decimal Qty { get; set; }
        public int? SeqNo { get; set; }
        public decimal TransferItemId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual POExch POExch { get; set; }
    }
}
