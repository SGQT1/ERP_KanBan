using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class CBSItemIn
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal CBSId { get; set; }
        public int Division { get; set; }
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string MaterialName { get; set; }
        public string UnitMea { get; set; }
        public decimal PerPairUsage { get; set; }
        public decimal UnitCost { get; set; }
        public string DollarName { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal UsageRate { get; set; }
        public decimal PerCost { get; set; }
        public int SubCal { get; set; }
        public string Vendor { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
