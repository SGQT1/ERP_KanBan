using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VirtualCompany
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string CompanyNo { get; set; }
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public string ShortName { get; set; }
        public string RegLocaleNameTw { get; set; }
        public string TelNo { get; set; }
        public string FaxNo { get; set; }
        public string ChineseAddress { get; set; }
        public string EnglishAddress { get; set; }
        public string UnifiedNo { get; set; }
        public string TaxNo { get; set; }
        public string Remark { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string ModifyUserName { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
