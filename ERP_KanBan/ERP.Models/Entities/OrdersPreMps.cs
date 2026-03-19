using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class OrdersPreMps
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string BrandTw { get; set; }
        public int DateType { get; set; }
        public decimal BOMOKDays { get; set; }
        public decimal BOMWarningDays { get; set; }
        public decimal? ClrSWOKDays { get; set; }
        public decimal? ClrSWWarningDays { get; set; }
        public decimal PurOKDays { get; set; }
        public decimal PurWarningDays { get; set; }
        public decimal? CfmShoeOKDays { get; set; }
        public decimal? CfmShoeWarningDays { get; set; }
        public decimal? SpecOKDays { get; set; }
        public decimal? SpecWarningDays { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
