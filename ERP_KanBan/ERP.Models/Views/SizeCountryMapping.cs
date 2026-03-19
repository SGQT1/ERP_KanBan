using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class SizeCountryMapping
    {
        public decimal Id { get; set; }
        public decimal SizeCountryCodeId { get; set; }
        public decimal ShoeSize { get; set; }
        public string ShoeSizeSuffix { get; set; }
        public decimal InnerShoeSize { get; set; }
        public decimal MappingCodeId { get; set; }
        public decimal MappingSize { get; set; }
        public string MappingSizeSuffix { get; set; }
        public decimal InnerMappingSize { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
    }
}
