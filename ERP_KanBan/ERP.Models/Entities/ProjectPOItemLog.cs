using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ProjectPOItemLog
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public decimal? ProjectPOId { get; set; }
        public decimal ProjectPOItemId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public decimal? VendorId { get; set; }
        public string MaterialName { get; set; }
        public string UnitNameTw { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal? PayUnitPrice { get; set; }
        public string DollarNameTw { get; set; }
        public string DeliveryOrderNo { get; set; }
        public string WarehouseNo { get; set; }
        public string LocationDesc { get; set; }
        public decimal? BankingRate { get; set; }
        public string APMonth { get; set; }
        public string Barcode { get; set; }
    }
}
