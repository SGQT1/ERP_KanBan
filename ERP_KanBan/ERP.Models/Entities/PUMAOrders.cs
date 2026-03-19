using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class PUMAOrders
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string CustomerName { get; set; }
        public string StyleNo { get; set; }
        public string OrderNo { get; set; }
        public DateTime? CSD { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? LCSD { get; set; }
        public string Season { get; set; }
    }
}
