using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class BondProductChinaContrast
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public string UnitName { get; set; }
        public string BondProductName { get; set; }
        public decimal? WeightEachUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public string DollarName { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
