using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class PackMapping
    {
        public PackMapping()
        {
            PackMappingItem1 = new HashSet<PackMappingItem1>();
            PackMappingItem2 = new HashSet<PackMappingItem2>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ArticleNo { get; set; }
        public string ShoeName { get; set; }
        public string SizeCountryNameTw { get; set; }
        public string WeightUnitNameTw { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<PackMappingItem1> PackMappingItem1 { get; set; }
        public virtual ICollection<PackMappingItem2> PackMappingItem2 { get; set; }
    }
}
