using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MaterialStockItemSize
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
        public string DisplaySize { get; set; }
    }
}
