using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSProcedureVendorItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MPSProcedureVendorId { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string BankAddress { get; set; }
        public string DollarNameTw { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
