using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ShipmentLog
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal OrdersId { get; set; }
        public decimal SaleQty { get; set; }
        public DateTime? CloseDate { get; set; }
        // public decimal? RefLocaleId { get; set; }
        public string Season { get; set; }
        public decimal? CustomerId { get; set; }
        public decimal? CompanyId { get; set; }
        public decimal? BrandCodeId { get; set; }
        public string Customer { get; set; }
        public string OrderNo { get; set; }
        public string CompanyNo { get; set; }
        public string BrandCode { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }

    }
}
