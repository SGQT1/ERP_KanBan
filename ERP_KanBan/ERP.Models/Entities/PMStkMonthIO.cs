using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class PMStkMonthIO
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string YM { get; set; }
        public string MaterialName { get; set; }
        public string Spec { get; set; }
        public decimal BeginStockQty { get; set; }
        public decimal StockInQty { get; set; }
        public decimal StockOutQty { get; set; }
        public decimal EndStockQty { get; set; }
        public string Unit { get; set; }
        public decimal BeginStockAmount { get; set; }
        public decimal StockInAmount { get; set; }
        public decimal StockOutAmount { get; set; }
        public decimal EndStockAmount { get; set; }
        public string DollarName { get; set; }
    }
}
