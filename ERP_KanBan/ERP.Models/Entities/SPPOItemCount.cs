using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class SPPOItemCount
    {
        public decimal LocaleId { get; set; }
        public string POMonth { get; set; }
        public decimal PaymentLocaleId { get; set; }
        public decimal? CompanyId { get; set; }
        public int Status { get; set; }
        public string ModifyUserName { get; set; }
        public decimal POItemCount { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
