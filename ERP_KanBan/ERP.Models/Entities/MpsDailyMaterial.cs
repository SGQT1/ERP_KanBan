using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsDailyMaterial
    {
        public MpsDailyMaterial()
        {
            MpsDailyMaterialItem = new HashSet<MpsDailyMaterialItem>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsDailyId { get; set; }
        public decimal TotalUsage { get; set; }
        public decimal PreTotalUsage { get; set; }
        public decimal MpsStyleItemId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string PartNameTw { get; set; }
        public string PartNameEn { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public string UnitNameTw { get; set; }
        public string UnitNameEn { get; set; }
        public int? PieceOfPair { get; set; }
        public int? AlternateType { get; set; }
        public string RefKnifeNo { get; set; }
        public string PartNo { get; set; }
        public virtual ICollection<MpsDailyMaterialItem> MpsDailyMaterialItem { get; set; }
    }
}
