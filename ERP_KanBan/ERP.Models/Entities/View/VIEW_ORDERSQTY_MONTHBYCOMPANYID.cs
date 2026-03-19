using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_ORDERSQTY_MONTHBYCOMPANYID
    {
        public string CSDYM { get; set; }
        public string BrandTw { get; set; }
        public decimal CompanyId { get; set; }
        public string CompanyNo { get; set; }
        public decimal? SumOrderQty { get; set; }
    }
}
