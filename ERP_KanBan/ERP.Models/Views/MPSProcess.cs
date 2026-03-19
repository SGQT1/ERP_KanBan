using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable
// 拖外單位(流程)
namespace ERP.Models.Views
{
    public partial class MPSProcess
    {
        public decimal Id { get; set; }
        public string ProcessNo { get; set; }
        public string ProcessNameTw { get; set; }
        public string ProcessNameEn { get; set; }
        public int LeadInDays { get; set; }
        public decimal SortKey { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int RelateCapacity { get; set; }
        public int RelateMaterial { get; set; }
        public decimal LocaleId { get; set; }

        public string DisplayProcess { get; set; }
    }
}
