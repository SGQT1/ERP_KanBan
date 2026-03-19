using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_UNITTRANSRATE
    {
        public decimal LocaleId { get; set; }
        public decimal CodeId { get; set; }
        public string UnitNameTw { get; set; }
        public decimal TransCodeId { get; set; }
        public string TransUnitNameTw { get; set; }
        public decimal Rate { get; set; }
        public decimal ReversedRate { get; set; }
    }
}
