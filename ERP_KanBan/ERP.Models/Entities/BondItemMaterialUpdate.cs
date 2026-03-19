using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class BondItemMaterialUpdate
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal BondItemProductUpdateId { get; set; }
        public decimal BondItemMaterialId { get; set; }
        public string MaterialNo { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public string MaterialNameCn { get; set; }
        public decimal OriginalCountryCodeId { get; set; }
        public decimal UnitCodeId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        public decimal DollarCodeId { get; set; }
        public decimal TotalUsage { get; set; }
        public decimal? TotalAmount { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int SumSeqNo { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual BondItemMaterial BondItemMaterial { get; set; }
        public virtual BondItemProductUpdate BondItemProductUpdate { get; set; }
    }
}
