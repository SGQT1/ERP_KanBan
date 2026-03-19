using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class BondProductChinaContrast
    {
        public decimal? Id { get; set; }
        public decimal LocaleId { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public string? UnitName { get; set; }
        public string? BondProductName { get; set; }
        public decimal? WeightEachUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? DollarName { get; set; }
        public string? ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public decimal? CategoryCodeId { get; set; }
        public decimal TextureCodeId { get; set; }
    }
}
