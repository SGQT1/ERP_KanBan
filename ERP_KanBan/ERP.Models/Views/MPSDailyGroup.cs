using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MPSDailyGroup
    {
        public MPSDaily MPSDaily { get; set; }
        public IEnumerable<MPSDailyMaterial> MPSDailyMaterial { get; set; }
        public IEnumerable<MPSDailyMaterialItem> MPSDailyMaterialItem { get; set; }
        // public IEnumerable<MPSStyleItemUsage> MPSStyleItemUsage { get; set; }
        public IEnumerable<MPSDailyMaterialItem> NewDailyMaterialItem { get; set; }
    }
}
