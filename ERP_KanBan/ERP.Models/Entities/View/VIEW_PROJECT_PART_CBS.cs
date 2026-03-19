using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_PROJECT_PART_CBS
    {
        public decimal Id { get; set; }
        public decimal ProjectType { get; set; }
        public string ArticleNo { get; set; }
        public int Division { get; set; }
        public string DivisionOther { get; set; }
        public decimal PartId { get; set; }
        public string SeqNo { get; set; }
        public decimal? UnitCodeId { get; set; }
        public decimal? DefaultUsage { get; set; }
        public decimal? ProjectId { get; set; }
        public decimal? ProjectPartId { get; set; }
        public decimal? MaterialId { get; set; }
        public decimal? VendorId { get; set; }
        public decimal? UnitPrice { get; set; }
        public string Remark { get; set; }
        public decimal? DollarCodeId { get; set; }
        public string TempMaterialName { get; set; }
        public decimal? ShoeSize { get; set; }
        public string Suffix { get; set; }
        public decimal? InnerSize { get; set; }
        public decimal UnitUsage { get; set; }
        public string MaterialName { get; set; }
        public string MaterialNameEng { get; set; }
        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string PartNameEn { get; set; }
        public decimal LocaleId { get; set; }
    }
}
