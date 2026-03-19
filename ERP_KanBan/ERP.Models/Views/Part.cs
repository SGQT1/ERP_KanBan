using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class Part
    {
        public decimal Id { get; set; }
        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string PartNameEn { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
    }
}
