using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class REP_PROJECT_CONTROL
    {
        public decimal ProjectId { get; set; }
        public string ProjectTypeNameTw { get; set; }
        public string ArticleNo { get; set; }
        public string ColorCode { get; set; }
        public string ColorDesc { get; set; }
        public DateTime OpenDate { get; set; }
        public string ShoeName { get; set; }
        public string BrandNameTw { get; set; }
        public decimal ShoeSize { get; set; }
        public string Suffix { get; set; }
        public decimal LeftQty { get; set; }
        public decimal RightQty { get; set; }
        public string DisplaySize { get; set; }
        public decimal LocaleId { get; set; }
        public string ItemControlName { get; set; }
        public DateTime ScheduleDate { get; set; }
        public DateTime? ActualDate { get; set; }
        public DateTime ExpectedShippingDate { get; set; }
        public DateTime? PlanShippingDate { get; set; }
        public DateTime? ShippingDate { get; set; }
    }
}
