using System;

namespace ERP.Models.Views.Setting
{
    public class ProjectOutsoleItem
    {
        public int Id { get; set; }
        public int ProjectOutsoleId { get; set; }
        public double ShoeSize { get; set; }
        public string ShoeSizeSuffix { get; set; }
        public double? ShoeSizeSortKey { get; set; }
        public int Qty { get; set; }
        public DateTime? MadeDate { get; set; }
        public double? Cost { get; set; }
        public decimal LocaleId { get; set; }
        public string Map2MDSize { get; set; }
        public string Map2EVASize { get; set; }
    }
}