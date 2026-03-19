using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsProcedureGroupItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsProcedureGroupId { get; set; }
        public string ProcedureNameTw { get; set; }
        public decimal? PairsStandardTime { get; set; }
        public decimal? PairsStandardPrice { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual MpsProcedureGroup MpsProcedureGroup { get; set; }
    }
}
