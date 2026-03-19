using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSStyleItem
    {
        public decimal Id { get; set; }
        public decimal MpsStyleId { get; set; }
        public string PartNo { get; set; }
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
        public int Type { get; set; }
        public decimal? TotalUsage { get; set; }
        public decimal? MRPItemId { get; set; }
        public decimal? ProcessId { get; set; }
        public decimal? ProceduresId { get; set; }
        public decimal? PreProceduresId { get; set; } 
        public decimal? ToStock { get; set; } 
        public decimal? AccomplishRate { get; set;}
        public decimal? CountType { get; set;}
        public decimal? PieceWorker { get; set;}
        public decimal? PieceStandardTime { get; set;}
        public decimal? PieceStandardPrice { get; set;}
        public decimal? PiecePairs { get; set;}
        public string InProcessNo { get; set;}
        public string ProcedureNo { get; set; }

        public decimal? StyleItemId { get; set; }
        public decimal? PartId { get; set; }
        public int? HasUsage { get; set; }
        public string StyleNo { get; set; }
    }
}
