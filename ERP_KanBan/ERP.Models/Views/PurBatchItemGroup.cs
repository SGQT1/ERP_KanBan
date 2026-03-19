using System;
using System.Collections.Generic;
using System.Text;

using ERP.Models.Views;

namespace ERP.Models.Views
{
    public class PurBatchItemGroup
    {
        public IEnumerable<PurBatchItem> PurBatchItem { get; set; }
        public IEnumerable<MaterialSimpleQuot> MaterialQuot { get; set; }
    }
}