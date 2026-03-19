using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class RDMaterialStockBalance
    {
        public decimal LocaleId { get; set; }
        public string MaterialName { get; set; }
        public string WarehouseNo { get; set; }
        public string LocationDesc { get; set; }
        public string Unit { get; set; }
        public decimal? StockInQty { get; set; }
        public decimal? StockOutQty { get; set; }
        public decimal? StockQty { get; set; }
        public decimal Amount { get; set; }
        public string Barcode { get; set; }
    }
}
