using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VirtualQuotation
    {
        public decimal Id { get; set; }
        public string SizeCountryNameTw { get; set; }
        public decimal SizeBeginning { get; set; }
        public string SizeBeginningSuffix { get; set; }
        public decimal SizeBeginningInner { get; set; }
        public decimal SizeEndding { get; set; }
        public string SizeEnddingSuffix { get; set; }
        public decimal SizeEnddingInner { get; set; }
        public decimal FactoryPriceIntel { get; set; }
        public string DollarNameTw { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public int? Confirmed { get; set; }
        public string BondProductName { get; set; }
        public decimal? ProductClass { get; set; }
        public int? ProductType { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
