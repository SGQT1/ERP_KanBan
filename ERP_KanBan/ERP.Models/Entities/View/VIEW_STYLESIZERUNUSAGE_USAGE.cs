using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_STYLESIZERUNUSAGE_USAGE
    {
        public decimal LocaleId { get; set; }
        public decimal OrdersId { get; set; }
        public decimal MaterialId { get; set; }
        public decimal UnitUsage { get; set; }
        public decimal UnitCodeId { get; set; }
        public double ArticleInnerSize { get; set; }
        public string UnitNameTw { get; set; }
        public string MaterialNameTw { get; set; }
        public decimal? Usage { get; set; }
    }
}
