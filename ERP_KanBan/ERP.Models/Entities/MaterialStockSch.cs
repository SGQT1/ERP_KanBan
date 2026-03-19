using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MaterialStockSch
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public DateTime TTLDate { get; set; }
    }
}
