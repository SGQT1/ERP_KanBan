using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class Article
    {
        public Article()
        {
            ArticlePart = new HashSet<ArticlePart>();
            ArticlePartUsageUpdateLog = new HashSet<ArticlePartUsageUpdateLog>();
            ArticleSizeRun = new HashSet<ArticleSizeRun>();
            Style = new HashSet<Style>();
        }

        public decimal Id { get; set; }
        public string ArticleNo { get; set; }
        public string ArticleName { get; set; }
        public int SizeRange { get; set; }
        public string OtherRangeDesc { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public decimal ProjectType { get; set; }
        public decimal BrandCodeId { get; set; }
        public decimal? OutsoleId { get; set; }
        public decimal? ShellId { get; set; }
        public decimal? KnifeId { get; set; }
        public decimal? LastId { get; set; }
        public decimal? DayCapacity { get; set; }
        public int? LastTurnover { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string Gender { get; set; }
        public int? forCBD { get; set; }
        public int IsAlternate { get; set; }

        public virtual ICollection<ArticlePart> ArticlePart { get; set; }
        public virtual ICollection<ArticlePartUsageUpdateLog> ArticlePartUsageUpdateLog { get; set; }
        public virtual ICollection<ArticleSizeRun> ArticleSizeRun { get; set; }
        public virtual ICollection<Style> Style { get; set; }
    }
}
