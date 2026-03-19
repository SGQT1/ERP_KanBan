using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class OrdersGBSItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal OrdersGBSId { get; set; }
        public string SizeBreakdown { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public string PackingTypeDesc { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
