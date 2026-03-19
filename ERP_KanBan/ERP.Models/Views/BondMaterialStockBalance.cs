using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class BondMaterialStockBalance
    {
        public string MaterialName { get; set; }
        public string OrderNo { get; set; }
        public decimal? PCLQty { get; set; }
        public decimal? RealPCLQty { get; set; }
        public string PCLUnitNameTw { get; set; }
        public decimal? BelongCompanyId { get; set; }
        public string BondMaterialName { get; set; }
        public decimal? WeightEachUnit { get; set; }
        public decimal? Weight { get; set; }
        public string VendorShortNameTw { get; set; }
        public string StockDollarNameTw { get; set; }

        public decimal LocaleId { get; set; }
        public decimal MaterialId { get; set; }
        public string WarehouseNo { get; set; }
        public string BondNo { get; set; }
        public string Currency { get; set; }
    }
}
