using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class SPPurBatchItemExp
    {
        public decimal LocaleId { get; set; }
        public decimal MaterialId { get; set; }
        public decimal CompanyId { get; set; }
        public decimal MaxPurBatchItemId { get; set; }
        public int POHere { get; set; }
    }
}
