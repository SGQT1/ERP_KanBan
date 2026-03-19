using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_MPSDAILYMATERIALADD
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsDailyAddId { get; set; }
        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string MaterialNameTw { get; set; }
        public string UnitNameTw { get; set; }
        public int AlternateType { get; set; }
        public decimal? PreTotalUsage { get; set; }
        public decimal? SubMulti { get; set; }
        public decimal TotalUsage { get; set; }
        public decimal? ProceduresId { get; set; }
        public decimal MpsStyleItemId { get; set; }
        public string WarehouseNo { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string MaterialNameEng { get; set; }
        public string PartNameEng { get; set; }
        public string UnitNameEng { get; set; }
    }
}
