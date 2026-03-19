using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ReceivedRejectStandard
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public int Priority { get; set; }
        public decimal CategoryCodeId { get; set; }
        public decimal MaterialId { get; set; }
        public decimal VendorId { get; set; }
        public decimal AbovePurQty { get; set; }
        public decimal WarningRate { get; set; }
        public decimal RejectRate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
