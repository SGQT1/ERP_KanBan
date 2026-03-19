using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ArticlePart
    {
        public ArticlePart()
        {
            ArticleSizeRunUsage = new HashSet<ArticleSizeRunUsage>();
        }

        public decimal Id { get; set; }
        public decimal ArticleId { get; set; }
        public int Division { get; set; }
        public string DivisionOther { get; set; }
        public decimal PartId { get; set; }
        public decimal StandardUsage { get; set; }
        public decimal UnitCodeId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public int AlternateType { get; set; }
        public string PartPhotoURL { get; set; }
        public string KnifeNo { get; set; }
        public int? PieceOfPair { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual Article Article { get; set; }
        public virtual Part Part { get; set; }
        public virtual ICollection<ArticleSizeRunUsage> ArticleSizeRunUsage { get; set; }
    }
}
