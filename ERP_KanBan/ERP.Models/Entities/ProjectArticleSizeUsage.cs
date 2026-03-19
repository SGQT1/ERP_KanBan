using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ProjectArticleSizeUsage
    {
        public decimal Id { get; set; }
        public decimal ProjectArticlePartId { get; set; }
        public decimal? ShoeSize { get; set; }
        public string Suffix { get; set; }
        public decimal? InnerSize { get; set; }
        public decimal UnitUsage { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual Company Locale { get; set; }
        public virtual ProjectArticlePart ProjectArticlePart { get; set; }
    }
}
