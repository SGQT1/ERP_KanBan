using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class FSContainerItemPre
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string OrderNo { get; set; }
        public int SeqId { get; set; }
        public decimal PlanExportQty { get; set; }
        public decimal? RealExportQty { get; set; }
        public DateTime? PlanBOMDate { get; set; }
        public DateTime? MaterialExportDate { get; set; }
        public DateTime? MaterialArrivalDate { get; set; }
        public DateTime? CutDate { get; set; }
        public string CutLines { get; set; }
        public decimal? CutPlusQty { get; set; }
        public DateTime? PreparationDate { get; set; }
        public decimal? PreparationPlusQty { get; set; }
        public DateTime? StitchDate { get; set; }
        public string StitchLines { get; set; }
        public decimal? StitchPlusQty { get; set; }
        public DateTime? OutsoleDate { get; set; }
        public string OutsoleLines { get; set; }
        public decimal? OutsolePlusQty { get; set; }
        public DateTime? LastDate { get; set; }
        public string LastLines { get; set; }
        public decimal? LastPlusQty { get; set; }
        public DateTime? PlanFinishDate { get; set; }
        public DateTime? PlanInspectDate { get; set; }
        public DateTime? FinishInspectDate { get; set; }
        public decimal? OrderPlusQty { get; set; }
        public string Remark { get; set; }
        public int CaseClose { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public DateTime? PUMAETC { get; set; }
        public DateTime? ETP { get; set; }
        public DateTime? ExFactoryDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
