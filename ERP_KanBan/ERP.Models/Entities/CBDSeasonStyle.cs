using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class CBDSeasonStyle
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid? msrepl_tran_version { get; set; }
        public string Season { get; set; }
        public string StyleNo { get; set; }
        public string BrandTw { get; set; }
        public decimal? CBDPrice { get; set; }
        public string SampleSize { get; set; }
        public string SampleSizeSuffix { get; set; }
        public decimal? SampleInnerSize { get; set; }
        public decimal? ProductionPrice { get; set; }
        public decimal? SamplePrice { get; set; }
        public int IsCFM { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
