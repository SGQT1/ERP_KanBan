using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class SPBOM
    {
        public decimal Id { get; set; }
        public decimal BOMLocaleId { get; set; }
        public int CSDMonth { get; set; }
        public string OrderNo { get; set; }
        public decimal Counts { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime MinFinishTime { get; set; }
        public DateTime MaxFinishTime { get; set; }
        public decimal DiffDays { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
