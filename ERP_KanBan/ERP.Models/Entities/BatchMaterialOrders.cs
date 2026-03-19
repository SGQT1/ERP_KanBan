using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class BatchMaterialOrders
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string OrderNo { get; set; }
        public decimal RefLocaleId { get; set; }
        public decimal OrdersId { get; set; }
        public int IsMRP { get; set; }
        public int IsCost { get; set; }
        public DateTime? ExchangeDate { get; set; }
    }
}
