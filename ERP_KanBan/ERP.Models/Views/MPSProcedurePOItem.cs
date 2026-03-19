using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSProcedurePOItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsProcedurePOId { get; set; }
        public string ProcedureNameTw { get; set; }
        public decimal? PairsStandardTime { get; set; }
        public decimal? PairsStandardPrice { get; set; }
    }
}
