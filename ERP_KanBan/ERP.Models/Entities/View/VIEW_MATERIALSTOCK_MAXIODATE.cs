using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_MATERIALSTOCK_MAXIODATE
    {
        public decimal MaterialStockId { get; set; }
        public decimal LocaleId { get; set; }
        public DateTime? MaxIODate { get; set; }
    }
}
