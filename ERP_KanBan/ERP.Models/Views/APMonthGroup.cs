using ERP.Models.System.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class APMonthGroup: BaseEntity
    {
        public APMonth APMonth { get; set; }
        public IEnumerable<APMonthItem> APMonthItem { get; set; }
        public IEnumerable<APMonthItemDiscount> APMonthItemDiscount { get; set; }
    }
}
