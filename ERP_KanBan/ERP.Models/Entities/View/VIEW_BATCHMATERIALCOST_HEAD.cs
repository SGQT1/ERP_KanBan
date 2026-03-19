using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_BATCHMATERIALCOST_HEAD
    {
        public decimal LocaleId { get; set; }
        public string OrderNo { get; set; }
        public decimal BOMLocaleId { get; set; }
        public decimal OrderQty { get; set; }
        public decimal CompanyId { get; set; }
        public string StyleNo { get; set; }
    }
}
