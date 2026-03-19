using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_MPSBOP
    {
        public string ProcessSetName { get; set; }
        public string ProcessNo { get; set; }
        public string ProcessNameTw { get; set; }
        public string ProcessNameEn { get; set; }
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal ProcessId { get; set; }
        public decimal ChildId { get; set; }
        public decimal ProcessSetId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public int LeadInDays { get; set; }
        public int RelateCapacity { get; set; }
        public int RelateMaterial { get; set; }
    }
}
