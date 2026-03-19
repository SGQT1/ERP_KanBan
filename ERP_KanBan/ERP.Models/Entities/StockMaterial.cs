using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class StockMaterial
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string MaterialNameEng { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
