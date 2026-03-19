using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class BondProduct
    {
        public BondProduct()
        {
            BondBOM = new HashSet<BondBOM>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ProductNo { get; set; }
        public string ProductNameTw { get; set; }
        public string ProductNameEn { get; set; }
        public string ProductNameCn { get; set; }
        public decimal OriginalCountryCodeId { get; set; }
        public decimal UnitCodeId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        public decimal DollarCodeId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string ShoeClassName { get; set; }
        public int? ShoeDivisionType { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<BondBOM> BondBOM { get; set; }
    }
}
