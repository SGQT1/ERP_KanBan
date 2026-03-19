using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class UnitTransRate
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal CodeId { get; set; }
        public decimal Rate { get; set; }
        public decimal TransCodeId { get; set; }
        public decimal ReversedRate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
