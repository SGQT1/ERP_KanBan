using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_ARTSTY_OSQTY
    {
        public string BrandTw { get; set; }
        public string ArticleNo { get; set; }
        public string ShoeName { get; set; }
        public string CSD { get; set; }
        public string ETD { get; set; }
        public string LCSD { get; set; }
        public decimal CompanyId { get; set; }
        public int ProductType { get; set; }
        public int OrdersTransfer { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal? SaleQty { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Discount { get; set; }
        public DateTime? MaxSaleDate { get; set; }
    }
}
