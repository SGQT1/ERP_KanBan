using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class OrdersNewStyle
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string StyleNo { get; set; }
        public string OrderNo { get; set; }
        public DateTime CSD { get; set; }
        public decimal? CompanyId { get; set; }
        public int? ProductType { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
