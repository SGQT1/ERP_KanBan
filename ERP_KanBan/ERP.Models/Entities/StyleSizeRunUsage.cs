using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class StyleSizeRunUsage
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal ArticleSizeRunId { get; set; }
        public decimal StyleId { get; set; }
        public decimal OrdersId { get; set; }
        public decimal MaterialId { get; set; }
        public decimal UnitUsage { get; set; }
        public decimal UnitCodeId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ArticleSizeRun ArticleSizeRun { get; set; }
        public virtual Material Material { get; set; }
        public virtual Style Style { get; set; }
    }
}
