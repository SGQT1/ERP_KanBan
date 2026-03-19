using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MonthMaterialStockItemIn
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal IOMonth { get; set; }
        public decimal MATERIALSTOCKId { get; set; }
        public decimal IOQty { get; set; }
        public decimal IOAmount { get; set; }
        public int? SourceType { get; set; }
    }
}
