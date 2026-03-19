using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsStyle
    {
        public MpsStyle()
        {
            MpsProcedure = new HashSet<MpsProcedure>();
            MpsStyleItem = new HashSet<MpsStyleItem>();
            MpsStyleItemSet = new HashSet<MpsStyleItemSet>();
            MpsStyleUsageUpdateLog = new HashSet<MpsStyleUsageUpdateLog>();
        }

        public decimal Id { get; set; }
        public decimal MpsArticleId { get; set; }
        public string StyleNo { get; set; }
        public string ColorDesc { get; set; }
        public decimal LocaleId { get; set; }
        public decimal SizeCountryCodeId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string RefOrderNo { get; set; }
        public int DoUsage { get; set; }
        public string DollarNameTw { get; set; }
        public decimal? UnitRelaxTime { get; set; }
        public decimal? UnitStandardTime { get; set; }
        public decimal? UnitLaborCost { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string RefOrderNoOfMaterial { get; set; }

        public virtual MpsArticle MpsArticle { get; set; }
        public virtual ICollection<MpsProcedure> MpsProcedure { get; set; }
        public virtual ICollection<MpsStyleItem> MpsStyleItem { get; set; }
        public virtual ICollection<MpsStyleItemSet> MpsStyleItemSet { get; set; }
        public virtual ICollection<MpsStyleUsageUpdateLog> MpsStyleUsageUpdateLog { get; set; }
    }
}
