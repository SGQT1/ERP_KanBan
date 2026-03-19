using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ChipProductOutRefNoItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public DateTime? RefDate { get; set; }
        public string RefNo { get; set; }
        public string StyleNo { get; set; }
        public string ShoeSizeDisplay { get; set; }
        public decimal LQty { get; set; }
        public decimal RQty { get; set; }
        public int Class { get; set; }
        public string ForUserNameTw { get; set; }
        public string ForWhat { get; set; }
        public int doOut { get; set; }
        public string doUserNameTw { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
