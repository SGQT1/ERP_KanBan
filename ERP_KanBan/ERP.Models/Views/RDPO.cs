using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class RDPO
    {   
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public DateTime ProjectPODate { get; set; }
        public int Type { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }

        public int? POCount {get; set;}
        public int? NotApplyCount { get; set; }
    }
}
