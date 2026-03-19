using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class APAddress
    {
        public decimal Id { get; set; }
        public string CompanyNo { get; set; }
        public string APIP { get; set; }
        public string PDFPort { get; set; }
        public int? SeqId { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
