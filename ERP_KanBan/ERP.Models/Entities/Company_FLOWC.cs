using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class Company_FLOWC
    {
        public decimal? Id { get; set; }
        public string CompanyNo { get; set; }
        public string F_INP_STAT { get; set; }
        public string F_INP_ID { get; set; }
        public string F_INP_TIME { get; set; }
        public string F_INP_INFO { get; set; }
    }
}
