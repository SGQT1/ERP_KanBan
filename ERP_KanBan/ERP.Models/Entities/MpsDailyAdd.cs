using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsDailyAdd
    {
        public MpsDailyAdd()
        {
            MpsDailyAddCost = new HashSet<MpsDailyAddCost>();
            MpsDailyAddDuty = new HashSet<MpsDailyAddDuty>();
            MpsDailyMaterialAdd = new HashSet<MpsDailyMaterialAdd>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string DailyNo { get; set; }
        public string PreDailyNo { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime DailyDate { get; set; }
        public DateTime? FinishedDate { get; set; }
        public decimal ProcessId { get; set; }
        public decimal ProcessUnitId { get; set; }
        public string OrderNo { get; set; }
        public decimal MpsStyleId { get; set; }
        public decimal OrderQty { get; set; }
        public decimal? Qty { get; set; }
        public string SizeCountryNameTw { get; set; }
        public int DailyMode { get; set; }
        public int DailyType { get; set; }
        public int DoDaily { get; set; }
        public DateTime? DoDate { get; set; }
        public decimal Multi { get; set; }
        public string Remark { get; set; }
        public int SeqId { get; set; }
        public decimal? MaterialCost { get; set; }
        public string DollarNameTw { get; set; }
        public int? CostBalance { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<MpsDailyAddCost> MpsDailyAddCost { get; set; }
        public virtual ICollection<MpsDailyAddDuty> MpsDailyAddDuty { get; set; }
        public virtual ICollection<MpsDailyMaterialAdd> MpsDailyMaterialAdd { get; set; }
    }
}
