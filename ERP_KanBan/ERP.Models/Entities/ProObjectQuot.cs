using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ProObjectQuot
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal ObjectId { get; set; }
        public decimal OtherVendorId { get; set; }
        public DateTime QuotDate { get; set; }
        public string VendorQuotNo { get; set; }
        public string UnitName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DollarCodeId { get; set; }
        public decimal PayCodeId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal MinOrderQty { get; set; }
        public string ReferenceNo { get; set; }
        public int ProcessMethod { get; set; }
        public string ContractNo { get; set; }
        public int Enable { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual OtherVendor OtherVendor { get; set; }
        public virtual ProObject ProObject { get; set; }
    }
}
