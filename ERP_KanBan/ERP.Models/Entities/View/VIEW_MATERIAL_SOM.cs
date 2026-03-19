using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_MATERIAL_SOM
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public decimal? ParentId { get; set; }
        public decimal? ChildId { get; set; }
        public string MaterialNameTwChild { get; set; }
        public string MaterialNameEnChild { get; set; }
        public int? SemiGoods { get; set; }
        public decimal? SOMId { get; set; }
    }
}
