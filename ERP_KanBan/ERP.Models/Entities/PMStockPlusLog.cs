using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class PMStockPlusLog
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid? msrepl_tran_version { get; set; }
        public DateTime? StockDate { get; set; }
        public string WarehouseNo { get; set; }
        public string MaterialName { get; set; }
        public string LocationDesc { get; set; }
        public string Barcode { get; set; }
        public string Unit { get; set; }
        public decimal? Qty { get; set; }
        public decimal? PlusQty { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
