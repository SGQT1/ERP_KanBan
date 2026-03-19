using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class StockIOMonthSch
    {
        public StockIOMonthSch()
        {
            StockIOMonthIn = new HashSet<StockIOMonthIn>();
            StockIOMonthOut = new HashSet<StockIOMonthOut>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal IOMonth { get; set; }
        public DateTime? CalTime { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<StockIOMonthIn> StockIOMonthIn { get; set; }
        public virtual ICollection<StockIOMonthOut> StockIOMonthOut { get; set; }
    }
}
