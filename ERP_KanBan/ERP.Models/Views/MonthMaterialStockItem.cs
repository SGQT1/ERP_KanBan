using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MonthMaterialStockItem
    {
        public decimal? Id { get; set; }
        public decimal? LocaleId { get; set; }
        public decimal? IOMonth { get; set; }
        public DateTime MaxUpdateTime { get; set; }
        public DateTime? CalTime { get; set; }
        public string? ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public int? IsReCal { get; set; }
        public decimal? Records { get; set; }
    }
}
