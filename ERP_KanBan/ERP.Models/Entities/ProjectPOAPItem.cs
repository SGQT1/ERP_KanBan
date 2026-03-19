using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ProjectPOAPItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal ProjectPOAPId { get; set; }
        public decimal RefPPOIId { get; set; }
        public decimal RefLocaleId { get; set; }
        public int ProjectPOType { get; set; }
        public decimal PayQty { get; set; }
        public string UnitNameTw { get; set; }
        public decimal PayUnitPrice { get; set; }
        public decimal ExtraAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal APAmount { get; set; }
        public string DollarNameTw { get; set; }
        public int APConfirmed { get; set; }
        public string APCfmUserName { get; set; }
        public DateTime? APCfmUpdateTime { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime? PayDate { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ProjectPOAP ProjectPOAP { get; set; }
    }
}
