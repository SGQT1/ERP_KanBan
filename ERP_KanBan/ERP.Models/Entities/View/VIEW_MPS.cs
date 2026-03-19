using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_MPS
    {
        public decimal MpsOrdersItemId { get; set; }
        public decimal Id { get; set; }
        public string ProcessNo { get; set; }
        public string ProcessNameTw { get; set; }
        public string BeginPlanDate { get; set; }
        public string EndPlanDate { get; set; }
        public decimal LocaleId { get; set; }
        public int LeadInDays { get; set; }
        public decimal MpsOrdersId { get; set; }
    }
}
