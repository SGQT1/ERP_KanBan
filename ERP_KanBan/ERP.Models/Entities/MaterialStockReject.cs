using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MaterialStockReject
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal RefLocaleId { get; set; }
        public decimal POItemId { get; set; }
        public decimal ReceivedLogId { get; set; }
        public decimal OriStockIOId { get; set; }
        public decimal? StockIOId { get; set; }
        public string PONo { get; set; }
        public string VendorNameTw { get; set; }
        public string OrderNo { get; set; }
        public decimal MaterialId { get; set; }
        public DateTime RejectDate { get; set; }
        public string RejectNo { get; set; }
        public decimal RejectQty { get; set; }
        public decimal PCLUnitCodeId { get; set; }
        public decimal RejectCount { get; set; }
        public string RejectRemark { get; set; }
        public int RepairType { get; set; }
        public decimal? PCLUnitPrice { get; set; }
        public decimal? CutTotalPrice { get; set; }
        public decimal? RepairQty { get; set; }
        public decimal? StockDollarCodeId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ReceivedLog ReceivedLog { get; set; }
        public virtual StockIO StockIO { get; set; }
    }
}
