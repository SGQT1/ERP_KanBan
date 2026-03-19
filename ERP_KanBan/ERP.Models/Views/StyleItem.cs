using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class StyleItem
    {
        public decimal Id { get; set; }
        public decimal StyleId { get; set; }
        public decimal ArticlePartId { get; set; }
        public decimal MaterialId { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public int EnableMaterial { get; set; }


        public decimal PartId { get; set; }
        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string PartNameEn { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public int? PieceOfPair { get; set; }
        public string KnifeNo { get; set; }
        public string StyleNo { get; set; }
        public decimal ArticleId { get; set; }
        public decimal AlternateType { get; set; }

        public int Division { get; set; }
        public string DivisionOther { get; set; }
        public decimal UnitCodeId { get; set; }
        public string UnitCode { get; set; }
    }
}
