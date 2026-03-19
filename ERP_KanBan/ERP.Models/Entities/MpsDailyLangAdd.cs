using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsDailyLangAdd
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsDailyAddId { get; set; }
        public decimal MpsStyleItemId { get; set; }
        public string PartNameCn { get; set; }
        public string PartNameVn { get; set; }
        public string PartNameOther { get; set; }
        public string MaterialNameCn { get; set; }
        public string MaterialNameVn { get; set; }
        public string MaterialNameOther { get; set; }
        public string UnitNameCn { get; set; }
        public string UnitNameVn { get; set; }
        public string UnitNameOther { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
