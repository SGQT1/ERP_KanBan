using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class NBPPM
    {
        public decimal Id { get; set; }
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
