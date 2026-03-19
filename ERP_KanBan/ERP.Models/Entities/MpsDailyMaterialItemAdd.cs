using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsDailyMaterialItemAdd
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsDailyMaterialAddId { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public string DisplaySize { get; set; }
        public decimal LSubQty { get; set; }
        public decimal RSubQty { get; set; }
        public decimal SubQty { get; set; }
        public decimal UnitUsage { get; set; }
        public decimal SubUsage { get; set; }
        public decimal? PreSubUsage { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
