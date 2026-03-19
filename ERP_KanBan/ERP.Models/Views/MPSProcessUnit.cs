using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MPSProcessUnit
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string UnitNo { get; set; }
        public string UnitNameTw { get; set; }
        public string UnitNameEn { get; set; }
        public string UnitNameCn { get; set; }
        public string UnitNameVn { get; set; }
        public int? UnitType { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
