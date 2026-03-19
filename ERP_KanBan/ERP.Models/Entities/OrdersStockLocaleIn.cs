using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class OrdersStockLocaleIn
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal OrdersStockLocaleId { get; set; }
        public string OrdersStockLocaleCode { get; set; }
        public decimal CTNLabelId { get; set; }
        public decimal CTNLabelItemId { get; set; }
        public string CTNLabelCode { get; set; }
        public DateTime StockInTime { get; set; }
        public string Remark { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string ModifyUserName { get; set; }
        public int Version { get; set; }
        public string OrderNo { get; set; }
        public int SeqNo { get; set; }
        public string SubLabelCode { get; set; }
    }
}
