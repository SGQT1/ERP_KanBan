using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsReceivedLogSizeItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsReceivedLogId { get; set; }
        public string DisplaySize { get; set; }
        public decimal QCQty { get; set; }
        public decimal? FreeQty { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual MpsReceivedLog MpsReceivedLog { get; set; }
    }
}
