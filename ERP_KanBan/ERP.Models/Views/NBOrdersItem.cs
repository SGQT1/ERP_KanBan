using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class NBOrdersItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ModifyUserName { get; set; }
        public string LastUpdateTime { get; set; }
        public string OrderNo { get; set; } // value = CustomerPONo
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
        public string SizeSuffix { get; set; }
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
        public string Season { get; set; }
    }
}
