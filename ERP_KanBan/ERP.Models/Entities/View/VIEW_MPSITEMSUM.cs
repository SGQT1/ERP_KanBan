using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_MPSITEMSUM
    {
        public decimal? SubTotal { get; set; }
        public decimal Id { get; set; }
        public decimal MpsId { get; set; }
        public DateTime PlanDate { get; set; }
        public decimal ProcessId { get; set; }
        public decimal PlanQty { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
    }
}
