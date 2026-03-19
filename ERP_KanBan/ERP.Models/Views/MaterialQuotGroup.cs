using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MaterialQuotGroup
    {
        public Material Material { get;set; }
        public IEnumerable<MaterialQuot> MaterialQuot { get;set; }
        
    }
}
