using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class Warehouse
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string WarehouseNo { get; set; }
        public string LocationDesc { get; set; }
        public decimal OrgUnitId { get; set; }
        public int TypeCode { get; set; }
        public int CloseOff { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int IsSluggish { get; set; }
        public decimal? BelongCompanyId { get; set; }
        // public string BelongCompany { get; set; }
        // public string MaterialStockType { get; set; }
    }
}
