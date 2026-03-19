using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class OutsoleItem
    {
        public decimal Id { get; set; }
        public decimal OutsoleId { get; set; }
        public decimal ShoeSize { get; set; }
        public string ShoeSizeSuffix { get; set; }
        public decimal? ShoeSizeSortKey { get; set; }
        public int Qty { get; set; }
        public DateTime? MadeDate { get; set; }
        public decimal? Cost { get; set; }
        public decimal LocaleId { get; set; }
        public string Map2MDSize { get; set; }
        public string Map2EVASize { get; set; }
    }
}
