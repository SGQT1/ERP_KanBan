using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class BondMaterialStockItem
    {
        public DateTime IODate { get; set; }
        public string OrderNo { get; set; }
        public int SourceType { get; set; }
        public string OrgUnitNameTw { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string PCLUnitNameTw { get; set; }
        
        public decimal PCLIOQty { get; set; }
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal StockQty { get; set; }
        public decimal PurQty { get; set; }
        public decimal? Weight { get; set; }
        public string Currency { get; set; }

        public DateTime? SaleDate { get; set; }
        public decimal? WeightEachUnit { get; set; }
        public string BondMaterialName { get; set; }
    }
}
