using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MaterialTotalUsageCost
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string MaterialNameTw { get; set; }
        public string OrderNo { get; set; }
        public DateTime TTLDate { get; set; }
        public decimal TotalUsageCost { get; set; }
        public string StockDollarNameTw { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
