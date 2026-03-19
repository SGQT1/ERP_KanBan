using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MonthMaterialStockIO
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal IOMonth { get; set; }
        public DateTime? MaxUpdateTime { get; set; }
        public DateTime? CalTime { get; set; }
        public string ModifyUserName { get; set; }
        public int IsReCal { get; set; }
        public decimal? Records { get; set; }
    }
}
