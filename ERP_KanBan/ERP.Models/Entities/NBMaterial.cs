using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class NBMaterial
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string CommodityType { get; set; }
        public string Description { get; set; }
        public string UOM { get; set; }
        public string ColorKey { get; set; }
        public string NBColorName { get; set; }
        public string ColorFamily { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }
    }
}
