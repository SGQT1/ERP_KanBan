using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ProjectArticleItem
    {
        public decimal Id { get; set; }
        public decimal? ProjectId { get; set; }
        public decimal ProjectPartId { get; set; }
        public decimal? MaterialId { get; set; }
        public decimal? VendorId { get; set; }
        public decimal? UnitPrice { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal? DollarCodeId { get; set; }
        public string TempMaterialName { get; set; }
        public decimal LocaleId { get; set; }
        public int? Enable { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual Company Locale { get; set; }
        public virtual Material Material { get; set; }
        public virtual Project Project { get; set; }
        public virtual ProjectArticlePart ProjectArticlePart { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
