using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class NBSCCCode_Upper
    {
        public decimal Id { get; set; }
        public string MAT_ { get; set; }
        public string Commodity_Type { get; set; }
        public string Description { get; set; }
        public string Unit_Of_Measure { get; set; }
        public string Color_Key { get; set; }
        public string NB_Color_Name { get; set; }
        public string Color_Family { get; set; }
        public string Vendor_Name { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
