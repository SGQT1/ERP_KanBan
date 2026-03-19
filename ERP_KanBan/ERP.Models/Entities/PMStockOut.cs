using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class PMStockOut
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public int Type { get; set; }
        public string OutRefNo { get; set; }
        public string WarehouseNo { get; set; }
        public string ReceiveRefNo { get; set; }
        public string TypeNo { get; set; }
        public string OrderNo { get; set; }
        public DateTime? BookingDate { get; set; }
        public string RefDeptNo { get; set; }
        public string RefUserName { get; set; }
        public DateTime? OutDate { get; set; }
        public string MaterialName { get; set; }
        public string Spec { get; set; }
        public decimal SubQty { get; set; }
        public string Unit { get; set; }
        public string Barcode { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public int IsClose { get; set; }
        public decimal NWeight { get; set; }
        public decimal GWeight { get; set; }
        public string LocationDesc { get; set; }
    }
}
