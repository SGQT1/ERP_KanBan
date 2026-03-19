using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class SizeMappingGroup
    {
        public SizeMapping SizeMapping { get; set; }
        public IEnumerable<SizeMappingItem> SizeMappingItem { get; set; }
    }
}
