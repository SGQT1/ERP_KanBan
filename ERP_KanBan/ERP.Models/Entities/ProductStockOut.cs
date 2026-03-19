using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ProductStockOut
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal WarehouseId { get; set; }
        public DateTime DateOut { get; set; }
        public string RefOrderNo { get; set; }
        public string StyleNo { get; set; }
        public decimal ShoeSize { get; set; }
        public string ShoeSizeSuffix { get; set; }
        public decimal InnerSize { get; set; }
        public decimal QtyOut { get; set; }
        public string RefNo { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
