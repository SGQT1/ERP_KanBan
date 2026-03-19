using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VirtualOrders
    {
        public VirtualOrders()
        {
            VirtualOrdersItem = new HashSet<VirtualOrdersItem>();
        }

        public decimal Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNo { get; set; }
        public decimal VirtualCustomerId { get; set; }
        public string StyleNo { get; set; }
        public int? OrderType { get; set; }
        public int? ProductType { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime? ShippingDate { get; set; }
        public decimal VirtualCompanyId { get; set; }
        public string SizeCountryNameTw { get; set; }
        public string MappingSizeCountryNameTw { get; set; }
        public string CustomerOrderNo { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public DateTime CSD { get; set; }
        public decimal OrderQty { get; set; }
        public int PackingType { get; set; }
        public string DollarNameTw { get; set; }
        public string ExportPortName { get; set; }
        public string PackingTypeDesc { get; set; }
        public string CustomerStyleNo { get; set; }
        public string ShoeName { get; set; }
        public string BondProductName { get; set; }
        public int PayType { get; set; }
        public string DeliveryTerms { get; set; }
        public int TransitType { get; set; }
        public string CustomerNameTw { get; set; }
        public string CustomerAddress { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<VirtualOrdersItem> VirtualOrdersItem { get; set; }
    }
}
