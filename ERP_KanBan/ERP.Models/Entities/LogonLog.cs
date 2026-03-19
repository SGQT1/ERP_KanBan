using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class LogonLog
    {
        public decimal Id { get; set; }
        public string UserName { get; set; }
        public DateTime LogonTime { get; set; }
        public string FromAddress { get; set; }
        public int Status { get; set; }
    }
}
