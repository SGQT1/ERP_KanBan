using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_PURBATCH2
    {
        public decimal? OrdersId { get; set; }
        public decimal? MaterialId { get; set; }
        public decimal? UnitCodeId { get; set; }
        public decimal? LocaleId { get; set; }
        public decimal? BatchId { get; set; }
        public decimal? CurrentPlanQty { get; set; }
        public decimal? PreviousPlanQty { get; set; }
        public decimal? PurQty { get; set; }
        public decimal NewPurQty { get; set; }
        public decimal? ParentMaterialId { get; set; }
        public int HasParentId { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
    }
}
