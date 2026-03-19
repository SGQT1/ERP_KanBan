using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsProcedurePOPrintLog
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal RefPOId { get; set; }
        public decimal RefLocaleId { get; set; }
        public string PrintUserName { get; set; }
        public DateTime PrintTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual MpsProcedurePO Ref { get; set; }
    }
}
