using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class RDMaterialStockOut
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public int Type { get; set; }
        public string OutRefNo { get; set; }
        public string WarehouseNo { get; set; }
        public string ReceiveRefNo { get; set; }
        public string TypeNo { get; set; }
        public DateTime? IODate { get; set; }
        public string MaterialName { get; set; }
        public string Spec { get; set; }
        public decimal SubQty { get; set; }
        public string Unit { get; set; }
        public string Barcode { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
         public decimal NWeight { get; set; }
        public decimal GWeight { get; set; }
        public string LocationDesc { get; set; }

        public string OrderNo { get; set; }
        public DateTime? BookingDate { get; set; }
        public string RefDeptNo { get; set; }
        public string RefUserName { get; set; }
        public int IsClose { get; set; }
       
        
    }
}
