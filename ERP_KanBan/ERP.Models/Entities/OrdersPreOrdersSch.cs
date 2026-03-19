using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class OrdersPreOrdersSch
    {
        public decimal LocaleId { get; set; }
        public decimal OrdersId { get; set; }
        public string BrandTw { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public DateTime CSD { get; set; }
        public DateTime? LCSD { get; set; }
        public DateTime ETD { get; set; }
        public DateTime? BOMOKDate { get; set; }
        public DateTime? BOMWarningDate { get; set; }
        public DateTime? ClrSWOKDate { get; set; }
        public DateTime? ClrSWWarningDate { get; set; }
        public DateTime? PurOKDate { get; set; }
        public DateTime? PurWarningDate { get; set; }
        public DateTime? CfmShoeOKDate { get; set; }
        public DateTime? CfmShoeWarningDate { get; set; }
        public DateTime? SpecOKDate { get; set; }
        public DateTime? SpecWarningDate { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
