using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class ReceivedStandard
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
        public string MaterialName { get; set; }
        public string MaterialNameEn { get; set; }
    }
}
