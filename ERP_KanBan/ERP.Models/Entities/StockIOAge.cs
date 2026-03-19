using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class StockIOAge
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string MaterialNameTw { get; set; }
        public decimal? Qty { get; set; }
        public string Days { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string PCLUnitNameTw { get; set; }
        public decimal? Amount { get; set; }
    }
}
