using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsStyleItem
    {
        public MpsStyleItem()
        {
            MpsStyleItemUsage = new HashSet<MpsStyleItemUsage>();
        }

        public decimal Id { get; set; }
        public decimal MpsStyleId { get; set; }
        public string PartNameTw { get; set; }
        public string PartNameEn { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public string UnitNameTw { get; set; }
        public string UnitNameEn { get; set; }
        public string RefKnifeNo { get; set; }
        public int PieceOfPair { get; set; }
        public int AlternateType { get; set; }
        public decimal UsageGiveBegin { get; set; }
        public decimal UsageGiveEnd { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public int Type { get; set; }
        public decimal? TotalUsage { get; set; }

        public virtual MpsStyle MpsStyle { get; set; }
        public virtual ICollection<MpsStyleItemUsage> MpsStyleItemUsage { get; set; }
    }
}
