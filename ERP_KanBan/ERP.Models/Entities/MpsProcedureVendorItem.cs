using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsProcedureVendorItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsProcedureVendorId { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string BankAddress { get; set; }
        public string DollarNameTw { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual MpsProcedureVendor MpsProcedureVendor { get; set; }
    }
}
