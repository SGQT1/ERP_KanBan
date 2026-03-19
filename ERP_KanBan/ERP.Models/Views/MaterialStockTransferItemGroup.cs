using ERP.Models.System.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class MaterialStockTransferItemGroup
    {
        public MaterialStockItem MaterialStockItemOut { get; set; }
        public MaterialStockItem MaterialStockItemIn { get; set; }
        public IEnumerable<MaterialStockItemSize> MaterialStockItemSize { get; set; }
    }
}
