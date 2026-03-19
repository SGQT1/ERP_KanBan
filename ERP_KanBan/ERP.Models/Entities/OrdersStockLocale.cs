using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class OrdersStockLocale
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string FactoryNo { get; set; }
        public string BlockNo { get; set; }
        public string AreaNo { get; set; }
        public string LocaleNo { get; set; }
        public decimal MaxQty { get; set; }
        public string Barcode { get; set; }
        public string LocaleDesc { get; set; }
        public int Disable { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string ModifyUserName { get; set; }
    }
}
