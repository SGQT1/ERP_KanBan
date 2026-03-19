using ERP.Models.System.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class MaterialStockGroup: BaseEntity
    {
        public MaterialStock MaterialStock { get; set; }
        public IEnumerable<MaterialStockItem> MaterialStockItem { get; set; }
        public IEnumerable<MaterialStockSize> MaterialStockSize { get; set; }
    }
}
