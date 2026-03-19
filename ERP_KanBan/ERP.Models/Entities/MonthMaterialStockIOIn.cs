using System;
using System.Collections.Generic;

namespace ERP.Models.Entities
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
        public Guid msrepl_tran_version { get; set; }
    }
}
