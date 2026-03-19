using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class Part
    {
        public Part()
        {
            ArticlePart = new HashSet<ArticlePart>();
            ArticlePartUsageUpdateLog = new HashSet<ArticlePartUsageUpdateLog>();
            ProjectArticlePart = new HashSet<ProjectArticlePart>();
        }

        public decimal Id { get; set; }
        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string PartNameEn { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual Company Locale { get; set; }
        public virtual ICollection<ArticlePart> ArticlePart { get; set; }
        public virtual ICollection<ArticlePartUsageUpdateLog> ArticlePartUsageUpdateLog { get; set; }
        public virtual ICollection<ProjectArticlePart> ProjectArticlePart { get; set; }
    }
}
