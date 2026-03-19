using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSProcedures
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ProcedureNo { get; set; }
        public string ProcedureName { get; set; }
        public string ProcedureNameTw { get; set; }
        public string ProcedureNameEn { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
