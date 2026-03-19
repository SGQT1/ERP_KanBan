using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_STOCKIO_MONTH
    {
        public decimal LocaleId { get; set; }
        public decimal? MaterialId { get; set; }
        public decimal IOYM { get; set; }
        public int Type { get; set; }
        public decimal PCLIOQty { get; set; }
        public string MaterialNameTw { get; set; }
        public string PCLUnitNameTw { get; set; }
    }
}
