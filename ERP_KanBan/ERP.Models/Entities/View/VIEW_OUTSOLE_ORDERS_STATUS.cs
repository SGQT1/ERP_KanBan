using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_OUTSOLE_ORDERS_STATUS
    {
        public string OrderNo { get; set; }
        public DateTime CSD { get; set; }
        public DateTime ETD { get; set; }
        public decimal? OrdersItemQty { get; set; }
        public decimal OutsoleId { get; set; }
        public string OutsoleNo { get; set; }
        public string OutsoleColorDescTW { get; set; }
        public string CompanyNo { get; set; }
        public string StyleNo { get; set; }
        public string CustomerOrderNo { get; set; }
        public decimal MadeByCompanyId { get; set; }
        public string MadeByCompanyNo { get; set; }
        public decimal LocaleId { get; set; }
        public decimal CompanyId { get; set; }
        public decimal OutsoleInnerSize { get; set; }
        public string OutsoleSizeText { get; set; }
        public decimal? OrdersId { get; set; }
        public decimal OrderQty { get; set; }
    }
}
