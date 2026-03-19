using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class BondBOM
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal BondProductId { get; set; }
        public decimal BondMaterialId { get; set; }
        public decimal UnitUsage { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual BondMaterial BondMaterial { get; set; }
        public virtual BondProduct BondProduct { get; set; }
    }
}
