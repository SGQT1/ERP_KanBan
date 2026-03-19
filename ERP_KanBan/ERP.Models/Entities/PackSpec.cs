using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class PackSpec
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? BrandCodeId { get; set; }
        public string Brand { get; set; }
        public int Type { get; set; }
        public string Spec { get; set; }
        public string L { get; set; }
        public string W { get; set; }
        public string H { get; set; }
        public decimal? TextureCodeId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
