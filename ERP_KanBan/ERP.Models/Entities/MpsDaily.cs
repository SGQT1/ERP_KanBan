using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MpsDaily
    {
        public MpsDaily()
        {
            MpsDailyLang = new HashSet<MpsDailyLang>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string DailyNo { get; set; }
        public string PreDailyNo { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime DailyDate { get; set; }
        public DateTime? FinishedDate { get; set; }
        public string OrderNo { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public decimal MaterialId { get; set; }
        public string UnitNameTw { get; set; }
        public string UnitNameEn { get; set; }
        public decimal UnitCodeId { get; set; }
        public decimal OrgUnitId { get; set; }
        public decimal Qty { get; set; }
        public int DailyMode { get; set; }
        public int DailyType { get; set; }
        public int DoDaily { get; set; }
        public DateTime? DoDate { get; set; }
        public decimal MpsLiveItemId { get; set; }
        public decimal Multi { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public int IsForOrders { get; set; }

        public virtual MpsLiveItem MpsLiveItem { get; set; }
        public virtual ICollection<MpsDailyLang> MpsDailyLang { get; set; }
    }
}
