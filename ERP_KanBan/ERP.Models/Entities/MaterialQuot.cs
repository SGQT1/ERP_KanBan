using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MaterialQuot
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MaterialId { get; set; }
        public decimal VendorId { get; set; }
        public DateTime QuotDate { get; set; }
        public string VendorQuotNo { get; set; }
        public decimal UnitCodeId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DollarCodeId { get; set; }
        public decimal PayCodeId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal MinOrderQty { get; set; }
        public int QuotType { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string VendorShortNameTw { get; set; }
        public string ReferenceNo { get; set; }
        public int? ProcessMethod { get; set; }
        public string ContractNo { get; set; }
        public int Enable { get; set; }
        public decimal CustomUnitCodeId { get; set; }
        public decimal CustomTransRate { get; set; }
        public int Confirmed { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual Material Material { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
