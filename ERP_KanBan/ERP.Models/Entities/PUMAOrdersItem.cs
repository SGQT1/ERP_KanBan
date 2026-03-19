using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class PUMAOrdersItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string OrderNo { get; set; }
        public string CustomerPONo { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCONo { get; set; }
        public string UCustomerCode { get; set; }
        public string UCustomerName { get; set; }
        public string UCustomerCONo { get; set; }
        public string OrderReleaseDate { get; set; }
        public string TotalQty { get; set; }
        public string StyleNo { get; set; }
        public string StyleDesc { get; set; }
        public string Color { get; set; }
        public string ColorDesc { get; set; }
        public string CustomerStyleNo { get; set; }
        public string CustomerDesc { get; set; }
        public string RHD { get; set; }
        public string EHD { get; set; }
        public string CHD { get; set; }
        public string LCHD { get; set; }
        public string LastUpdate { get; set; }
        public string SupplierCode { get; set; }
        public string FactoryCode { get; set; }
        public string CounryOrigin { get; set; }
        public string Brand { get; set; }
        public string ULTCustomerDSNo { get; set; }
        public string CreateDate { get; set; }
        public string Currency { get; set; }
        public string HarmonizedSystemNo { get; set; }
        public string NotHarmonizedSystemNo { get; set; }
        public string DeliveryAddress { get; set; }
        public string ShipmentMode { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string Carrier { get; set; }
        public string OrderChar { get; set; }
        public string SizeTableName { get; set; }
        public string KeySize { get; set; }
        public string Size { get; set; }
        public string Quantity { get; set; }
        public string Sur { get; set; }
        public string Sku { get; set; }
        public string Ocp { get; set; }
        public string DeliveryGroup { get; set; }
        public string DestinationName { get; set; }
        public string VasRemarks { get; set; }
        public string PitRemarks { get; set; }
        public string OPD { get; set; }
        public string Season { get; set; }
    }
}
