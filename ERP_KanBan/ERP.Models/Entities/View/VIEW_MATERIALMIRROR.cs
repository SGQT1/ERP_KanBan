using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_MATERIALMIRROR
    {
        public decimal Id { get; set; }
        public string MaterialName { get; set; }
        public string MaterialNameEng { get; set; }
        public decimal LocaleId { get; set; }
        public decimal CategoryCodeId { get; set; }
        public string CategoryNameTw { get; set; }
        public string CategoryCodeType { get; set; }
        public decimal TextureCodeId { get; set; }
        public int SemiGoods { get; set; }
        public string OtherName { get; set; }
        public int SamplingMethod { get; set; }
        public string MaterialNo { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
