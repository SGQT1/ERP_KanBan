using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ProjectArticlePart
    {
        public ProjectArticlePart()
        {
            ProjectArticleItem = new HashSet<ProjectArticleItem>();
            ProjectArticleSizeUsage = new HashSet<ProjectArticleSizeUsage>();
        }

        public decimal Id { get; set; }
        public decimal ProjectType { get; set; }
        public string ArticleNo { get; set; }
        public int Division { get; set; }
        public string DivisionOther { get; set; }
        public decimal PartId { get; set; }
        public string SeqNo { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal? UnitCodeId { get; set; }
        public decimal? DefaultUsage { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual Company Locale { get; set; }
        public virtual Part Part { get; set; }
        public virtual ICollection<ProjectArticleItem> ProjectArticleItem { get; set; }
        public virtual ICollection<ProjectArticleSizeUsage> ProjectArticleSizeUsage { get; set; }
    }
}
