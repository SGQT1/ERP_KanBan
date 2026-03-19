using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class PMStock
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string WarehouseNo { get; set; }
        public string TypeNo { get; set; }
        public string MaterialName { get; set; }
        public string Spec { get; set; }
        public string Barcode { get; set; }
        public decimal StockQty { get; set; }
        public string Unit { get; set; }
        public decimal StockValue { get; set; }
        public string DollarName { get; set; }
        public DateTime? StockDate { get; set; }
        public decimal StandardPrice { get; set; }
        public decimal NWeight { get; set; }
        public decimal GWeight { get; set; }
        public string LocationDesc { get; set; }
    }
}
