using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ProjectExcel
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public decimal ProjectId { get; set; }
        public int? SeqNo { get; set; }
        public string PartName { get; set; }
        public string MaterialName { get; set; }
        public string Spec { get; set; }
        public string Color { get; set; }
        public string Vendor { get; set; }
        public string Process { get; set; }
        public decimal? PartId { get; set; }
        public decimal? MaterialId { get; set; }
        public decimal? VendorId { get; set; }
    }
}
