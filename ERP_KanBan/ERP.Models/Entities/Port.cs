using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class Port
    {
        public decimal Id { get; set; }
        public string PortNo { get; set; }
        public string PortName { get; set; }
        public decimal PortVarietyCodeId { get; set; }
        public string PortNameEng { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual Company Locale { get; set; }
    }
}
