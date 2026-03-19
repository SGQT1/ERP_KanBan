using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class BatchOrdersCostGroup
    {
        public BatchOrdersCost BatchOrdersCost { get; set; }                      // BatchMaterialOrders + BatchProductionCost
        public IEnumerable<BatchOrdersCostItem> BatchOrdersCostItem { get; set; } // BatchMaterialCost
        public IEnumerable<ExchangeRate> ExchangeRate { get; set; }
        public IEnumerable<MPSProcedurePO> MPSOutsource  { get; set; }    //MPSProcedurePO
    }
}
