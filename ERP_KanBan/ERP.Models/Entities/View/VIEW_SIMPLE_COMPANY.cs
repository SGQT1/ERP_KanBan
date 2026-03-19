using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_SIMPLE_COMPANY
    {
        public string CompanyNo { get; set; }
        public decimal Id { get; set; }
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public string ChineseShortName { get; set; }
        public string EnglishShortName { get; set; }
    }
}
