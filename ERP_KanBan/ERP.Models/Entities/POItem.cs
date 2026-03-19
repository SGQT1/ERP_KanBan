using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class POItem
    {
        public POItem()
        {
            POItemSize = new HashSet<POItemSize>();
            ProcessPO = new HashSet<ProcessPO>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal POId { get; set; }
        public decimal OrdersId { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public int? POType { get; set; }
        public decimal MaterialId { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal DollarCodeId { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitCodeId { get; set; }
        public int PayCodeId { get; set; }
        public decimal PurLocaleId { get; set; }
        public decimal ReceivingLocaleId { get; set; }
        public decimal PaymentLocaleId { get; set; }
        public decimal? PaymentCodeId { get; set; }
        public int? PaymentPoint { get; set; }
        public decimal? ParentMaterialId { get; set; }
        public int IsOverQty { get; set; }
        public int SamplingMethod { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal PayDollarCodeId { get; set; }
        public int Status { get; set; }
        public DateTime FactoryETD { get; set; }
        public string Remark { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public decimal? CompanyId { get; set; }
        public decimal? PurBatchItemId { get; set; }
        public string PONo { get; set; }
        public int? AlternateType { get; set; }
        public int? MRPVersion { get; set; }
        public virtual PO PO { get; set; }
        public virtual ICollection<POItemSize> POItemSize { get; set; }
        public virtual ICollection<ProcessPO> ProcessPO { get; set; }
    }
}
