using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_SAFECODE_ORDERS
    {
        public decimal LocaleId { get; set; }
        public decimal StyleId { get; set; }
        public decimal OrdersId { get; set; }
        public decimal MaterialId { get; set; }
        public string OrderNo { get; set; }
        public DateTime CSD { get; set; }
        public decimal OrderQty { get; set; }
        public string SafeCodeDesc { get; set; }
        public string CustomerOrderNo { get; set; }
        public string StyleNo { get; set; }
        public string MaterialNameTw { get; set; }
        public string CategoryCodeNameTw { get; set; }
        public string CustomerNameTw { get; set; }
        public string ShoeName { get; set; }
        public string CSDYM { get; set; }
        public int Status { get; set; }
        public decimal BrandCodeId { get; set; }
        public int ProductType { get; set; }
        public decimal CustomerId { get; set; }
        public int OrderType { get; set; }
        public string UnitNameTw { get; set; }
        public decimal CompanyId { get; set; }
    }
}
