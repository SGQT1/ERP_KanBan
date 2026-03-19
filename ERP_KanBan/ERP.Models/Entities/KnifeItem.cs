using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class KnifeItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal KnifeId { get; set; }
        public decimal ShoeSize { get; set; }
        public string ShoeSizeSuffix { get; set; }
        public decimal ShoeSizeSortKey { get; set; }
        public int Qty { get; set; }
        public DateTime MadeDate { get; set; }
        public decimal Cost { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual Knife Knife { get; set; }
    }
}
