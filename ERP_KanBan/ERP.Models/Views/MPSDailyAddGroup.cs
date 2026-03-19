using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MPSDailyAddGroup
    {
        public MPSDailyAdd MPSDailyAdd { get; set; }
        public IEnumerable<MPSDailyMaterialAdd> MPSDailyMaterialAdd { get; set; }
        public IEnumerable<MPSDailyMaterialItemAdd> MPSDailyMaterialItemAdd { get; set; }
        public IEnumerable<MPSDailyMaterialItemAdd> NewDailyMaterialItemAdd { get; set; }
    }
}
