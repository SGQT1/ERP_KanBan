using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_SIZECOUNTRYMAPPING
    {
        public decimal LocaleId { get; set; }
        public decimal Id { get; set; }
        public decimal SizeCountryCodeId { get; set; }
        public decimal ShoeSize { get; set; }
        public string ShoeSizeSuffix { get; set; }
        public decimal InnerShoeSize { get; set; }
        public decimal MappingCodeId { get; set; }
        public decimal MappingSize { get; set; }
        public string MappingSizeSuffix { get; set; }
        public decimal InnerMappingSize { get; set; }
        public string SizeCountryNameTw { get; set; }
        public string MappingSizeCountryNameTw { get; set; }
    }
}
