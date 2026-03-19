using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class PMStockIn
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public int Type { get; set; }
        public string InRefNo { get; set; }
        public string WarehouseNo { get; set; }
        public string ReceiveRefNo { get; set; }
        public string ItemSeqNo { get; set; }
        public string TypeNo { get; set; }
        public DateTime? InDate { get; set; }
        public string MaterialName { get; set; }
        public string Spec { get; set; }
        public decimal SubQty { get; set; }
        public string Unit { get; set; }
        public string Barcode { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal GetValue { get; set; }
        public string DollarName { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string RefOutOrderNo { get; set; }
        public decimal? RefPPOIId { get; set; }
        public decimal NWeight { get; set; }
        public decimal GWeight { get; set; }
        public string LocationDesc { get; set; }
    }
}
