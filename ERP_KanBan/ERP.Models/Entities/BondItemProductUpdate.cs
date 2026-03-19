using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class BondItemProductUpdate
    {
        public BondItemProductUpdate()
        {
            BondItemMaterialUpdate = new HashSet<BondItemMaterialUpdate>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal BondId { get; set; }
        public decimal BondItemProductId { get; set; }
        public DateTime ApplyDate { get; set; }
        public int ApplyType { get; set; }
        public int SeqNo { get; set; }
        public string ProductNo { get; set; }
        public string ProductNameTw { get; set; }
        public string ProductNameEn { get; set; }
        public string ProductNameCn { get; set; }
        public decimal OriginalCountryCodeId { get; set; }
        public decimal UnitCodeId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        public decimal DollarCodeId { get; set; }
        public decimal Qty { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual Bond Bond { get; set; }
        public virtual BondItemProduct BondItemProduct { get; set; }
        public virtual ICollection<BondItemMaterialUpdate> BondItemMaterialUpdate { get; set; }
    }
}
