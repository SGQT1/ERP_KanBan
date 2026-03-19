using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_PCL_SIZERUN_QTY
    {
        public decimal LocaleId { get; set; }
        public decimal OrdersItemId { get; set; }
        public decimal OrdersItemQty { get; set; }
        public decimal? ArticlePartId { get; set; }
        public decimal? OrdersId { get; set; }
        public decimal? UnitUsage { get; set; }
        public decimal OrderQty { get; set; }
        public decimal MaterialId { get; set; }
    }
}
