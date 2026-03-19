using System;
using System.Collections.Generic;

namespace ERP.Models.Views {
    public class ShipmentSummary {
        public decimal ArticleToolingCost { get;set; }
        public decimal StyleToolingCost { get;set; }
        public decimal ArticleToolingFund { get;set; }
        public decimal StyleToolingFund { get;set; }
        public decimal ShipmentQtyTotal { get;set; }
        public decimal OrderQty { get;set; }
    }
}