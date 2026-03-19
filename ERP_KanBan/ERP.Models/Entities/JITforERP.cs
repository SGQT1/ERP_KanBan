using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class JITforERP
    {
        public decimal Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime DateIn { get; set; }
        public string LinesNo { get; set; }
        public string LinesName { get; set; }
        public string ShoeSize { get; set; }
        public decimal QtyIn { get; set; }
        public int? Process { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
