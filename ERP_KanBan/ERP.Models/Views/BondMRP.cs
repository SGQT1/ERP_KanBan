using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class BondMRP
    {
        public decimal? Id { get; set; }
        public int? IsClose { get; set; }
        public string? BondOrderNo { get; set; }
        public decimal? BOMLocaleId { get; set; }
        public string? BondStyleNo { get; set; }
        public string? BondProductName { get; set; }
        public string? ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public DateTime? SalesDate { get; set; }
        public string? BondNo { get; set; }
        public string? RefBondProductName { get; set; }

        public decimal LocaleId { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public decimal CompanyId { get; set; }
        public string CompanyNo { get; set; }
        public string CustomerName { get; set; }
        public string ShoeName { get; set; }
        public decimal OrderQty { get; set; }
        public decimal ShipQty { get; set; }
        public decimal ShortQty { get; set; }
        public string LastNo { get; set; }
        public string OutsoleNo { get; set; }
        public DateTime CSD { get; set; }
        public DateTime? LCSD { get; set; }
        public string CSDYM { get; set; }




    }
}
