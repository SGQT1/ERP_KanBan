using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_ORDERSQTY_MONTHSUM
    {
        public string CSDYM { get; set; }
        public string BrandTw { get; set; }
        public decimal LocaleId { get; set; }
        public string LocaleNo { get; set; }
        public decimal? SumQrderQty { get; set; }
    }
}
