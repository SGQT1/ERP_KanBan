using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class ArticlePartUsage
    {
        public decimal Id { get; set; }
        public decimal ArticleId { get; set; }
        public int Division { get; set; }
        public string DivisionOther { get; set; }
        public decimal PartId { get; set; }
        public decimal StandardUsage { get; set; }
        public decimal UnitCodeId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public int AlternateType { get; set; }
        public string KnifeNo { get; set; }
        public int? PieceOfPair { get; set; }
        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string PartNameEn { get; set; }
        public string UnitName { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEng { get; set; }
        public decimal MaterialId { get; set; }

        public string ArticleNo { get; set; }
        public string ArticleSizeRun { get; set; }
        public string ArticleHead { get; set; }
        public string ArticleSizeRunId { get; set; }

        public int? HasUsage { get; set; }
    }
}
