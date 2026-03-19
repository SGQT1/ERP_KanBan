using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class BondMaterial
    {
        public BondMaterial()
        {
            BondBOM = new HashSet<BondBOM>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string MaterialNo { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public string MaterialNameCn { get; set; }
        public decimal OriginalCountryNameCodeId { get; set; }
        public decimal UnitCodeId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        public decimal DollarCodeId { get; set; }
        public int ForPacking { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<BondBOM> BondBOM { get; set; }
    }
}
