using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class StockIOSize
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal StockIOId { get; set; }
        public decimal ShoeSize { get; set; }
        public string ShoeSizeSuffix { get; set; }
        public double ShoeInnerSize { get; set; }
        public decimal PCLQty { get; set; }
        public decimal PurQty { get; set; }
        public decimal ReLogSizeItemId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string DisplaySize { get; set; }
    }
}
