using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class Port
    {
        public decimal Id { get; set; }
        public int PortNo { get; set; }
        public string PortName { get; set; }
        public decimal PortVarietyCodeId { get; set; }
        public string PortNameEng { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
    }
}
