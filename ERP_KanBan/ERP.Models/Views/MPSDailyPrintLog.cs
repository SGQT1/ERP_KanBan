using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSDailyPrintLog
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal RefLocaleId { get; set; }
        public string PrintUserName { get; set; }
        public DateTime PrintTime { get; set; }
        public string RefDailyNo { get; set; }
    }
}
