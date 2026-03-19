using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable
//拖外報價單
namespace ERP.Models.Views
{
    public partial class MPSOutsourceForQuot
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string StyleNo { get; set; }
        public string GroupNameTw { get; set; }
        public string GroupNameLocal { get; set; }
        public string GroupNameEn { get; set; }
        public string DollarNameTw { get; set; }
        public decimal? UnitStandardTime { get; set; }
        public decimal? UnitLaborCost { get; set; }
        public decimal? HasQuot { get; set; }

        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
