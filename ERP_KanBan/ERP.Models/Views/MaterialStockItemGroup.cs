using ERP.Models.System.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class MaterialStockItemGroup
    {
        public MaterialStockItem MaterialStockItem { get; set; }
        public IEnumerable<Models.Views.MaterialStockItemSize> MaterialStockItemSize { get; set; }
        public MaterialStockItemPO MaterialStockItemPO { get; set; }
        public IEnumerable<Models.Views.OutsourcePOItem> OutsourcePOItem { get; set; }
    }
}
