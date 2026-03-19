using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class StockIOMonthIn
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal IOMonth { get; set; }
        public decimal MATERIALSTOCKId { get; set; }
        public decimal IOQty { get; set; }
        public decimal IOAmount { get; set; }
        public int? SourceType { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual StockIOMonthSch StockIOMonthSch { get; set; }
    }
}
