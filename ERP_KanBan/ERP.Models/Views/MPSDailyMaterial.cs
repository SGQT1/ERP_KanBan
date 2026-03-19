using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MPSDailyMaterial
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MPSDailyId { get; set; }
        public decimal TotalUsage { get; set; }
        public decimal PreTotalUsage { get; set; }
        public decimal MpsStyleItemId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string PartNameTw { get; set; }
        public string PartNameEn { get; set; }
        public int? PieceOfPair { get; set; }
        public int? AlternateType { get; set; }
        public string RefKnifeNo { get; set; }
        // public decimal? MRPItemId { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public decimal MaterialId { get; set; }
        public string UnitNameTw { get; set; }
        public string UnitNameEn { get; set; }
        public decimal UnitCodeId { get; set; }

        public decimal MpsStyleId { get; set; }
        public string PartNo { get; set; }
    }
}
