using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class CBSOut
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal CBSId { get; set; }
        public decimal Labor { get; set; }
        public decimal Overhead { get; set; }
        public decimal Profit { get; set; }
        public decimal TTL { get; set; }
        public decimal TotalMaterialCost { get; set; }
        public decimal? SurfaceRate { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
