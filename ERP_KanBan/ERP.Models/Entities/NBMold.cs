using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class NBMold
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string Factory { get; set; }
        public string StyleNo { get; set; }
        public string PartName { get; set; }
        public string VendorShortNameTw { get; set; }
        public string MaterialId { get; set; }
        public string MaterialName { get; set; }
        public DateTime? PODate { get; set; }
        public string MoldSizeRange { get; set; }
        public decimal POQty { get; set; }
        public decimal? PCSOfPairs { get; set; }
        public string UOM { get; set; }
        public decimal? UnitPrice { get; set; }
        public DateTime? VendorETD { get; set; }
        public string SampleMOQ { get; set; }
        public string ProductionMOQ { get; set; }
        public string SampleLeadTime { get; set; }
        public string ProductionLeadTime { get; set; }
        public string MaterialCountry { get; set; }
        public string MaterialType { get; set; }
        public DateTime? Importdate { get; set; }
        public string Remark { get; set; }
        public decimal PartId { get; set; }
    }
}
