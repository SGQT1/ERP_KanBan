using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MaterialForQuot
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string MaterialName { get; set; }
        public decimal? SamplingMethod { get; set; }
        public decimal? TextureCodeId { get; set; }
        public decimal? CategoryCodeId { get; set; }
        public decimal? SemiGoods { get; set; }
        public int HasQuot { get; set; }
        public decimal? VendorId { get; set; }
        public string VendorShortNameTw { get; set; }
    }
}
