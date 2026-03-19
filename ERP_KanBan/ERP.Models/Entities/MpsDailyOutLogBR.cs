using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsDailyOutLogBR
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string LabelCode { get; set; }
        public string DailyNo { get; set; }
        public string OrderNo { get; set; }
        public string MaterialName { get; set; }
        public decimal? Qty { get; set; }
        public string UnitName { get; set; }
        public decimal? MaterialStockId { get; set; }
        public decimal? StockIOId { get; set; }
        public DateTime? IODate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
