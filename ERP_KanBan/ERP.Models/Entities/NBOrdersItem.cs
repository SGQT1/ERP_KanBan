using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class NBOrdersItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string CustomerPONo { get; set; }
        public string Brand { get; set; }
        public string Region { get; set; }
        public string Market { get; set; }
        public string VendorShortName { get; set; }
        public string ProductType { get; set; }
        public string ProductClass { get; set; }
        public string PONo { get; set; }
        public string POReferenceNo { get; set; }
        public string CustomerOrderNo { get; set; }
        public string POType { get; set; }
        public string LineNos { get; set; }
        public string StylePartNo { get; set; }
        public string StyleStatus { get; set; }
        public string ShipMode { get; set; }
        public string ColorWidth { get; set; }
        public string Size { get; set; }
        public string Quantity { get; set; }
        public string FOBPrice { get; set; }
        public string Customer { get; set; }
        public string Warehouse { get; set; }
        public string OrigReqXFD { get; set; }
        public string OrigCFMXFD { get; set; }
        public string POReleaseDate { get; set; }
        public string ExpOrActXFD { get; set; }
        public string ReasonCode { get; set; }
        public string StyleDescription { get; set; }
        public string Model { get; set; }
        public string SizeSuffix { get; set; }
        public string Season { get; set; }
    }
}
