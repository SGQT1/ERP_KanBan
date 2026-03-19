using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class PowerOnLog
    {
        public decimal Id { get; set; }
        public string CompanyNo { get; set; }
        public DateTime PingDate { get; set; }
        public string IPAdress { get; set; }
        public string HostName { get; set; }
        public string ReplyMsg { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
