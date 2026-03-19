using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_PROCESSPO_COSTSHARE
    {
        public decimal ProcessPOId { get; set; }
        public decimal POItemId { get; set; }
        public decimal LocaleId { get; set; }
        public decimal PurQty { get; set; }
        public decimal MaterialCost { get; set; }
        public decimal? CostUnitPrice { get; set; }
        public decimal SumPurIOQty { get; set; }
    }
}
