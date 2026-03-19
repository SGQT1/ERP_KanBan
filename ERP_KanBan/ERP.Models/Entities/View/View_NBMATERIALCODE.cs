using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class View_NBMATERIALCODE
    {
        public string MaterialName { get; set; }
        public string MaterialNameEng { get; set; }
        public decimal? CategoryCodeId { get; set; }
        public string PartNo { get; set; }
        public string ColorKey { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string UOM { get; set; }
    }
}
