using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_PUR_ORDER
    {
        public decimal Id { get; set; }
        public string OrderNo { get; set; }
        public decimal LocaleId { get; set; }
        public decimal CompanyId { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public DateTime CSD { get; set; }
        public DateTime ETD { get; set; }
    }
}
