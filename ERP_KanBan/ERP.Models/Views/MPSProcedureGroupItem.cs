using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MPSProcedureGroupItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsProcedureGroupId { get; set; }
        public string ProcedureNameTw { get; set; }
        public decimal? PairsStandardTime { get; set; }
        public decimal? PairsStandardPrice { get; set; }
        public string StyleNo { get; set; }
        public string OutsourceProcess { get; set; }
        public string GroupNameLocal { get; set; }
    }
}
