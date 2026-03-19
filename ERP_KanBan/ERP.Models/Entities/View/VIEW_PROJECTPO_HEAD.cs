using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_PROJECTPO_HEAD
    {
        public decimal LocaleId { get; set; }
        public decimal VendorId { get; set; }
        public string ShortNameTw { get; set; }
        public string NameTw { get; set; }
        public string NameEn { get; set; }
        public DateTime ProjectPODate { get; set; }
        public string ProjectPONo { get; set; }
    }
}
