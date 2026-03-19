using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class BondProductChina
    {
        public decimal Id { get; set; }
        public string BondProductName { get; set; }
        public string BondProductNo { get; set; }
        public string BondUnitName { get; set; }
        public decimal? BondWeightEachUnit { get; set; }
        public decimal? BondUnitPrice { get; set; }
        public string BondDollarName { get; set; }
        public decimal? BondTaxRate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
