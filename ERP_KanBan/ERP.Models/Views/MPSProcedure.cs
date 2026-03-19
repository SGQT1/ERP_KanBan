using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable
// 發外部位
namespace ERP.Models.Views
{
    public partial class MPSProcedure
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsStyleId { get; set; }
        public decimal? MpsStyleItemId { get; set; }
        public decimal ProcessId { get; set; }
        public decimal ProceduresId { get; set; }
        public decimal? PreProceduresId { get; set; }
        public string InProcessNo { get; set; }
        public int CountType { get; set; }
        public decimal PieceWorker { get; set; }
        public decimal PieceStandardPrice { get; set; }
        public decimal PieceStandardTime { get; set; }
        public decimal PiecePairs { get; set; }
        public decimal PairsStandardTime { get; set; }
        public decimal PairsStandardPrice { get; set; }
        public decimal? AccomplishRate { get; set; }
        public int ToStock { get; set; }
        public decimal SortKey { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public string MpsStyleNo { get; set; }
        public string ProcessNameTw { get; set; }
        public string ProcedureNameTw { get; set; }

    }
}
