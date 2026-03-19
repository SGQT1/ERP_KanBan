using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_PCL_SIZERUN_USAGE
    {
        public decimal ArticleId { get; set; }
        public double ArticleInnerSize { get; set; }
        public decimal UnitUsage { get; set; }
        public decimal ArticlePartId { get; set; }
        public decimal LocaleId { get; set; }
        public Guid? GUID { get; set; }
    }
}
