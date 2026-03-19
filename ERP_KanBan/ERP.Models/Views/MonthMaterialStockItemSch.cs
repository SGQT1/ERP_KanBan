using System;

namespace ERP.Models.Views
{
    public partial class MonthMaterialStockItemSch
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal IOMonth { get; set; }
        public DateTime? CalTime { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
