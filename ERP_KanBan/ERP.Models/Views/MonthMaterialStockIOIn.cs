using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MonthMaterialStockIOIn
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal IOMonth { get; set; }
        public decimal MaterialStockId { get; set; }
        public decimal IOQty { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? BankingRate { get; set; }
        public decimal IOAmount { get; set; }
        public int? SourceType { get; set; }
    }
}
