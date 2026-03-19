using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class View_StyleItemForCBD
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
        public Guid msrepl_tran_version { get; set; }
        public int? Division { get; set; }
        public string DivisionOther { get; set; }
        public decimal? PartId { get; set; }
        public decimal? UnitCodeId { get; set; }
        public decimal? StandardUsage { get; set; }
        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string PartNameEn { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public decimal? CategoryCodeId { get; set; }
        public string CategoryNameTw { get; set; }
        public string CategoryNameEn { get; set; }
        public decimal? ArticleId { get; set; }
        public int? AlternateType { get; set; }
        public string UnitNameTw { get; set; }
        public string UnitNameEn { get; set; }
        public string KnifeNo { get; set; }
        public int? PieceOfPair { get; set; }
        public int doMRP { get; set; }
        public decimal? UnitUsage { get; set; }
        public string ArticleNo { get; set; }
        public string ArticleName { get; set; }
        public decimal? SamplePrice { get; set; }
        public string SampleDollar { get; set; }
        public decimal? ProductionPrice { get; set; }
        public string ProductionDollar { get; set; }
        public decimal? USDExchangeRate { get; set; }
        public string Season { get; set; }
        public string StyleNo { get; set; }
        public string BrandTw { get; set; }
        public decimal? CBDPrice { get; set; }
        public string SampleSize { get; set; }
        public string SampleSizeSuffix { get; set; }
        public decimal? SampleInnerSize { get; set; }
    }
}
