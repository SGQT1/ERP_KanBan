using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class BondItemProduct
    {
        public BondItemProduct()
        {
            BondItemMaterial = new HashSet<BondItemMaterial>();
            BondItemProductUpdate = new HashSet<BondItemProductUpdate>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal BondId { get; set; }
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
        public decimal? Qty { get; set; }
        public decimal ImportQty { get; set; }
        public decimal ExportQty { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual Bond Bond { get; set; }
        public virtual ICollection<BondItemMaterial> BondItemMaterial { get; set; }
        public virtual ICollection<BondItemProductUpdate> BondItemProductUpdate { get; set; }
    }
}
