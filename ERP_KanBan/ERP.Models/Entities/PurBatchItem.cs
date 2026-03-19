using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class PurBatchItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal BatchId { get; set; }
        public decimal OrdersId { get; set; }
        public decimal MaterialId { get; set; }
        public decimal PlanUnitCodeId { get; set; }
        public decimal PlanQty { get; set; }
        public decimal RefQuotId { get; set; }
        public decimal VendorId { get; set; }
        public decimal PurUnitPrice { get; set; }
        public decimal DollarCodeId { get; set; }
        public decimal PayCodeId { get; set; }
        public decimal PurUnitCodeId { get; set; }
        public decimal PurQty { get; set; }
        public decimal PurLocaleId { get; set; }
        public decimal ReceivingLocaleId { get; set; }
        public decimal PaymentLocaleId { get; set; }
        public decimal? POItemId { get; set; }
        public decimal OnHandQty { get; set; }
        public decimal? ParentMaterialId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal PayDollarCodeId { get; set; }
        public decimal RefLocaleId { get; set; }
        public decimal RefItemId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public int? AlternateType { get; set; }
        public int? Status { get; set; }
        public int? MRPVersion { get; set; }
        public virtual PurBatch PurBatch { get; set; }
    }
}
